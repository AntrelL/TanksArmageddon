using IJunior.TypedScenes;
using UnityEngine;

public class HangarSceneNavigationManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void LoadHomeScene()
    {
        Debug.Log("Load HomeScene");
        MainScene.Load();
    }
}
