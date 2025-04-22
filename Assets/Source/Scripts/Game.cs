using Essentia;

namespace TanksArmageddon
{
    public class Game : Program
    {
        private static void Main()
        {
            MainCamera mainCamera = new();
            EventSystem eventSystem = new();

            LevelsScreen levelsScreen = new(mainCamera.Core);
            HangarScreen hungarScreen = new(mainCamera.Core);
            ShopScreen shopScreen = new(mainCamera.Core);

            Console.Log(5);

            Vector3 direction = new(1, 4);
        }
    }
}
