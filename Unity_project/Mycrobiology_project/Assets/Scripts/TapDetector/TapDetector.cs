using UnityEngine;
using UnityEngine.InputSystem;

//Requiere tener un collider para detectar los taps
public class TapDetector : MonoBehaviour
{

    [SerializeField] private InputActionReference tapAction;
    [SerializeField] private MonoBehaviour tapActionBehaviour;

    private ITapAction tapActionHandler;
    private Camera mainCamera;


    private void Awake()
    {

        Debug.Log(tapAction.action.actionMap.enabled ? "El Mapa Input Actions Asset está activo" : "No se activo el mapa Input Actions Asset");

        Debug.Log(tapActionBehaviour != null ? $"tapActionBehaviour assigned: {tapActionBehaviour.name} (type {tapActionBehaviour.GetType()})" : "tapActionBehaviour NOT assigned");
        tapActionHandler = tapActionBehaviour as ITapAction;
        Debug.Log(tapActionHandler != null ? "tapActionBehaviour implements ITapAction" : "tapActionBehaviour does NOT implement ITapAction");

        mainCamera = Camera.main;
        Debug.Log(mainCamera != null ? $"Main Camera found: {mainCamera.name}" : "Main Camera NOT found");

        Debug.Log(tapAction != null ? $"Tap Action found: {tapAction.name}" : $"Tap Action Tap NOT found");
    }

    private void OnEnable()
    {
        Debug.Log("TapDetector.OnEnable called");
        if (tapAction != null)
        {
            tapAction.action.performed += OnTapPerformed;
            Debug.Log("tapAction enabled and subscribed to performed");
        }
        else
        {
            Debug.LogWarning("tapAction is null in OnEnable - cannot enable or subscribe");
        }
    }

    private void OnDisable()
    {
        Debug.Log("TapDetector.OnDisable called");
        if (tapAction != null)
        {
            tapAction.action.performed -= OnTapPerformed;
            Debug.Log("tapAction unsubscribed and disabled");
        }
        else
        {
            Debug.LogWarning("tapAction is null in OnDisable - nothing to unsubscribe/disable");
        }
    }

    public void OnTapPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"OnTapPerformed invoked - phase: {context.phase}");

        Vector2 screenPosition;
        if (Touchscreen.current != null)
        {
            screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Debug.Log($"Touchscreen position read: {screenPosition}");
        }
        else if (Mouse.current != null)
        {
            screenPosition = Mouse.current.position.ReadValue();
            Debug.Log($"Touchscreen not available, using Mouse position: {screenPosition}");
        }
        else
        {
            screenPosition = Vector2.zero;
            Debug.LogWarning("No input device available to read position");
        }

        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera is null - cannot raycast");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        Debug.Log($"Ray created - origin: {ray.origin}, direction: {ray.direction}");

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"Raycast hit: {hit.collider.name} at {hit.point} (distance {hit.distance})");
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("Hit is this GameObject -> invoking tap handler");
                if (tapActionHandler != null)
                {
                    tapActionHandler.OnTap(gameObject);
                    Debug.Log("tapActionHandler.OnTap invoked");
                }
                else
                {
                    Debug.LogWarning("tapActionHandler is null - no handler to invoke");
                }
            }
            else
            {
                Debug.Log($"Hit another object: {hit.collider.gameObject.name}");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any collider");
        }
    }
}