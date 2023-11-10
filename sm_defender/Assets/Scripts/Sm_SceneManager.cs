using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sm_SceneManager : MonoBehaviour
{
    static int actualScene = 0;

    public static void LoadScene(int sceneIndex)
    {
        actualScene = sceneIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(actualScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadNextScene()
    {
        actualScene++;
        UnityEngine.SceneManagement.SceneManager.LoadScene(actualScene);
    }

    public void LoadPreviousScene()
    {
        actualScene--;
        UnityEngine.SceneManagement.SceneManager.LoadScene(actualScene);
    }
}
