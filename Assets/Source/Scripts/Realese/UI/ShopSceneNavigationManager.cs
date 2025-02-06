using IJunior.TypedScenes;
using UnityEngine;

public class ShopSceneNavigationManager : MonoBehaviour
{
    public void LoadMainScene()
    {
        Debug.Log("Load MainScene");
        MainScene.Load();
    }
}
