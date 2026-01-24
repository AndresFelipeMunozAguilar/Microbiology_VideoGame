using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // private Camera mainCamera;

    // public void Start()
    // {
    //     mainCamera = Camera.main;
    //     ControlsManager.getControls().Interact.started += OnClick;
    // }

    // public void OnClick(InputAction.CallbackContext context)
    // {
    //     // Debug.Log("Entre a click");

    //     if (!context.started) return;
    //     // Debug.Log("El contexto es started");

    //     RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

    //     if (!rayHit.collider) return;

    //     Debug.Log($"La posicion del mouse es: {Mouse.current.position.ReadValue()}");
    //     Debug.Log($"Se hizo click sobre el objeto {rayHit.collider.gameObject.name}");
    // }

    // ===================== WOKRING VERSION =====================

    [SerializeField]
    private Raycaster raycaster;

    public void Start()
    {
        ControlsManager.getControls().Interact.started += OnTap;
        Debug.Log("Se ha suscrito interact al evento on tap");
    }

    public void OnDestroy()
    {
        ControlsManager.getControls().Interact.started -= OnTap;
        Debug.Log("Desuscrito interact al evento on tap");
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        Debug.Log("Se ha entrado a OnTap");
        if (!context.started) return;

        Vector2 tapPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Debug.Log($"La posicion del tap es: {tapPosition}");

        raycaster.ProcessTap(tapPosition);
    }
}
