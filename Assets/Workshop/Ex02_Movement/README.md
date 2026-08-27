# Exercise 02 — Player Movement, Written by the Agent

**Time:** 5 min instructor demo + 10 min hands-on
**Start scene:** `Ex02_Start.unity` — open it now. It equals the finished state of the previous exercise.
**Missed an exercise?** Nothing to restore: opening this scene catches you up completely.

## Goal
Have Claude Code write your first gameplay script — under your control. You review the
plan before any file is touched, and you verify the result in the editor.

## Instructor demo (5 min)
1. Open `CLAUDE.md` at the project root — this is the contract every agent session reads.
2. Start Claude Code in plan mode and paste the task prompt (below).
3. Review the plan: which files will it create? Is it staying inside `Ex02_Movement/`?
4. Approve; watch it create the script and attach it via the CLI; test in play mode.

## Your turn (10 min)
1. In the project folder, start Claude Code and paste:
   > Create a `PlayerController` MonoBehaviour in `Assets/Workshop/Ex02_Movement/Scripts/`.
   > WASD/arrow keys move the player on the XZ plane (speed 6); holding the left mouse
   > button (or a touch) moves the player toward the point under the cursor, raycast
   > against the ground plane y=0. Clamp position to x and z in [-9, 9]. Rotate the player
   > to face its movement direction. Then attach it to the "Player" object in the open
   > scene using the Unity CLI, and tell me how you verified it compiles.
2. **Read the plan before approving.** If it plans to edit anything outside
   `Ex02_Movement/`, say no and tell it to stay in the exercise folder.
3. After it finishes, press Play in the editor (or `unity command editor_play`) and move
   with WASD and click-to-move.

## Verify
- `unity command eval 'UnityEngine.GameObject.Find("Player").GetComponent("PlayerController") != null'` → `True`
- In play mode the capsule moves and turns toward where you click.

## If you get stuck
- Script compiles but player doesn't move? Check the Console for exceptions, then ask
  the agent: "the player does not move — investigate and fix."
- Completely stuck? Move on. `Ex03_Start.unity` contains working movement.
