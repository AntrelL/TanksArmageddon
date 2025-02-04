using TanksArmageddon.Core.CompositionRoot;
using UnityEngine;

namespace TanksArmageddon
{
    public class TestObject1 : MonoScript
    {
        public void Construct(string text = "default")
        {
            Debug.Log("constructed");
            Debug.Log(text);

            OnConstructed();
        }
    }
}
