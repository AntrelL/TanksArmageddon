using Zenject;
using UnityEngine;
using TanksArmageddon.TankControl;

namespace TanksArmageddon.Installers
{
    public class TankSpawnerInstaller : MonoInstaller
    {
        [SerializeField] private TankSpawner _tankSpawner;

        public override void InstallBindings()
        {
            Container.Bind<TankSpawner>().FromInstance(_tankSpawner);
        }
    }
}
