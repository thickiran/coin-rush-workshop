using System.Text;
using Unity.Pipeline.Commands;
using UnityEngine;

namespace Workshop.Complete.EditorTools
{
    /// <summary>
    /// Reference implementation for Exercise 07: a level validator exposed to the
    /// Unity CLI. Registered as "workshop_validate_reference" so it never collides
    /// with the "workshop_validate" command students create during the exercise.
    /// Run with:  unity command workshop_validate_reference
    /// </summary>
    public static class LevelValidator
    {
        const float Bounds = 9f;

        [CliCommand("workshop_validate_reference",
            "Validate the open Coin Rush scene (reference implementation for Exercise 07)",
            Tags = new[] { "workshop" })]
        public static string Validate()
        {
            var sb = new StringBuilder();
            int failures = 0;

            // 1. Exactly one player, with a PlayerController, inside the arena.
            var players = GameObject.FindGameObjectsWithTag("Player");
            Check(sb, ref failures, players.Length == 1,
                "exactly one object tagged 'Player' (found " + players.Length + ")");
            if (players.Length == 1)
            {
                var p = players[0];
                Check(sb, ref failures, p.GetComponent<Workshop.Complete.PlayerController>() != null,
                    "Player has a PlayerController");
                Check(sb, ref failures, InBounds(p.transform.position),
                    "Player inside the arena (|x|,|z| <= " + Bounds + ")");
            }

            // 2. Coins: at least one, all in bounds, each with exactly ONE collider.
            var coins = Object.FindObjectsByType<Workshop.Complete.CoinPickup>(FindObjectsSortMode.None);
            Check(sb, ref failures, coins.Length >= 1, "at least one coin (found " + coins.Length + ")");
            foreach (var coin in coins)
            {
                if (!InBounds(coin.transform.position))
                    Check(sb, ref failures, false, "coin '" + coin.name + "' inside the arena");
                int colliderCount = coin.GetComponents<Collider>().Length;
                if (colliderCount != 1)
                    Check(sb, ref failures, false,
                        "coin '" + coin.name + "' has exactly one collider (found " + colliderCount + ")");
            }

            // 3. Managers present with references assigned.
            var score = Object.FindFirstObjectByType<Workshop.Complete.ScoreManager>();
            Check(sb, ref failures, score != null, "ScoreManager present");
            if (score != null)
                Check(sb, ref failures, score.scoreText != null, "ScoreManager.scoreText assigned");

            var gm = Object.FindFirstObjectByType<Workshop.Complete.GameManager>();
            Check(sb, ref failures, gm != null, "GameManager present");
            if (gm != null)
            {
                Check(sb, ref failures, gm.winPanel != null, "GameManager.winPanel assigned");
                Check(sb, ref failures, gm.losePanel != null, "GameManager.losePanel assigned");
            }

            string verdict = failures == 0
                ? "PASS — all checks green"
                : "FAIL — " + failures + " check(s) failed";
            return verdict + "\n" + sb;
        }

        static bool InBounds(Vector3 pos) =>
            Mathf.Abs(pos.x) <= Bounds && Mathf.Abs(pos.z) <= Bounds;

        static void Check(StringBuilder sb, ref int failures, bool ok, string what)
        {
            sb.AppendLine((ok ? "  [ok]   " : "  [FAIL] ") + what);
            if (!ok) failures++;
        }
    }
}
