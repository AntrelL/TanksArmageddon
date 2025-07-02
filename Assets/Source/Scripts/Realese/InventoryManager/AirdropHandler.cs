using UnityEngine;

public class AirdropHandler : MonoBehaviour
{
    [SerializeField] private AirdropSpawner _spawner;
    
    private AirdropBox _box;

    public void Initialize(System.Action<int> onPickedUp)
    {
        _spawner.Spawned += () =>
        {
            _box = FindObjectOfType<AirdropBox>();
            _box.PlayerPickedUpAirdrop += onPickedUp;
        };
    }

    private void OnDisable()
    {
        if (_box != null)
            _box.PlayerPickedUpAirdrop -= null;
    }
}