using UnityEngine;

namespace TanksArmageddon
{
    [DefaultExecutionOrder(-9000)]
    public class EarlyObjectActivitySetter : MonoBehaviour
    {
        [SerializeField] private bool _state;
        
        private void Awake() => gameObject.SetActive(_state);
    }
}
