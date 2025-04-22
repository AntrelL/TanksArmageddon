using Essentia;

namespace TanksArmageddon
{
    public class MainCamera : Entity<MainCameraConfig.IAccessPoint>
    {
        public Camera Core => Config.Camera;
    }
}
