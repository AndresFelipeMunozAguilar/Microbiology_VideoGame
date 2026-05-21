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
        btPlay.interactable=false;
        LoginWindowState(false);
        RegisterWindowState(false);
    }
    public void ChangeScene(int nextScene)
    {
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
        registerWindow.SetActive(state);
    }
    public void LoginWindowState(bool state)
    {
        loginWindow.SetActive(state);
    }
}
