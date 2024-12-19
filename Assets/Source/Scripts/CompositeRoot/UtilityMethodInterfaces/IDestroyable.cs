using System;

namespace TanksArmageddon.CompositeRoot
{
    public interface IDestroyable
    {
        public event Action Destroyed;
    }
}
