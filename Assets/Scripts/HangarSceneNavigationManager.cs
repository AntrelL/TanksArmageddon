using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HangarSceneNavigationManager : MonoBehaviour
{
    public void LoadHomeScene()
    {
        Debug.Log("Load HomeScene");
        SceneManager.LoadScene("HomeScene");
    }
}
