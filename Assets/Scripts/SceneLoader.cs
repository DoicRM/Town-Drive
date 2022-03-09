using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    static public void LoadNextScene()
    {
        SoundManagerScript.PlaySound("playerGetNot");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    static public void LoadStartScene()
    {
        SoundManagerScript.PlaySound("playerGetNot");
        SceneManager.LoadScene(0);
    }

    static public void QuitGame()
    {
        SoundManagerScript.PlaySound("playerGetNot");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
