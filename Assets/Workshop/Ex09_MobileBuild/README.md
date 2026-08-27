# Exercise 09 — Mobile Build from the Terminal

**Time:** 5 min instructor demo + 10 min hands-on
**Start scene:** `Ex09_Start.unity` — open it now. It equals the finished state of the previous exercise.
**Missed an exercise?** Nothing to restore: opening this scene catches you up completely.

## Goal
Ship it: a build kicked off entirely from the terminal — the same command CI runs at
night with a service account.

## Instructor demo (5 min)
1. The running editor can build asynchronously — no second Unity instance needed:
   ```
   unity command build --target iOS --outputPath Builds/iOS-demo --scenes Assets/Workshop/_Complete/CoinRush.unity --confirm true
   unity command build_status
   ```
2. Poll `build_status` until `completed`, then walk through the BuildReport it returns.
3. The CI story: the same thing on a closed project with a service account —
   `unity build . --target iOS --execute-method Workshop.Complete.EditorTools.BuildScript.BuildIOS`

## Your turn (10 min)
1. Prompt:
   > Create a `BuildScript` editor class in `Assets/Workshop/Ex09_MobileBuild/Editor/`
   > (with an Editor asmdef) containing a static `BuildIOS()` that builds ONLY the
   > scene `Assets/Workshop/Ex09_MobileBuild/Ex09_Start.unity` to `Builds/iOS-Ex09`
   > with BuildPipeline.BuildPlayer, logging the BuildReport summary (result, size,
   > duration). Then, instead of running that method, start the same build on the LIVE
   > editor with the pipeline `build` command and give me the exact command plus the
   > `build_status` polling command.
2. Run the build command and poll `build_status` while it works. First iOS export
   takes a few minutes — read the returned report when it lands.
3. If a signing/Xcode step fails at the very end, that's fine for today — the exercise
   is the pipeline, not the provisioning profile.

## Verify
- The build command exits 0 (or fails only at Xcode signing) and `Builds/iOS-Ex09/` exists.
- You can say, in one sentence, what CI would need beyond this command. (Answer:
  service-account auth + a license seat.)

## If you get stuck
- `_Complete/Editor/BuildScript.cs` is the reference implementation.
