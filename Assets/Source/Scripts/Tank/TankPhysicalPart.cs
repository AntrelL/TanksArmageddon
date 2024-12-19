using UnityEngine;
using TanksArmageddon.CompositeRoot;

namespace TanksArmageddon.TankComponents
{
    [RequireComponent(typeof(Collider2D))]
    public class TankPhysicalPart : MonoScript, IConstructable<Tank>
    {
        public void Construct(Tank parentTank)
        {
            Collider2D = GetComponent<Collider2D>();
            ParentTank = parentTank;
        }

        public Tank ParentTank { get; private set; }

        public Collider2D Collider2D { get; private set; }
    }
}
