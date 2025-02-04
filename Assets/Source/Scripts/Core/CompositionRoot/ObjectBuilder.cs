using System;
using TanksArmageddon.Core.PrefabControl;
using UnityEngine;

namespace TanksArmageddon.Core.CompositionRoot
{
    public static class ObjectBuilder
    {
        public static T CreateNew<T>(T prefab, Func<T> creator, Action<T> initializer, bool isActivateObject = true)
            where T : MonoScript
        {
            T instance = CreateNew(prefab, creator);

            initializer.Invoke(instance);
            instance.gameObject.SetActive(isActivateObject);

            return instance;
        }

        public static T CreateNew<T>(T prefab, Func<T> creator) 
            where T : MonoScript
        {
            prefab.gameObject.SetActive(false);
            T instance = creator.Invoke();
            prefab.gameObject.SetActive(true);

            return instance;
        }

        public static T CreateNew<T>(PrefabName prefabName, Action<T> initializer, bool isActivateObject = true)
            where T : MonoScript
        {
            T prefab = PrefabStorage.Get<T>(prefabName);

            prefab.gameObject.SetActive(false);
            T instance = GameObject.Instantiate<T>(prefab);
            prefab.gameObject.SetActive(true);

            initializer.Invoke(instance);
            instance.gameObject.SetActive(isActivateObject);

            return instance;
        }

        public static T CreateNew<T>(PrefabName prefabName)
            where T : UnityEngine.Object
        {
            T prefab = PrefabStorage.Get<T>(prefabName);
            return GameObject.Instantiate<T>(prefab);
        }

        public static GameObject CreateNew(PrefabName prefabName)
        {
            GameObject prefab = PrefabStorage.Get(prefabName); 
            return GameObject.Instantiate(prefab);
        }
    }
}
