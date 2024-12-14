using UnityEngine;

[RequireComponent(typeof(Tank))]
public class Player : MonoBehaviour
{
    private Tank _tank;

    private void Start()
    {
        _tank = GetComponent<Tank>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        _tank.Move((int)horizontalInput, Time.deltaTime);
    }
}
