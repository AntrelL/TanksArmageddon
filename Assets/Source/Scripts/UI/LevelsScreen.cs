using Essentia;

namespace TanksArmageddon
{
    public class LevelsScreen : Entity<ScreenConfig.IAccessPoint>
    {
        public LevelsScreen(Camera camera = null)
        {
            Core = new Screen(Socket, camera);
        }

        public Screen Core { get; private set; }
    }
}
