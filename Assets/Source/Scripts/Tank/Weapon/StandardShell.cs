using UnityEngine;

public class StandardShell : Shell
{
    [SerializeField] [Min(0)] private float _shotPower;

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
        Debug.Log("hit");
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
