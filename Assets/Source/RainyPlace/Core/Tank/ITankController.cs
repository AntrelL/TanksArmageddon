using System;

namespace RainyPlace.Core
{
    public interface ITankController
    {
        IProtectedEvent<Action> ShotActivated { get; }

        int MovementDirection { get; }
    }
}
