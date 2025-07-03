using UnityEngine;

public class PlayerRoot : MonoBehaviour
{
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private float _travelTime;

    private void Awake()
    {
        _movement.Initialize(_travelTime);
    }

    public void TakeDamage(int amount) => _health.TakeDamage(amount);
    public void EnableMovement() => _movement.SetCanMove(true);
}