using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEditor.AssetDatabase;
using static System.IO.Directory;
using static System.IO.Path;
using UnityEditor;
using System.IO;



namespace Platformer
{
    public static class Setup
    {
        [MenuItem("Tools/Platformer/Create Default Folders")]
        public static void CreateDefaultFolders()
        {
            Folders.CreateDefault("Platformer", "Animations", "Art", "Audio", "Materials", "Prefabs", "Scenes", "Scripts");
            Refresh();

        }

        static class Folders
        {
            public static void CreateDefault(string root, params string[] folders)
            {
                var fullpath = Path.Combine(Application.dataPath, root);
                foreach (var folder in folders)
                {
                    var path = Path.Combine(fullpath, folder);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                }
            }
        }
        
    }
}
