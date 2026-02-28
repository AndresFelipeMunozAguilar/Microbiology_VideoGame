using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private Raycaster raycaster;

    public void Start()
    {
        ControlsManager.getControls().Interact.started += OnTap;
        // Debug.Log("Se ha suscrito interact al evento on tap");
    }

    public void OnDestroy()
    {
        ControlsManager.getControls().Interact.started -= OnTap;
        // Debug.Log("Desuscrito interact al evento on tap");
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        // Debug.Log("Se ha entrado a OnTap");
        Vector2 tapPosition = context.ReadValue<Vector2>();
        // Debug.Log($"La posicion del tap es: {tapPosition}");

        raycaster.ProcessTap(tapPosition);
    }
}
