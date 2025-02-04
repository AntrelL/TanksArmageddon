using System.Collections;
using TanksArmageddon.Core.CompositionRoot;
using TanksArmageddon.Core.PrefabControl;

namespace TanksArmageddon
{
    public class FirstCompositionGroup : BaseCompositionGroup
    {
        public override void CreateEnvironment()
        {
        }

        public override void Construct()
        {
            ObjectBuilder.CreateNew(PrefabName.TestObject2);

            TestObject1 testObject1Prefab = ObjectBuilder.CreateNew<TestObject1>(PrefabName.GameObject1);

            ObjectBuilder.CreateNew<TestObject1>(
                PrefabName.GameObject1, (testObject1) => testObject1.Construct("text"));

            var testObject2Prefab = PrefabStorage.Get<TestObject1>(PrefabName.GameObject1);
            ObjectBuilder.CreateNew(testObject2Prefab, 
                () => Instantiate(testObject2Prefab),
                (testObject2) => testObject2.Construct());

            var testObject3Prefab = PrefabStorage.Get<TestObject1>(PrefabName.GameObject1);
            ObjectBuilder.CreateNew(testObject3Prefab,
                () => Instantiate(testObject3Prefab),
                (testObject3) => testObject3.Construct("text"));
        }

        public override IEnumerator LazyConstruct()
        {
            yield return null;
        }
    }
}
