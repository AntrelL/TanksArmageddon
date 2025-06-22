using System;
using TanksArmageddon;
using UnityEngine;

public class EdgeOfMap : MonoBehaviour
{
    public static event Action<int> CollisionWithPlayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            CollisionWithPlayer?.Invoke(5000);
        }
    }
}
