using UnityEngine;
using UnityEngine.InputSystem;
public class ControlsManager : MonoBehaviour
{
    Map map;
    static Map.ControlsActions controls;
    public static ControlsManager instance;
    private void Awake()
    {
        map = new Map();
        controls = map.controls;
        instance = this;
    }
    public static Map.ControlsActions getControls() { return controls; }
    private void OnEnable()
    {
        map.Enable();
    }
    private void OnDisable()
    {
        map.Disable();
    }

}
