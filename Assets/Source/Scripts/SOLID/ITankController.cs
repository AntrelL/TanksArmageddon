using System;

namespace Assets.Source.Scripts.SOLID
{
    public interface ITankController
    {
        public event Action ShotActivated;
        int MovementDirection { get; }
    }
}
