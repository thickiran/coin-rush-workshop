# Exercise 05 — Hazard and the Lose State — Iterating on Behavior

**Time:** 5 min instructor demo + 10 min hands-on
**Start scene:** `Ex05_Start.unity` — open it now. It equals the finished state of the previous exercise.
**Missed an exercise?** Nothing to restore: opening this scene catches you up completely.

## Goal
Add danger: a patrolling hazard that ends the run. The point of this exercise is
*iteration* — your first prompt won't be the final behavior, and that's normal. You
direct; the agent adjusts.

## Instructor demo (5 min)
1. First prompt (below), approve, play — hazard works but feels too slow.
2. Follow-up prompt: "make it 50% faster and patrol along Z instead of X" — watch the
   agent edit and re-verify. Iteration is cheap; asking for perfection up front is not.

## Your turn (10 min)
1. Prompt:
   > In `Assets/Workshop/Ex05_Hazard/`: create a `Hazard` MonoBehaviour in `Scripts/`
   > that ping-pongs a red cube between two points (speed ~4) and calls
   > GameManager.Lose() when its trigger touches the object tagged "Player". Add
   > Lose() to the existing GameManager: it shows a "GAME OVER" panel (with a RESTART
   > button) and freezes time, same as Win(). Build the lose panel in the scene's
   > canvas, create the hazard cube (red, trigger collider) patrolling between
   > (-6, 0.6, -3.5) and (6, 0.6, -3.5) — deliberately away from the player spawn, and wire everything with the Unity CLI.
2. Play: touch the hazard → GAME OVER → RESTART works.
3. Now iterate at least once — change its speed, path, or size with a follow-up prompt.

## Verify
- Hazard patrols; touching it shows GAME OVER; coins still win the game.
- After your follow-up prompt, the changed behavior is visible in play mode.

## If you get stuck
- Hazard doesn't trigger? Its collider must be a trigger and the player keeps its Rigidbody.
- `Ex06_Start.unity` has the finished hazard (and a surprise for the next exercise).
