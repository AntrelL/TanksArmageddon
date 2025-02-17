using TanksArmageddon;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    private Cutter _cutter;

    private void Start()
    {
        _cutter = FindObjectOfType<Cutter>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EdgeOfMap edgeOfMap))
        {
            Debug.Log("Hit edge of map");
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.PlayHitEffect(transform.position);
            Debug.Log("Hit player");
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.PlayHitEffect(transform.position);
            Debug.Log("Hit enemy");
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Land land))
        {
            _cutter.transform.position = transform.position;
            Debug.Log("Hit land");
            Invoke(nameof(DoCut), 0.001f);
        }
    }

    private void DoCut()
    {
        Debug.Log("DoCut beep");
        _cutter.DoCut();
        Destroy(gameObject);
    }
}
