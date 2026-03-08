using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
        [SerializeField] private Raycaster raycaster;

        // Referencias a tus acciones (puedes asignarlas por inspector o vía el asset generado)
        [SerializeField] private InputAction _interactAction;
        [SerializeField] private InputAction _pointerPositionAction;

        public void Start()
        {
                _interactAction = ControlsManager.getControls().Interact;
                _pointerPositionAction = ControlsManager.getControls().PointerPosition;

                _interactAction.started += OnTap;
                // Debug.Log("Se ha suscrito interact al evento on tap");
        }

        public void OnDestroy()
        {
                _interactAction.started -= OnTap;
                // Debug.Log("Desuscrito interact al evento on tap");
        }

        public void OnTap(InputAction.CallbackContext context)
        {
                Debug.Log("Se ha entrado a OnTap");
                if (!context.started) return;

                Vector2 currentPosition = _pointerPositionAction.ReadValue<Vector2>();

                Debug.Log($"La posicion del tap es: {currentPosition}");

                raycaster.ProcessTap(currentPosition);
        }
}
