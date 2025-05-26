using System.IO;
using UnityEngine;

namespace Vidkol {
    public static class Folders {
        public static void CreateDirectories(string root, params string[] directories) {
            var fullPath = Path.Combine(Application.dataPath, root);
            foreach (var dir in directories) {
                var path = Path.Combine(fullPath, dir);
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
            }
        }

        public static void CreateDirectories(Directory<string, string[]> directories) {
            foreach (var kvp in directories) {
                CreateDirectories(kvp.Key, kvp.Value);
            }
        }
    }
}
