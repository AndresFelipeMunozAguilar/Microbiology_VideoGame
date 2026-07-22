using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] List<GameScenes> scenes;
    public GameObject registerWindow,loginWindow;
    public Button btPlay;
    void Start()
    {
        if (!btPlay)
        {
            btPlay = GameObject.Find("Bt_play").GetComponent<Button>();
        }
        if(btPlay)btPlay.interactable=false;
        LoginWindowState(false);
        RegisterWindowState(false);
    }
    public void ChangeScene(int nextScene)
    {
        GameScenes sceneToLoad = scenes[nextScene];
        if(sceneToLoad == GameScenes.GameDemo_B || sceneToLoad == GameScenes.GameDemo)
        {
            if (EvaluationSystem.Instance != null)
            {
                EvaluationSystem.Instance.ResetProgress();
            }
            AudioManager.Instance.PlayMusic("gameMusic");
        }
        else
        {
            AudioManager.Instance.PlayMusic("menuMusic");
        }
        SceneManager.LoadScene(scenes[nextScene].ToString());
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Registrar()
    {
        UserRegister.Instance.Registrar();
    }
    public void Loguear()
    {
         UserRegister.Instance.Login();
    }
    public void activePlay()
    {
        btPlay.interactable=true;
    }
    public void RegisterWindowState(bool state)
    {
        if(registerWindow)registerWindow.SetActive(state);
    }
    public void LoginWindowState(bool state)
    {
        if(loginWindow)loginWindow.SetActive(state);
    }
}
