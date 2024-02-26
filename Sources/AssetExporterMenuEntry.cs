using System.IO;

using UnityEngine;
using UnityEditor;

namespace Cgw.Editor
{
    public class AssetExporterMenuEntry : EditorWindow
    {
        const string EP_LastFolder = "EP_LastFolder";

        [MenuItem("Assets/Cgw - Export asset", true, 0)]
        public static bool ExportMenuValidation()
        {
            var selectedObject = Selection.activeGameObject;
            return selectedObject != null;
        }

        [MenuItem("Assets/Cgw - Export asset", false, 0)]
        public static void ExportMenuEntry()
        {
            Export();
        }

        public static void Export()
        {
            var selectedObject = Selection.activeGameObject;
            if (selectedObject != null)
            {
                var lastFolder = EditorPrefs.GetString(EP_LastFolder, "");
                string path = EditorUtility.OpenFolderPanel("Export assets", lastFolder, "");
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }
                if (!Directory.Exists(path))
                {
                    EditorUtility.DisplayDialog("Asset exporter", "Export directory not found", "ok");
                    return;
                }
                EditorPrefs.SetString(EP_LastFolder, path);
                AssetExporter.Export(selectedObject, path);
            }
        }
    }
}