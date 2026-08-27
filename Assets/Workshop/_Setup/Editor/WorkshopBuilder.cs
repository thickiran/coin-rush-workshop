using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Workshop.Setup
{
    /// <summary>
    /// Builds every exercise checkpoint scene, prefab, and material for the
    /// Coin Rush workshop. Idempotent: run it again to rebuild everything.
    /// Invoke from the menu (Workshop > Build All Checkpoints) or via CLI eval:
    ///   unity command eval 'Workshop.Setup.WorkshopBuilder.BuildAll();'
    /// </summary>
    public static class WorkshopBuilder
    {
        const string Root = "Assets/Workshop";
        const string MatDir = Root + "/_Shared/Materials";

        class Stage
        {
            public string Folder;
            public string Ns;
            public int Level;          // cumulative feature level (1..9)
            public bool BuggedCoin;    // Ex06 ships with the double-collider bug
            public string SceneName;
        }

        static readonly Stage[] Stages =
        {
            new Stage { Folder = "Ex01_SceneSetup",  Ns = null,                Level = 1, SceneName = "Ex01_Start" },
            new Stage { Folder = "Ex02_Movement",    Ns = null,                Level = 2, SceneName = "Ex02_Start" },
            new Stage { Folder = "Ex03_Coins",       Ns = "Workshop.Ex03",     Level = 3, SceneName = "Ex03_Start" },
            new Stage { Folder = "Ex04_Score",       Ns = "Workshop.Ex04",     Level = 4, SceneName = "Ex04_Start" },
            new Stage { Folder = "Ex05_Hazard",      Ns = "Workshop.Ex05",     Level = 5, SceneName = "Ex05_Start" },
            new Stage { Folder = "Ex06_BugHunt",     Ns = "Workshop.Ex06",     Level = 6, BuggedCoin = true, SceneName = "Ex06_Start" },
            new Stage { Folder = "Ex07_CliCommand",  Ns = "Workshop.Ex07",     Level = 6, SceneName = "Ex07_Start" },
            new Stage { Folder = "Ex08_Magnet",      Ns = "Workshop.Ex08",     Level = 6, SceneName = "Ex08_Start" },
            new Stage { Folder = "Ex09_MobileBuild", Ns = "Workshop.Ex09",     Level = 9, SceneName = "Ex09_Start" },
            new Stage { Folder = "_Complete",        Ns = "Workshop.Complete", Level = 9, SceneName = "CoinRush" },
        };

        [MenuItem("Workshop/Build All Checkpoints")]
        public static void BuildAll()
        {
            BuildMaterials();
            var scenePaths = new List<string>();
            foreach (var stage in Stages)
            {
                string path = BuildStage(stage);
                scenePaths.Add(path);
                Debug.Log("[WorkshopBuilder] built " + path);
            }
            var entries = new List<EditorBuildSettingsScene>();
            foreach (var p in scenePaths) entries.Add(new EditorBuildSettingsScene(p, true));
            EditorBuildSettings.scenes = entries.ToArray();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[WorkshopBuilder] DONE — " + scenePaths.Count + " scenes in build settings");
        }

        // ------------------------------------------------------------ materials
        static void BuildMaterials()
        {
            MakeMat("Ground", new Color(0.42f, 0.55f, 0.42f));
            MakeMat("Wall",   new Color(0.25f, 0.28f, 0.34f));
            MakeMat("Player", new Color(0.25f, 0.5f, 0.9f));
            MakeMat("Coin",   new Color(1f, 0.78f, 0.12f), emissive: true);
            MakeMat("Hazard", new Color(0.85f, 0.2f, 0.2f));
            MakeMat("Magnet", new Color(0.62f, 0.3f, 0.85f), emissive: true);

            string artPath = MatDir + "/ArtVertexColor.mat";
            if (AssetDatabase.LoadAssetAtPath<Material>(artPath) == null)
            {
                var shader = Shader.Find("Workshop/VertexColorLit");
                if (shader != null)
                    AssetDatabase.CreateAsset(new Material(shader), artPath);
            }
        }

        static Material MakeMat(string name, Color c, bool emissive = false)
        {
            string path = MatDir + "/" + name + ".mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(mat, path);
            }
            mat.SetColor("_BaseColor", c);
            if (emissive)
            {
                mat.EnableKeyword("_EMISSION");
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                mat.SetColor("_EmissionColor", c * 0.4f);
            }
            EditorUtility.SetDirty(mat);
            return mat;
        }

        static Material Mat(string name) => AssetDatabase.LoadAssetAtPath<Material>(MatDir + "/" + name + ".mat");

        /// <summary>Polyfork model from _Shared/Art (.glb via glTFast, .fbx fallback), or null → primitives.</summary>
        static GameObject Art(string fileName)
        {
            var glb = AssetDatabase.LoadAssetAtPath<GameObject>(Root + "/_Shared/Art/" + fileName + ".glb");
            if (glb != null) return glb;
            return AssetDatabase.LoadAssetAtPath<GameObject>(Root + "/_Shared/Art/" + fileName + ".fbx");
        }

        static GameObject Model(GameObject asset, Transform parent, Vector3 localPos, float scale)
        {
            var inst = (GameObject)PrefabUtility.InstantiatePrefab(asset);
            inst.transform.SetParent(parent, false);
            inst.transform.localPosition = localPos;
            inst.transform.localScale = Vector3.one * scale;
            foreach (var c in inst.GetComponentsInChildren<Collider>())
                UnityEngine.Object.DestroyImmediate(c);
            // Polyfork palettes live in vertex colors; the glTF import shader
            // ignores them, so all art renders through our vertex-color shader.
            var artMat = Mat("ArtVertexColor");
            if (artMat != null)
            {
                foreach (var r in inst.GetComponentsInChildren<Renderer>())
                {
                    var mats = new Material[r.sharedMaterials.Length];
                    for (int i = 0; i < mats.Length; i++) mats[i] = artMat;
                    r.sharedMaterials = mats;
                }
            }
            return inst;
        }

        // ------------------------------------------------------------ stages
        static string BuildStage(Stage stage)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            var cam = Camera.main;
            cam.transform.position = new Vector3(0f, 13f, -11f);
            cam.transform.rotation = Quaternion.Euler(50f, 0f, 0f);
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.53f, 0.71f, 0.84f);

            GameObject player = null;

            if (stage.Level >= 2)
            {
                BuildArena();
                BuildDecor();
                player = BuildPlayer();
            }

            if (stage.Level >= 3)
                player.AddComponent(FindType(stage.Ns + ".PlayerController"));

            if (stage.Level >= 4)
            {
                var coinPrefab = BuildCoinPrefab(stage);
                PlaceCoins(coinPrefab);
            }

            GameObject winPanel = null, losePanel = null;
            Component gm = null, score = null;
            if (stage.Level >= 5)
            {
                var canvas = BuildCanvas(out Text scoreText, out winPanel, out losePanel, stage.Level >= 6);

                var managers = new GameObject("Managers");
                score = managers.AddComponent(FindType(stage.Ns + ".ScoreManager"));
                gm = managers.AddComponent(FindType(stage.Ns + ".GameManager"));
                SetField(score, "scoreText", scoreText);
                SetField(gm, "winPanel", winPanel);
                SetField(gm, "losePanel", losePanel);

                WireRestart(winPanel, gm);
                if (losePanel != null) WireRestart(losePanel, gm);
            }

            if (stage.Level >= 6)
            {
                var mine = Art("space-mine-3a1654");
                GameObject hazard;
                if (mine != null)
                {
                    hazard = new GameObject("Hazard");
                    hazard.transform.position = new Vector3(-6f, 0.6f, -3.5f);
                    var col = hazard.AddComponent<BoxCollider>();
                    col.isTrigger = true;
                    col.size = new Vector3(1.4f, 1.4f, 1.4f);
                    Model(mine, hazard.transform, new Vector3(0f, -0.6f, 0f), 1f);
                }
                else
                {
                    hazard = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    hazard.name = "Hazard";
                    hazard.transform.position = new Vector3(-6f, 0.6f, -3.5f);
                    hazard.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    hazard.GetComponent<Renderer>().sharedMaterial = Mat("Hazard");
                    hazard.GetComponent<BoxCollider>().isTrigger = true;
                }
                hazard.AddComponent(FindType(stage.Ns + ".Hazard"));
            }

            if (stage.Level >= 9)
            {
                player.AddComponent(FindType(stage.Ns + ".PlayerMagnet"));
                var magnetPrefab = BuildMagnetPrefab(stage);
                var inst = (GameObject)PrefabUtility.InstantiatePrefab(magnetPrefab);
                inst.transform.position = new Vector3(-6f, 0.6f, -6f);
            }

            string dir = Root + "/" + stage.Folder;
            string path = dir + "/" + stage.SceneName + ".unity";
            EditorSceneManager.SaveScene(scene, path);
            return path;
        }

        // ------------------------------------------------------------ pieces
        static void BuildArena()
        {
            var arena = new GameObject("Arena");

            var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.name = "Ground";
            ground.transform.SetParent(arena.transform);
            ground.transform.position = new Vector3(0f, -0.25f, 0f);
            ground.transform.localScale = new Vector3(20f, 0.5f, 20f);
            ground.GetComponent<Renderer>().sharedMaterial = Mat("Ground");

            var walls = new (Vector3 pos, Vector3 scale)[]
            {
                (new Vector3(0f, 0.5f, 10.25f), new Vector3(20.5f, 1.5f, 0.5f)),
                (new Vector3(0f, 0.5f, -10.25f), new Vector3(20.5f, 1.5f, 0.5f)),
                (new Vector3(10.25f, 0.5f, 0f), new Vector3(0.5f, 1.5f, 20.5f)),
                (new Vector3(-10.25f, 0.5f, 0f), new Vector3(0.5f, 1.5f, 20.5f)),
            };
            for (int i = 0; i < walls.Length; i++)
            {
                var w = GameObject.CreatePrimitive(PrimitiveType.Cube);
                w.name = "Wall_" + i;
                w.transform.SetParent(arena.transform);
                w.transform.position = walls[i].pos;
                w.transform.localScale = walls[i].scale;
                w.GetComponent<Renderer>().sharedMaterial = Mat("Wall");
            }
        }

        static GameObject BuildPlayer()
        {
            var bot = Art("companion-bot-170074");
            GameObject player;
            if (bot != null)
            {
                player = new GameObject("Player");
                player.transform.position = new Vector3(0f, 1f, 0f);
                var col = player.AddComponent<CapsuleCollider>();
                col.height = 2f;
                col.radius = 0.5f;
                Model(bot, player.transform, new Vector3(0f, -1f, 0f), 1.4f);
            }
            else
            {
                player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                player.name = "Player";
                player.transform.position = new Vector3(0f, 1f, 0f);
                player.GetComponent<Renderer>().sharedMaterial = Mat("Player");

                var nose = GameObject.CreatePrimitive(PrimitiveType.Cube);
                nose.name = "FacingIndicator";
                UnityEngine.Object.DestroyImmediate(nose.GetComponent<Collider>());
                nose.transform.SetParent(player.transform);
                nose.transform.localPosition = new Vector3(0f, 0.35f, 0.45f);
                nose.transform.localScale = new Vector3(0.2f, 0.2f, 0.3f);
                nose.GetComponent<Renderer>().sharedMaterial = Mat("Player");
            }

            player.tag = "Player";
            var rb = player.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            return player;
        }

        static void BuildDecor()
        {
            var tree = Art("maple-tree-65fa12");
            if (tree == null) return;
            var decor = new GameObject("Decor");
            var spots = new[]
            {
                new Vector3(-8.3f, 0f, 8.3f), new Vector3(8.3f, 0f, 8.3f),
                new Vector3(-8.3f, 0f, -8.3f), new Vector3(8.3f, 0f, -8.3f),
            };
            for (int i = 0; i < spots.Length; i++)
            {
                var t = Model(tree, decor.transform, spots[i], 0.7f);
                t.transform.localRotation = Quaternion.Euler(0f, i * 85f, 0f);
            }
        }

        static GameObject BuildCoinPrefab(Stage stage)
        {
            var root = new GameObject("Coin");
            root.AddComponent(FindType(stage.Ns + ".CoinPickup"));
            var col = root.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 0.6f;
            if (stage.BuggedCoin)
            {
                // Seeded bug for Ex06: a second identical trigger collider makes
                // OnTriggerEnter fire twice, so every coin scores double.
                var extra = root.AddComponent<SphereCollider>();
                extra.isTrigger = true;
                extra.radius = 0.6f;
            }

            var visual = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            visual.name = "Visual";
            UnityEngine.Object.DestroyImmediate(visual.GetComponent<Collider>());
            visual.transform.SetParent(root.transform);
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            visual.transform.localScale = new Vector3(0.9f, 0.06f, 0.9f);
            visual.GetComponent<Renderer>().sharedMaterial = Mat("Coin");

            string dir = Root + "/" + stage.Folder + "/Prefabs";
            if (!AssetDatabase.IsValidFolder(dir))
                AssetDatabase.CreateFolder(Root + "/" + stage.Folder, "Prefabs");
            var prefab = PrefabUtility.SaveAsPrefabAsset(root, dir + "/Coin.prefab");
            UnityEngine.Object.DestroyImmediate(root);
            return prefab;
        }

        static void PlaceCoins(GameObject prefab)
        {
            var parent = new GameObject("Coins");
            const int count = 8;
            for (int i = 0; i < count; i++)
            {
                float angle = i * Mathf.PI * 2f / count;
                var coin = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                coin.transform.SetParent(parent.transform);
                coin.transform.position = new Vector3(Mathf.Cos(angle) * 6f, 0.6f, Mathf.Sin(angle) * 6f);
            }
        }

        static GameObject BuildMagnetPrefab(Stage stage)
        {
            var root = new GameObject("MagnetPickup");
            root.AddComponent(FindType(stage.Ns + ".MagnetPickup"));
            var col = root.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 0.6f;

            var crystal = Art("crystal-cluster-624138");
            if (crystal != null)
            {
                Model(crystal, root.transform, new Vector3(0f, -0.6f, 0f), 1.5f);
            }
            else
            {
                var visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
                visual.name = "Visual";
                UnityEngine.Object.DestroyImmediate(visual.GetComponent<Collider>());
                visual.transform.SetParent(root.transform);
                visual.transform.localPosition = Vector3.zero;
                visual.transform.localRotation = Quaternion.Euler(45f, 45f, 0f);
                visual.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                visual.GetComponent<Renderer>().sharedMaterial = Mat("Magnet");
            }

            string dir = Root + "/" + stage.Folder + "/Prefabs";
            if (!AssetDatabase.IsValidFolder(dir))
                AssetDatabase.CreateFolder(Root + "/" + stage.Folder, "Prefabs");
            var prefab = PrefabUtility.SaveAsPrefabAsset(root, dir + "/MagnetPickup.prefab");
            UnityEngine.Object.DestroyImmediate(root);
            return prefab;
        }

        // ------------------------------------------------------------ UI
        static GameObject BuildCanvas(out Text scoreText, out GameObject winPanel, out GameObject losePanel, bool withLose)
        {
            var canvasGo = new GameObject("Canvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasGo.AddComponent<GraphicRaycaster>();

            var es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();

            scoreText = MakeText(canvasGo.transform, "ScoreText", "Coins: 0 / 0", 44, TextAnchor.UpperLeft);
            var rt = scoreText.rectTransform;
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = new Vector2(30f, -25f);
            rt.sizeDelta = new Vector2(600f, 80f);

            winPanel = MakePanel(canvasGo.transform, "WinPanel", "YOU WIN!", new Color(0.1f, 0.35f, 0.15f, 0.85f));
            losePanel = null;
            if (withLose)
                losePanel = MakePanel(canvasGo.transform, "LosePanel", "GAME OVER", new Color(0.35f, 0.1f, 0.1f, 0.85f));
            return canvasGo;
        }

        static Text MakeText(Transform parent, string name, string content, int size, TextAnchor anchor)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var text = go.AddComponent<Text>();
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = size;
            text.fontStyle = FontStyle.Bold;
            text.alignment = anchor;
            text.color = Color.white;
            var shadow = go.AddComponent<Shadow>();
            shadow.effectDistance = new Vector2(2f, -2f);
            return text;
        }

        static GameObject MakePanel(Transform parent, string name, string message, Color bg)
        {
            var panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            var img = panel.AddComponent<Image>();
            img.color = bg;
            var rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            var title = MakeText(panel.transform, "Title", message, 90, TextAnchor.MiddleCenter);
            var trt = title.rectTransform;
            trt.anchorMin = new Vector2(0.5f, 0.5f);
            trt.anchorMax = new Vector2(0.5f, 0.5f);
            trt.anchoredPosition = new Vector2(0f, 80f);
            trt.sizeDelta = new Vector2(1200f, 160f);

            var btnGo = new GameObject("RestartButton");
            btnGo.transform.SetParent(panel.transform, false);
            var btnImg = btnGo.AddComponent<Image>();
            btnImg.color = new Color(1f, 1f, 1f, 0.92f);
            var btn = btnGo.AddComponent<Button>();
            var brt = btnGo.GetComponent<RectTransform>();
            brt.anchorMin = new Vector2(0.5f, 0.5f);
            brt.anchorMax = new Vector2(0.5f, 0.5f);
            brt.anchoredPosition = new Vector2(0f, -90f);
            brt.sizeDelta = new Vector2(380f, 110f);

            var label = MakeText(btnGo.transform, "Label", "RESTART", 44, TextAnchor.MiddleCenter);
            label.color = new Color(0.12f, 0.12f, 0.12f);
            var lrt = label.rectTransform;
            lrt.anchorMin = Vector2.zero;
            lrt.anchorMax = Vector2.one;
            lrt.offsetMin = Vector2.zero;
            lrt.offsetMax = Vector2.zero;

            panel.SetActive(false);
            return panel;
        }

        static void WireRestart(GameObject panel, Component gm)
        {
            var btn = panel.GetComponentInChildren<Button>(true);
            if (btn == null) return;
            var action = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), gm, "Restart");
            UnityEventTools.AddVoidPersistentListener(btn.onClick, action);
        }

        // ------------------------------------------------------------ helpers
        static Type FindType(string fullName)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = asm.GetType(fullName);
                if (t != null) return t;
            }
            throw new Exception("[WorkshopBuilder] type not found: " + fullName);
        }

        static void SetField(Component target, string field, object value)
        {
            var f = target.GetType().GetField(field, BindingFlags.Public | BindingFlags.Instance);
            if (f == null) throw new Exception("[WorkshopBuilder] field not found: " + target.GetType().Name + "." + field);
            f.SetValue(target, value);
            EditorUtility.SetDirty(target);
        }
    }
}
