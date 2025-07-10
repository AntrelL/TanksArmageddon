using Source.Scripts.Release.Airdrop;
using UnityEngine;

namespace Source.Scripts.Release.InventoryManager
{
    public class AirdropHandler : MonoBehaviour
    {
        [SerializeField] private AirdropSpawner _spawner;
    
        private AirdropBox _box;

        private void OnDisable()
        {
            if (_box != null)
                _box.PlayerPickedUpAirdrop -= null;
        }
    
        public void Initialize(System.Action<int> onPickedUp)
        {
            _spawner.Spawned += () =>
            {
                _box = FindObjectOfType<AirdropBox>();
                _box.PlayerPickedUpAirdrop += onPickedUp;
            };
        }
    }
}