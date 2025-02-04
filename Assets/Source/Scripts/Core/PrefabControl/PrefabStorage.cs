using UnityEngine;

namespace TanksArmageddon.Core.PrefabControl
{
    public static class PrefabStorage
    {
        public static bool IsPrefabsLoaded { get; private set; } = false;

        public static void LoadPrefabs()
        {
            if (IsPrefabsLoaded)
            {
                Debug.LogError("Prefabs are already loaded");
                return;
            }

            PrefabInfoStorage.LoadPrefabs();
            IsPrefabsLoaded = true;
        }

        public static GameObject Get(PrefabName prefabName) => PrefabInfoStorage.GetPrefab(prefabName);

        public static T Get<T>(PrefabName prefabName) => Get(prefabName).GetComponent<T>();
    }
}
