using UnityEditor;
using static UnityEditor.AssetDatabase;

namespace Vidkol {
    public static class ToolsMenu {
        private static string[] _mainDir = {
            "Data", "Materials", "Models", "Prefabs", "Scenes", "Scripts", "Settings", "Shaders", "Textures",
        }; 
        

        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders() {
            Folders.CreateDirectories("_Project", _mainDir);
            Refresh();
        }

        [MenuItem("Tools/Setup/Load New Manifest")]
        public static async void LoadNewManifest() => await Packages.ReplacePackagesFromGist("99692c9c984ceb2a670f80a2aafd544b");

        // Install Unity Packages
        [MenuItem("Tools/Setup/Packages/ProBuilder")]
        public static void InstallProBuilder() {
            Packages.InstallUnityPackage("probuilder");
        }

        [MenuItem("Tools/Setup/Packages/ProGrids")]
        public static void InstallProGrid() {
            Packages.InstallUnityPackage("progrids");
        }

        [MenuItem("Tools/Setup/Packages/PostProcessing")]
        public static void InstallPostProcessing() {
            Packages.InstallUnityPackage("postprocessing");
        }

        [MenuItem("Tools/Setup/Packages/Cinemachine")]
        public static void InstallCinemachine() {
            Packages.InstallUnityPackage("cinemachine");
        }
    }
}
