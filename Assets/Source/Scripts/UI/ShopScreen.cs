using Essentia;

namespace TanksArmageddon
{
    public class ShopScreen : Entity<ScreenConfig.IAccessPoint>
    {
        public ShopScreen(Camera camera = null)
        {
            Core = new Screen(Socket, camera);
        }

        public Screen Core { get; private set; }
    }
}
