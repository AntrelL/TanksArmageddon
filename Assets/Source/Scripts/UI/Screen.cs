using Essentia;

namespace TanksArmageddon
{
    public class Screen : Script<ScreenConfig.IAccessPoint>
    {
        public Screen(Socket<ScreenConfig.IAccessPoint> socket, Camera camera = null) : base(socket)
        {
            Config.Canvas.WorldCamera = camera;
        }
    }
}
