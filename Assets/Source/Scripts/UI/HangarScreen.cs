using Essentia;

namespace TanksArmageddon
{
    public class HangarScreen : Entity<ScreenConfig.IAccessPoint>
    {
        public HangarScreen(Camera camera = null)
        {
            Core = new Screen(Socket, camera);
        }

        public Screen Core { get; private set; }
    }
}
