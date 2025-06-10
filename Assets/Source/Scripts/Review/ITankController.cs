using System;

namespace Realese
{
    public interface ITankController
    {
        IProtectedEvent<Action> ShotActivated { get; }

        int MovementDirection { get; }
    }
}
