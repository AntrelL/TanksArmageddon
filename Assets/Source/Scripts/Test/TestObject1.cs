using TanksArmageddon.Core.CompositionRoot;
using UnityEngine;

namespace TanksArmageddon
{
    public class TestObject1 : MonoScript
    {
        public void Construct(string text)
        {
            Debug.Log("constructed");
            Debug.Log(text);

            OnConstructed();
        }
    }
}
