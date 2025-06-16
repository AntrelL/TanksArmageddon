using System;
using TanksArmageddon;
using UnityEngine;

public class EdgeOfMap : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            Debug.Log("Player hit edge of map");
            CollisionWithPlayer?.Invoke(5000);
        }
    }

    public static event Action<int> CollisionWithPlayer;
}