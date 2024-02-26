using System.IO;

using UnityEngine;
using UnityEditor;

namespace Cgw.Editor
{
    public class AssetExporter
    {
        private static string TempPath => Path.GetFullPath("TempBundleExport");

        private static void CreateTempDirectory()
        {
            if (Directory.Exists(TempPath))
            {
                return;
            }
            Directory.CreateDirectory(TempPath);
        }

        private static void DeleteTempDirectory()
        {
            if (!Directory.Exists(TempPath))
            {
                return;
            }
            Directory.Delete(TempPath, true);
        }

        public static void Export(GameObject p_prefab, string p_outputPath)
        {
            if (p_outputPath == null)
            {
                return;
            }
            var buildOptions =
                BuildAssetBundleOptions.ForceRebuildAssetBundle |
                BuildAssetBundleOptions.UncompressedAssetBundle |
                BuildAssetBundleOptions.AssetBundleStripUnityVersion;

            var name = p_prefab.name;
            var path = AssetDatabase.GetAssetPath(p_prefab);

            AssetBundleBuild build = new AssetBundleBuild()
            {
                assetBundleName = $"{name.ToLower()}.asset",
                assetNames = new string[] { path }
            };

            CreateTempDirectory();

            BuildPipeline.BuildAssetBundles(
                TempPath, 
                new AssetBundleBuild[] { build }, 
                buildOptions, 
                BuildTarget.StandaloneWindows64
            );

            var tempFilePath = Path.Combine(TempPath, build.assetBundleName);
            var outputFilePath = Path.Combine(p_outputPath, build.assetBundleName);

            if (!File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            File.Move(tempFilePath, outputFilePath);

            DeleteTempDirectory();

            File.WriteAllText(Path.Combine(p_outputPath, $"{name.ToLower()}.yaml"), $"asset_name: {name}");
        }
    }
}
