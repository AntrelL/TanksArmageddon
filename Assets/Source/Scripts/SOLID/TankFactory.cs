using UnityEngine;

namespace Assets.Source.Scripts.SOLID
{
    public abstract class TankFactory : MonoBehaviour
    {
        public abstract Tank Create();
    }
}
