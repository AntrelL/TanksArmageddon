using System;
using System.Collections;
using System.Collections.Generic;
using TanksArmageddon;
using UnityEngine;

public class EdgeOfMap : MonoBehaviour
{
    public static event Action<int> CollisionWithPlayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            Debug.Log("Player hit edge of map");
            CollisionWithPlayer?.Invoke(1000);
        }
    }
}
