using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    Map map;
    static Map.PlayerActions controls;
    public static ControlsManager instance;
    public TextMeshProUGUI tx;
    public void Awake()
    {
        map = new Map();
        controls = map.Player;
        instance = this;
    }
    public void setError(string message)
    {
        tx.SetText(message);
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

}
