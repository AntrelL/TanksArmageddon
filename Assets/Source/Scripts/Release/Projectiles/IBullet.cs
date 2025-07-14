using Source.Scripts.Release.LandCutter;

namespace Source.Scripts.Release.Projectiles
{
    public interface IBullet
    {
        void SetLandCutter(LandCutterFacade landCutter);
    }
}