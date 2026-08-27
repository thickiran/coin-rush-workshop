using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Workshop.Complete.EditorTools
{
    /// <summary>
    /// Reference implementation for Exercise 09: a scripted player build.
    /// In class, prefer the live editor's async build command:
    ///   unity command build --target iOS --outputPath Builds/iOS --scenes Assets/Workshop/_Complete/CoinRush.unity
    ///   unity command build_status
    /// This method is the CI variant, invoked on a CLOSED project:
    ///   unity build . --target iOS --execute-method Workshop.Complete.EditorTools.BuildScript.BuildIOS
    /// </summary>
    public static class BuildScript
    {
        public static void BuildIOS()
        {
            var options = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/Workshop/_Complete/CoinRush.unity" },
                locationPathName = "Builds/iOS",
                target = BuildTarget.iOS,
                options = BuildOptions.None,
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary s = report.summary;
            Debug.Log("[BuildScript] result=" + s.result
                + " sizeBytes=" + s.totalSize
                + " duration=" + s.totalTime
                + " errors=" + s.totalErrors
                + " output=" + s.outputPath);
        }
    }
}
