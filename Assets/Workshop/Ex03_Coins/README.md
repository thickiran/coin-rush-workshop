# Exercise 03 — Coins — Prefabs and Live Inspection

**Time:** 5 min instructor demo + 10 min hands-on
**Start scene:** `Ex03_Start.unity` — open it now. It equals the finished state of the previous exercise.
**Missed an exercise?** Nothing to restore: opening this scene catches you up completely.

## Goal
Build the game's first collectible as a prefab, place a ring of them, and learn to
interrogate the live scene with `eval` — the skill you'll later use to prove bugs.

## Instructor demo (5 min)
1. Ask the agent for a spinning coin prefab (prompt below); approve the plan.
2. When it's done, count the coins from the terminal:
   `unity command eval 'return UnityEngine.GameObject.Find("Coins").transform.childCount;'`
3. Collect one in play mode; run the count again — one fewer.

## Your turn (10 min)
1. Prompt for Claude Code:
   > In `Assets/Workshop/Ex03_Coins/`: create a `CoinPickup` MonoBehaviour in `Scripts/`
   > that spins the coin around world Y (120°/s) and, in OnTriggerEnter with the object
   > tagged "Player", logs "Coin collected!" and destroys itself. Then build a Coin
   > prefab in `Prefabs/`: root object with the CoinPickup and a trigger SphereCollider
   > (radius 0.6), child cylinder visual (scale 0.9, 0.06, 0.9, rotated 90° on X) with
   > its collider removed. Instantiate 8 coins in a circle of radius 6 at y=0.6 in the
   > open scene, under a parent named "Coins". Use the Unity CLI to do the scene work.
2. Play: walk into a coin — it should log and vanish.
3. Practice live inspection — count remaining coins from the terminal:
   ```
   unity command eval 'return UnityEngine.GameObject.Find("Coins").transform.childCount;'
   ```

## Verify
- 8 coins in the scene before play; each collected coin logs once and disappears.
- The terminal count drops as you collect.

## If you get stuck
- Coins don't trigger? The player needs its (kinematic) Rigidbody — it's already on the
  Player in this checkpoint; if the agent replaced the player, tell it to restore the Rigidbody.
- `Ex04_Start.unity` contains the finished coin ring.
