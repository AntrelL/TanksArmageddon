using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public abstract class Shell : MonoBehaviour
{
    protected Transform Transform { get; private set; }

    protected Rigidbody2D Rigidbody2D { get; private set; }

    protected TankArmament ParentTankArmament { get; private set; }

    public void Construct(TankArmament parentTankArmament)
    {
        Transform = transform;

        Rigidbody2D = GetComponent<Rigidbody2D>();
        ParentTankArmament = parentTankArmament;
    }

    public abstract void Activate(Vector2 startPosition, Vector2 direction);

    public abstract void ResetValues();
}
