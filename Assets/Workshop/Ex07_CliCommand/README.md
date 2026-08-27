# Exercise 07 — Your Own CLI Command — a Level Validator

**Time:** 5 min instructor demo + 10 min hands-on
**Start scene:** `Ex07_Start.unity` — open it now. It equals the finished state of the previous exercise.
**Missed an exercise?** Nothing to restore: opening this scene catches you up completely.

## Goal
Turn studio knowledge into a tool: a level validator the CLI (and therefore any agent,
and CI) can call. This is how Panteon's real editor tooling gets exposed to agents.

## Instructor demo (5 min)
1. Show `unity command` — today's list. We're about to add to it.
2. Ask the agent for the validator (prompt below); approve; run:
   ```
   unity command workshop_validate
   ```
3. Break the level on purpose (delete the Managers object), run it again — it fails
   loudly. Restore with undo.

## Your turn (10 min)
1. Prompt:
   > Create a `LevelValidator` editor class in `Assets/Workshop/Ex07_CliCommand/Editor/`
   > and register a CLI command named `workshop_validate` using the com.unity.pipeline
   > package's command attribute (look at how the package declares its own commands to
   > get the exact attribute and signature). The command checks the open scene and
   > reports: (1) exactly one object tagged "Player" with a PlayerController,
   > (2) at least one coin, all inside |x|,|z| <= 9, (3) a ScoreManager and GameManager
   > present with their references assigned, (4) every coin has exactly ONE collider.
   > Return a readable report; make it fail clearly when a check fails.
2. Run `unity command workshop_validate` — all checks pass.
3. Sabotage: move the Player to x = 50 with eval, validate (fails), move it back, validate.

## Verify
- `unity command` lists `workshop_validate`.
- The validator passes on the intact scene and fails with a clear message when sabotaged.

## If you get stuck
- If the attribute name is unclear, the agent can read the pipeline package source in
  `Library/PackageCache/com.unity.pipeline*/` — reading the package is the exercise.
- `_Complete/Editor/LevelValidator.cs` is the reference implementation (registered as
  `workshop_validate_reference` so it won't collide with yours).
