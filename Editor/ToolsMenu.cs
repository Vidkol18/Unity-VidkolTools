using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.AssetDatabase;

namespace Vidkol {
    public static class ToolsMenu {

        private static readonly Dictionary<string, string[]> _folders = new() {
            { "__Project", new[] { "Animations", "Editor", "Fonts", "Materials", "Models", "Settings", "Shaders", "Textures" } },
            { "__Project/Audio", new[] { "Music", "SFX" } },
            { "__Project/Prefabs", new[] { "Items", "NPCs", "Objects", "UI", "World", "Player" } },
            { "__Project/Data", new[] { "Scriptables" } },
            { "__Project/Scenes", new[] { "Levels" } },
            { "__Project/Scripts", new[] { "Managers", "Networking", "NPC", "Player", "UI", "Utils", "World" } }
        };

        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders() {
            Folders.CreateDirectories(_folders);
            Refresh();
        }

        [MenuItem("Tools/Setup/Load New Manifest")]
        public static async void LoadNewManifest() {
            try {
                await Packages.ReplacePackagesFromGist("99692c9c984ceb2a670f80a2aafd544b");
            } catch (Exception e) {
                Debug.LogError($"Failed to load new manifest: {e.Message}");
            }
        }

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

        [MenuItem("Tools/Setup/Generate Default Scripts")]
        public static async void GenerateDefaultScripts() {
            try {
                const string scriptsPath = "__Project/Scripts";
                await Packages.CreateScriptFromGist("49ecb149d3bd429ea1923ae5ff5798e8", scriptsPath + "/Utils", "SingletonUtilities");
            } catch (Exception e) {
                Debug.LogError($"Failed to generate default scripts: {e.Message}");
            }
        }
    }
}
