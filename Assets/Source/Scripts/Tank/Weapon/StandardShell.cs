using UnityEngine;

public class StandardShell : Shell
{
    [SerializeField] [Min(0)] private float _shotPower;
    [SerializeField] [Min(0)] private float _explosionRadius;
    [SerializeField] [Min(0)] private float _damage;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _entityLayerMask;

    private float _minMapHeight = -10f;

    private void FixedUpdate()
    {
        Vector2 movementDirection = Rigidbody2D.velocity.normalized;
        Rigidbody2D.MoveRotation(Quaternion.LookRotation(movementDirection));

        if (Transform.position.y < _minMapHeight)
            ParentTankArmament.ReturnShell(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == ParentTankArmament.MainTankObject)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(Transform.position, _explosionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Entity entity))
            {
                float distance = Vector2.Distance(Transform.position, collider.ClosestPoint(Transform.position));

                float damage = _damage * (_explosionRadius - distance);
                entity.TakeDamage(damage);
                Debug.Log($"hit entity, distance: {distance}, damage: {damage}");
                continue;
            }

            if (collider.gameObject.layer == _groundLayerMask)
            {
                Debug.Log("hit: ground");
                continue;
            }
        }

        ParentTankArmament.ReturnShell(this);
    }

    public override void Activate(Vector2 startPosition, Vector2 direction)
    {
        Transform.position = startPosition;
        Rigidbody2D.AddForce(direction * _shotPower, ForceMode2D.Impulse);
    }

    public override void ResetValues()
    {
        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.angularVelocity = 0f;
    }
}
