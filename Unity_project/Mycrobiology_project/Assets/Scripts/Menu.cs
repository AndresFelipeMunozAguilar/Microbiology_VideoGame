using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] List<GameScenes> scenes;
    public void ChangeScene(int nextScene)
    {
        SceneManager.LoadScene(scenes[nextScene].ToString());
    }
    public void Quit()
    {
        Application.Quit();
    }
}
