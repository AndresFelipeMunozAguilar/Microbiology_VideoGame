using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private Raycaster raycaster;

    public void Start()
    {
        ControlsManager.getControls().Interact.performed += OnTap;
    }

    public void OnDestroy()
    {
        ControlsManager.getControls().Interact.performed -= OnTap;
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        Vector2 tapPosition = ControlsManager.getControls().Position.ReadValue<Vector2>();

        raycaster.ProcessTap(tapPosition);
    }
}
