using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    Map map;
    static Map.PlayerActions controls;
    public static ControlsManager instance;
    public void Awake()
    {
        map = new Map();
        controls = map.Player;
        instance = this;
    }
    public static Map.PlayerActions getControls() { return controls; }
    private void OnEnable()
    {
        map.Enable();
    }
    private void OnDisable()
    {
        map.Disable();
    }

    public void Greet()
    {
        Debug.Log("Hello from ControlsManager");
    }


}
