using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace Vidkol {
    public static class Folders {
        public static void CreateDirectories(string root, params string[] directories) {
            var fullPath = Path.Combine(Application.dataPath, root);
            foreach (var dir in directories) {
                var path = Path.Combine(fullPath, dir);
                Directory.CreateDirectory(path);
            }
        }

        public static void CreateDirectories(Dictionary<string, string[]> directories) {
            foreach (var kvp in directories) {
                CreateDirectories(kvp.Key, kvp.Value);
            }
        }
    }
}
