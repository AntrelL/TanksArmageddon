using UnityEditor.Compilation;

namespace Realese
{
    public abstract class TankFactory : MonoScript
    {
        public abstract Tank Create();
    }
}
