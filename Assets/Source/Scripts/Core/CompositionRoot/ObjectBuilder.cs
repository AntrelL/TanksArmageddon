using System;

namespace TanksArmageddon.Core.CompositionRoot
{
    public static class ObjectBuilder
    {
        public static T CreateNew<T>(T prefab, Func<T> creator, Action<T> initializer, bool isActivateObject = true)
            where T : MonoScript
        {
            prefab.gameObject.SetActive(false);
            T instance = creator.Invoke();
            prefab.gameObject.SetActive(true);

            initializer.Invoke(instance);
            instance.gameObject.SetActive(isActivateObject);

            return instance;
        }
    }
}
