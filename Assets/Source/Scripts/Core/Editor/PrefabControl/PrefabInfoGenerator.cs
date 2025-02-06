using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace TanksArmageddon.Core.PrefabControl.Editor
{
    public class PrefabInfoGenerator// : IPreprocessBuildWithReport
    {
        private const string PathToPrefabsFolder = "Assets/Resources/Prefabs";
        private const string PrefabsFindFilter = "t:Prefab";

        private const string ExtraPartOfPath = "Assets/Resources/";
        private const string PrefabExtension = ".prefab";

        //public int callbackOrder => 0;

        //public void OnPreprocessBuild(BuildReport report) => UpdatePrefabsInfo();

        [MenuItem("Tools/Update Prefabs Info")]
        private static void UpdatePrefabsInfo()
        {
            List<GameObject> prefabs = new();
            List<string> pathsToPrefabs = new();
            Dictionary<int, string> enumNames = new();

            string[] prefabGuids = AssetDatabase.FindAssets(PrefabsFindFilter, new[] { PathToPrefabsFolder });

            foreach (string guid in prefabGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                prefabs.Add(prefab);
                pathsToPrefabs.Add(CutOffExcessPartOfPath(path));
            }

            for (int i = 0; i < prefabs.Count; i++)
            {
                enumNames.Add(i, prefabs[i].name);
            }

            PrefabNameGenerator.Generate(enumNames);
            PrefabInfoStorageGenerator.Generate(enumNames, pathsToPrefabs);

            AssetDatabase.Refresh();
        }

        private static string CutOffExcessPartOfPath(string path)
        {
            return path
                .Replace(ExtraPartOfPath, string.Empty)
                .Replace(PrefabExtension, string.Empty);
        }
    }
}
