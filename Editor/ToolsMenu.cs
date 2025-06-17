#nullable enable
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.AssetDatabase;

namespace Vidkol {
    public static class ToolsMenu {
        private static readonly Dictionary<string, string[]> Folders = new() {
            { "__Project", new[] { "Core", "Game", "UI", "Scenes", "ScriptableObjects", "Audio", "Visuals", "DevTools", "Editor", "Localization", "Prefabs", "Themes", "Docs" } },
            { "__Project/Core", new[] { "Systems", "Utils", "BaseClasses", "Constants" } },
            { "__Project/Game", new[] { "Characters", "Managers", "Mechanics", "Data" } },
            { "__Project/UI", new[] { "Components", "Screens", "Transitions", "Bindings" } },
            { "__Project/Scenes", new[] { "Main", "Test" } },
            { "__Project/ScriptableObjects", new[] { "Settings", "Characters", "Audio", "Themes" } },
            { "__Project/Audio", new[] { "Music", "SFX", "Mixers" } },
            { "__Project/Visuals", new[] { "Sprites", "Animations", "VFX" } },
            { "__Project/DevTools", new[] { "DebugUI", "Cheats", "Logging" } },
            { "__Project/Editor", new[] { "Inspectors", "Windows", "PropertyDrawers" } },
            { "__Project/Localization", new[] { "CSV", "System" } },
            { "__Project/Prefabs", new[] { "Characters", "UI", "Environment" } },
            { "__Project/Themes", new[] { "Colors", "Fonts" } },
            { "__Project/Docs", new[] { "Readme", "Design" } }
        };
        
        private static readonly Dictionary<string, (string filePath, string fileName, string? user)> Scripts = new() {
            { "49ecb149d3bd429ea1923ae5ff5798e8", ("__Project/Core/BaseClasses", "SingletonUtilities.cs", null) },
            { "Vidkol18/UnityConsoleUI", ("__Project/DevTools", "Console", null) },
        };

        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders() {
            Vidkol.Folders.CreateDirectories(Folders);
            Refresh();
        }

        [MenuItem("Tools/Setup/Load New Manifest")]
        public static async void LoadNewManifest() {
            try {
                // Don't think this is necessary, but keeping it for reference
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
                await Packages.DownloadScriptsAsync(Scripts);
            } catch (Exception e) {
                Debug.LogError($"Failed to generate default scripts: {e.Message}");
            }
        }
    }
}