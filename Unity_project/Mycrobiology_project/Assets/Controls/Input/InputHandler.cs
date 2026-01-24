using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera mainCamera;

    public void Start()
    {
        mainCamera = Camera.main;
        ControlsManager.getControls().Click.started += OnClick;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        // Debug.Log("Entre a click");

        if (!context.started) return;
        // Debug.Log("El contexto es started");

        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit.collider) return;

        // Debug.Log($"La posicion del mouse es: {Mouse.current.position.ReadValue()}");
        // Debug.Log($"Se hizo click sobre el objeto {rayHit.collider.gameObject.name}");
    }
}
