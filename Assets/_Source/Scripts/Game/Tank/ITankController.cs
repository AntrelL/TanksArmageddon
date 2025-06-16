using System;

namespace TanksArmageddon
{
    public interface ITankController
    {
        event Action ShotActivated;
        
        int MovementDirection { get; }
        int CannonRotateDirection { get; }
    }
}
