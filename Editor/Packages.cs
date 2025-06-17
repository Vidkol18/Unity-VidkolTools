using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
                    string url = kvp.Key;

                    // Check if this is a Git repository URL or user/repo format
                    if (IsGitRepoUrl(url)) {
                        // The fileName is used as the subfolder name for git repos
                        string targetPath = Path.Combine(filePath, fileName);
                        await DownloadGitRepoAsync(url, targetPath);
                        Debug.Log($"Git repository downloaded to {targetPath}");
                    } else {
                        // Handle regular Gist URLs
                        url = url.StartsWith("http") ? url : GetGistUrl(url, resolvedUser);
                        var contents = await GetContents(url);
                        CreateScripFileFromContents(contents, filePath, fileName);
                    }
                } catch (Exception ex) {
                    Debug.LogError($"Failed to download script from {kvp.Key}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Determines if a URL is a Git repository URL or user/repo format
        /// </summary>
        private static bool IsGitRepoUrl(string url) {
            return url.Contains("github.com/") || 
                   (url.Contains("/") && !url.Contains("gist.github.com")) || 
                   url.EndsWith(".git");
        }

        public static async Task CreateScriptFromGist(string gistId, string filePath, string fileName, string user = "Vidkol18") {
            var scriptUrl = GetGistUrl(gistId, user);
            var contents = await GetContents(scriptUrl);
            CreateScripFileFromContents(contents, filePath, fileName);
        }

        /// <summary>
        /// Downloads a Git repository as a ZIP file and extracts it to the specified path
        /// </summary>
        /// <param name="repoUrl">Repository URL or format "username/repo"</param>
        /// <param name="targetPath">Path relative to Assets folder where to extract the repository</param>
        /// <param name="branch">Branch to download, defaults to main</param>
        /// <returns>Task representing the async operation</returns>
        public static async Task DownloadGitRepoAsync(string repoUrl, string targetPath, string branch = "main") {
            try {
                // Parse the repo URL to get owner and repo name
                string owner, repo;
                if (repoUrl.Contains("github.com")) {
                    var parts = repoUrl.TrimEnd('/').Split('/');
                    owner = parts[^2];
                    repo = parts[^1].Replace(".git", "");
                } else if (repoUrl.Contains("/")) {
                    var parts = repoUrl.Split('/');
                    owner = parts[0];
                    repo = parts[1];
                } else {
                    throw new ArgumentException("Invalid repository format. Use 'username/repo' or a GitHub URL");
                }

                // GitHub API URL to download repository as ZIP
                string zipUrl = $"https://api.github.com/repos/{owner}/{repo}/zipball/{branch}";
                Debug.Log($"Downloading repository: {owner}/{repo} (branch: {branch})");

                // Download the ZIP file
                using var client = new HttpClient();
                // Add a user agent to avoid GitHub API limitations
                client.DefaultRequestHeaders.Add("User-Agent", "Unity-VidkolTools");
                
                byte[] zipData = await client.GetByteArrayAsync(zipUrl);
                string tempZipPath = Path.Combine(Path.GetTempPath(), $"{repo}-{Guid.NewGuid()}.zip");
                File.WriteAllBytes(tempZipPath, zipData);
                
                // Create the target directory
                string fullTargetPath = Path.Combine(Application.dataPath, targetPath);
                Directory.CreateDirectory(fullTargetPath);

                // Extract the ZIP file
                using (ZipArchive archive = ZipFile.OpenRead(tempZipPath)) {
                    // Get the root folder name (usually owner-repo-commitish)
                    string rootFolder = archive.Entries[0].FullName;
                    
                    foreach (ZipArchiveEntry entry in archive.Entries) {
                        // Skip directories and the root directory
                        if (entry.FullName.Equals(rootFolder) || entry.FullName.EndsWith("/")) {
                            continue;
                        }
                        
                        // Get the relative path by removing the root folder
                        string relativePath = entry.FullName.Substring(rootFolder.Length);
                        string destinationPath = Path.Combine(fullTargetPath, relativePath);
                        
                        // Ensure directory exists
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath) ?? string.Empty);
                        
                        // Extract file
                        entry.ExtractToFile(destinationPath, true);
                    }
                }
                
                // Clean up
                File.Delete(tempZipPath);
                Debug.Log($"Repository extracted to {fullTargetPath}");
            } catch (Exception ex) {
                Debug.LogError($"Failed to download repository: {ex.Message}");
                throw;
            }
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