# Exercise 04 — Score UI and the Win State — Multi-file Editing

**Time:** 5 min instructor demo + 10 min hands-on
**Start scene:** `Ex04_Start.unity` — open it now. It equals the finished state of the previous exercise.
**Missed an exercise?** Nothing to restore: opening this scene catches you up completely.

## Goal
A real feature now: score display and a win screen. This needs coordinated changes in
multiple files — exactly the kind of work you delegate to the agent as one task, not
file by file.

## Instructor demo (5 min)
1. Show the task prompt (below) — one prompt, four coordinated pieces.
2. Approve the plan only after checking it touches only `Ex04_Score/`.
3. Play to the win screen; then prove the state from the terminal:
   `unity command eval 'return UnityEngine.Time.timeScale;'` → `0` after winning.

## Your turn (10 min)
1. Prompt:
   > In `Assets/Workshop/Ex04_Score/`: (1) Create `ScoreManager` in `Scripts/` — counts
   > total coins at Start, exposes AddCoins(int), updates a UI Text "Coins: X / Y".
   > (2) Create `GameManager` in `Scripts/` — Win() shows a win panel and sets
   > Time.timeScale to 0; Restart() resets timeScale and reloads the scene.
   > (3) Modify the existing `CoinPickup` to call ScoreManager.AddCoins instead of
   > logging. (4) In the open scene build a Screen Space Overlay canvas with a score
   > label top-left and a hidden full-screen win panel with a RESTART button wired to
   > GameManager.Restart, and a "Managers" object holding both managers, references
   > assigned. Use the Unity CLI for all scene work; verify by entering play mode.
2. Play: collect all 8 coins → "YOU WIN!" appears, game freezes, RESTART works.

## Verify
- Label reads `Coins: 0 / 8` on play start and counts up.
- `unity command eval 'return UnityEngine.Time.timeScale;'` returns 0 on the win screen.

## If you get stuck
- Score stuck at 0? Ask the agent to check that CoinPickup actually calls ScoreManager.
- `Ex05_Start.unity` has the finished score + win flow.
