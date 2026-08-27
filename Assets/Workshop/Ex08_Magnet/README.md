# Exercise 08 — Coin Magnet — You Direct, Solo

**Time:** 5 min instructor demo + 10 min hands-on
**Start scene:** `Ex08_Start.unity` — open it now. It equals the finished state of the previous exercise.
**Missed an exercise?** Nothing to restore: opening this scene catches you up completely.

## Goal
No prompt template this time. You get acceptance criteria — like a real ticket — and
you direct the agent from plan to proof on your own. Everything you practiced: plan
mode, folder discipline, iteration, eval verification.

## The ticket
> **FEAT-77 — Magnet power-up**
> A visible pickup in the arena. When the player collects it, nearby coins are pulled
> toward the player for 5 seconds (radius ~4, pull speed ~10). One use, then it's gone.
> Must not break: win at exactly 8/8, hazard lose, restart.

## Instructor demo (5 min)
Instructor sketches how they'd brief the agent from a bare ticket — what belongs in the
prompt (files, folder, acceptance criteria, verification demand) — then stops. The rest
is yours.

## Your turn (10 min)
1. Write your own prompt. Include: target folder `Assets/Workshop/Ex08_Magnet/`, the
   behavior above, and a demand that the agent verify via play mode and eval.
2. Review the plan hard: two scripts? one? scene wiring? Push back where it's vague.
3. Play it: grab the magnet, watch coins fly to you, still win at 8/8.
4. Run your validator from Exercise 07: `unity command workshop_validate` — still green.

## Verify
- Magnet pickup visible; collecting it pulls nearby coins for ~5 seconds.
- Full run still ends 8/8; hazard and restart still work; validator passes.

## If you get stuck
- Coins pulled but not collected? They must reach the player's trigger — pull them to
  the player's position, not just near it.
- `Ex09_Start.unity` contains a finished magnet.
