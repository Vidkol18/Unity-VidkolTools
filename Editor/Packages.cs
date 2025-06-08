using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Vidkol {
    public static class Packages {
        public static async Task ReplacePackagesFromGist(string gistId, string user = "Vidkol18") {
            var manifestUrl = GetGistUrl(gistId, user);
            var contents = await GetContents(manifestUrl);
            ReplacePackageFile(contents);
        }

        public static async Task DownloadScriptsAsync(Dictionary<string, (string filePath, string fileName, string? user)> scripts, string defaultUser = "Vidkol18") {
            foreach (var kvp in scripts) {
                try {
                    var (filePath, fileName, user) = kvp.Value;
                    string resolvedUser = string.IsNullOrEmpty(user) ? defaultUser : user;
                    string url = kvp.Key.StartsWith("http") ? kvp.Key : GetGistUrl(kvp.Key, resolvedUser);
                    var contents = await GetContents(url);
                    CreateScripFileFromContents(contents, filePath, fileName);
                } catch (Exception ex) {
                    Debug.LogError($"Failed to download script from {kvp.Key}: {ex.Message}");
                }
            }
        }

        public static async Task CreateScriptFromGist(string gistId, string filePath, string fileName, string user = "Vidkol18") {
            var scriptUrl = GetGistUrl(gistId, user);
            var contents = await GetContents(scriptUrl);
            CreateScripFileFromContents(contents, filePath, fileName);
        }

        private static string GetGistUrl(string gistId, string user) =>
            $"https://gist.githubusercontent.com/{user}/{gistId}/raw";

        private static async Task<string> GetContents(string url) {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsStringAsync();
            }
            throw new Exception($"Failed to fetch content from {url}");
        }

        private static void ReplacePackageFile(string contents) {
            var existingPath = Path.Combine(Application.dataPath, "Packages", "manifest.json");
            if (File.Exists(existingPath)) {
                File.WriteAllText(existingPath, contents);
                Debug.Log("Package manifest replaced successfully.");
            } else {
                Debug.LogError($"Manifest file not found at {existingPath}");
            }
        }

        private static void CreateScripFileFromContents(string contents, string filePath, string fileName) {
            var fullPath = Path.Combine(Application.dataPath, filePath, fileName + ".cs");
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);
            File.WriteAllText(fullPath, contents);
            Debug.Log($"File created at {fullPath}");
        }

        public static void InstallUnityPackage(string packageName) {
            UnityEditor.PackageManager.Client.Add($"com.unity.{packageName}");
        }
    }
}