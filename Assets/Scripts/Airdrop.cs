using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airdrop : MonoBehaviour
{
    public event Action<GameObject> OnAirDropCollected; // Событие для оповещения о сборе

    //private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        //_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Включаем гравитацию
       /* if (_rigidbody2D != null)
        {
            _rigidbody2D.gravityScale = 1f; // Настроить нужное значение гравитации в инспекторе или здесь
        }
        */
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Если соприкоснулись с ландшафтом
        if (collision.gameObject.TryGetComponent(out Land land))
        {
            return; // Ничего не делаем
        }

        // Если соприкоснулись с игроком
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            OnAirDropCollected?.Invoke(collision.gameObject); // Оповещаем о сборе
            Debug.Log("Если соприкоснулись с игроком");
            Destroy(gameObject);
            return;
        }

        // Если соприкоснулись с врагом
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            OnAirDropCollected?.Invoke(collision.gameObject); // Оповещаем о сборе
            Destroy(gameObject);
            Debug.Log("Если соприкоснулись с врагом");
        }
    }
}
