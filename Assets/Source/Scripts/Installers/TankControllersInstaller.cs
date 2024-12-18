using Zenject;
using UnityEngine;
using TanksArmageddon.UI;
using TanksArmageddon.TankControl;

namespace TanksArmageddon.Installers
{
    public class TankControllersInstaller : MonoInstaller
    {
        [SerializeField] private ButtonPressListener _moveLeftButton;
        [SerializeField] private ButtonPressListener _moveRightButton;

        public override void InstallBindings()
        {
            Container.Bind<Player>().FromNew().AsSingle()
                .WithArguments(_moveLeftButton, _moveRightButton);
        }
    }
}