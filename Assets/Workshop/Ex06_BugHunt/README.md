# Exercise 06 — Bug Hunt — Prove It, Fix It, Prove the Fix

**Time:** 5 min instructor demo + 10 min hands-on
**Start scene:** `Ex06_Start.unity` — open it now. It equals the finished state of the previous exercise.
**Missed an exercise?** Nothing to restore: opening this scene catches you up completely.

## Goal
This checkpoint ships a real bug, reported by QA. Run the full professional loop you
saw in the morning demo: reproduce → prove the root cause with `eval` → fix → verify.
The rule: **no fix until the root cause is proven.**

## The bug report
> **BUG-201** · Reported on device, intermittent
> "Sometimes a coin gives 2 points instead of 1. The counter can show 9/8 or 10/8
> after collecting everything. Happens on some coins, not others… we think. No
> console errors."

## Instructor demo (5 min)
1. Reproduce: play, collect coins, watch the counter jump by 2.
2. The wrong move: "just make the score clamp at 8" — a patch that hides the defect.
3. The right move: hand the report to the agent in plan mode and *require proof*.

## Your turn (10 min)
1. Prompt:
   > Read this bug report for the scene Ex06_Start: [paste BUG-201 above]. Investigate
   > the root cause. Before changing anything, prove the cause using the Unity CLI's
   > eval command and show me the evidence. Then propose the minimal fix, apply it,
   > and prove the fix the same way.
2. The agent should discover the coin prefab carries **two identical trigger
   colliders** — so OnTriggerEnter fires twice. Evidence looks like:
   ```
   unity command eval 'UnityEngine.GameObject.Find("Coins").transform.GetChild(0).GetComponents<UnityEngine.Collider>().Length'
   ```
   → `2` (should be 1).
3. Approve the minimal fix (remove the duplicate collider on the prefab), then play a
   full run: final score must read exactly 8 / 8.

## Verify
- Evidence shown *before* the fix (collider count 2) and *after* (count 1).
- A full playthrough ends at 8 / 8 — never 9 or 10.

## If you get stuck
- If the agent patches the symptom (clamping, cooldowns, "collected" flags), reject the
  plan: "find the root cause first, prove it with eval."
- `Ex07_Start.unity` contains the properly fixed game.
