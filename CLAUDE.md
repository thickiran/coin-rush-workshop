# Coin Rush Workshop — Agent Contract

This is a training project for the Panteon Games Unity CLI workshop. Multiple exercise
snapshots of the same game coexist under `Assets/Workshop/`. These rules keep them intact.

## Hard rules
- **Work ONLY inside the single exercise folder you were asked to work in**
  (e.g. `Assets/Workshop/Ex04_Score/`). Never modify other `ExNN` folders, `_Shared/`,
  `_Setup/`, or `_Complete/` unless explicitly asked.
- **Never edit `ProjectSettings/` or `Packages/`** unless explicitly asked.
- **Never delete or rename a Start scene** (`ExNN_Start.unity`, `CoinRush.unity`).
- New scripts go in the exercise's `Scripts/` (or `Editor/` for editor code). If the
  folder has an `.asmdef`, match its namespace (e.g. `Workshop.Ex04`); if it has none,
  use no namespace.

## How to work in this project
- A live editor is connected. Use the Unity CLI for all scene and asset work:
  `unity status`, `unity command`, and `unity command eval '<C#>'`. Do not hand-edit
  `.unity` or `.prefab` YAML while the editor is reachable.
- Prefer plan mode for multi-file work; state which files you will create or change.
- Verify every change: compile cleanly (check the editor console via CLI), then prove
  behavior with `eval` or a play-mode test, and report the evidence.
- When fixing a bug, prove the root cause with `eval` **before** changing code, and
  prove the fix the same way after.

## Project facts
- Unity 6000.4.7f1, URP, legacy Input active alongside the Input System (scripts use
  `UnityEngine.Input`).
- Player is tagged `Player` and carries a kinematic Rigidbody; pickups use trigger
  colliders (`OnTriggerEnter`).
- UI is UGUI with the built-in `LegacyRuntime.ttf` font.
- Arena is 20×20 centered at origin; gameplay positions stay within |x|,|z| ≤ 9.
