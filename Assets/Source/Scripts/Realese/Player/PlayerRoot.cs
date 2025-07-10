using UnityEngine;

namespace Source.Scripts.Release.Player
{
    public class PlayerRoot : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private float _travelTime;

        private void Awake()
        {
            _movement.Initialize(_travelTime);
        }
    }
}