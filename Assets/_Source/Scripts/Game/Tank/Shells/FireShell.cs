using UnityEngine;

namespace TanksArmageddon
{
    public class FireShell : Shell
    {
        [SerializeField] private int _damagePerTurn;
        
        public override void ApplyEffect(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IIgnitieable ignitiable))
            {
                ignitiable.Ignite(_damagePerTurn);
            }
        }
    }
}