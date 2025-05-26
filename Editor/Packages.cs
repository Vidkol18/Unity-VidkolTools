using System;
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
        }

        public static void InstallUnityPackage(string packageName) {
            UnityEditor.PackageManager.Client.Add($"com.unity.{packageName}");
        }
    }
}
