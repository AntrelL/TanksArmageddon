using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Tank : MonoBehaviour
{
    [SerializeField] private TankMovement _tankMovement;

    private void Awake()
    {
        _tankMovement.Construct(GetComponent<Rigidbody2D>());
    }

    public void Move(int direction, float deltaTime)
    {
        _tankMovement.Move(direction, deltaTime);
    }
}
