using System.Collections;
using UnityEngine;

namespace TanksArmageddon
{
    [DefaultExecutionOrder(ExecutionOrderValue)]
    public class MenuBootstrap : Bootstrap
    {
        [SerializeField] private Screen _startScreen;
        [SerializeField] private ScreenSwitch _screenSwitch;

        public override IEnumerator Initialize()
        {
            _startScreen.Initialize();
            _screenSwitch.Initialize();

            return null;
        }
    }
}
