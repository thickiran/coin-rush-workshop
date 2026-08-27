# Exercise 01 — Scene Setup with the Unity CLI

**Time:** 5 min instructor demo + 10 min hands-on
**Start scene:** `Ex01_Start.unity` — open it now. It equals the finished state of the previous exercise.
**Missed an exercise?** Nothing to restore: opening this scene catches you up completely.

## Goal
Meet the Unity CLI: connect to the running editor, discover its commands, and build
the first pieces of the game — a ground and a player — without touching the mouse.

## Instructor demo (5 min)
1. `unity status` — confirm the editor is connected (state: `ready`).
2. `unity command` — list every command this editor exposes.
3. `unity command eval 'new UnityEngine.GameObject("Hello");'` — create an object live.
4. Delete it again with eval, and save the scene with the editor's save command.

## Your turn (10 min)
Run each command in your terminal and watch the editor react:

1. Confirm the connection:
   ```
   unity status
   ```
2. Create the ground:
   ```
   unity command eval 'var g = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube); g.name = "Ground"; g.transform.position = new UnityEngine.Vector3(0, -0.25f, 0); g.transform.localScale = new UnityEngine.Vector3(20, 0.5f, 20);'
   ```
3. Create the player:
   ```
   unity command eval 'var p = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Capsule); p.name = "Player"; p.tag = "Player"; p.transform.position = new UnityEngine.Vector3(0, 1, 0); var rb = p.AddComponent<UnityEngine.Rigidbody>(); rb.isKinematic = true; rb.useGravity = false;'
   ```
4. Ask the editor what you built:
   ```
   unity command eval 'UnityEngine.GameObject.Find("Player") != null'
   ```

## Verify
- `unity command eval 'UnityEngine.GameObject.Find("Ground").transform.localScale'` prints `(20.00, 0.50, 20.00)`.
- The Hierarchy shows `Ground` and `Player`.

Your scene will look plainer than the next checkpoint (no walls, no materials) — that is
expected. Checkpoints always contain the fully polished state.

## If you get stuck
- `unity status` not `ready`? The editor may still be compiling — wait for the spinner, run it again.
- `eval` errors usually mean a typo in the C# — copy the command exactly, on one line.
