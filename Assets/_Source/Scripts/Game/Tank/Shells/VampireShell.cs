using UnityEngine;

namespace TanksArmageddon
{
    public class VampireShell : Shell
    {
        [SerializeField] private int _healAmount;
        
        public override void ApplyEffect(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IAliveable aliveable))
            {
                aliveable.RestoreHealth(_healAmount);
            }
        }
    }
}