# Coin Rush — Unity CLI Workshop Lab (Track A)

A complete mobile mini-game built in nine exercises, each teaching one Unity CLI /
agent-workflow skill. Format per exercise: **5 min instructor demo, then 5–10 min
hands-on**, then a terminal-verifiable checkpoint.

## The one rule that keeps everyone in sync
Every `ExNN` folder contains a **Start scene equal to the finished state of the
previous exercise**. At the start of each exercise, *everyone* opens that exercise's
Start scene — whether you finished the last one, went further, or missed it entirely.
Nobody restores anything, ever.

## Exercises
| # | Folder | Builds | Teaches |
|---|--------|--------|---------|
| 01 | `Ex01_SceneSetup` | ground + player | `unity status`, command discovery, `eval` object creation |
| 02 | `Ex02_Movement` | movement script | agent writes code under plan mode; CLAUDE.md contract |
| 03 | `Ex03_Coins` | coin prefab + ring | prefab workflow; live inspection with `eval` |
| 04 | `Ex04_Score` | score UI + win | one prompt, coordinated multi-file change |
| 05 | `Ex05_Hazard` | hazard + lose | iterating on behavior with follow-up prompts |
| 06 | `Ex06_BugHunt` | fix BUG-201 | reproduce → prove root cause → fix → prove fix |
| 07 | `Ex07_CliCommand` | `workshop_validate` | exposing studio tools as CLI commands |
| 08 | `Ex08_Magnet` | magnet power-up | solo: from ticket to verified feature |
| 09 | `Ex09_MobileBuild` | iOS build | terminal builds; the road to CI |

`_Complete/` holds the finished game. `_Shared/` holds materials and art used by every
checkpoint — read-only for students. `_Setup/` contains `WorkshopBuilder`, which
regenerates every checkpoint scene and prefab (`Workshop > Build All Checkpoints`, or
`unity command eval 'Workshop.Setup.WorkshopBuilder.BuildAll();'`).

## Why each exercise folder has its own namespace
`Ex05_Hazard/Scripts` and `Ex06_BugHunt/Scripts` contain the *same* scripts at
different stages, compiled into separate assemblies (`Workshop.Ex05`, `Workshop.Ex06`)
via asmdefs. That's what lets ten snapshots of one game coexist in a single project.

## Instructor notes
- Timing: ~15 min per exercise + 15 min setup + 15 min break ≈ a 180-minute track.
- Ex06's bug is seeded only in `Ex06_BugHunt/Prefabs/Coin.prefab` (double collider).
- If a student's editor breaks mid-exercise: close without saving, reopen the current
  Start scene. Worst case, `WorkshopBuilder.BuildAll()` rebuilds every checkpoint.
