using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneNavigationManager : MonoBehaviour
{
    public void LoadLevel()
    {
        Debug.Log("Load level");
        SceneManager.LoadScene("TestScene");
    }
}
