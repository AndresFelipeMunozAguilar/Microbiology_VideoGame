using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
        [SerializeField] private Raycaster raycaster;

        // Referencias a tus acciones (puedes asignarlas por inspector o via el asset generado)
        [SerializeField] private InputAction _interactAction;
        [SerializeField] private InputAction _pointerPositionAction;

        public void Start()
        {

                if (ControlsManager.instance == null)
                {
                        Debug.LogError("InputHandler: No se encontr├│ la instancia de ControlsManager. Aseg├║rate de que el objeto con ControlsManager est├® presente en la escena y se haya inicializado correctamente.");
                        return;
                }


                Map.PlayerActions controls = ControlsManager.getControls();

                _interactAction = controls.Interact;
                _pointerPositionAction = controls.PointerPosition;

                // Suscribimos al evento 'started'
                _interactAction.started += OnTap;
                // Debug.Log("Se ha suscrito interact al evento on tap");

                _interactAction.Enable();
                _pointerPositionAction.Enable();


        }


        private void OnDestroy()
        {
                _interactAction.started -= OnTap;
                // Debug.Log("Se ha desuscrito interact al evento on tap");

                _interactAction.Disable();
                _pointerPositionAction.Disable();
        }


        public void OnTap(InputAction.CallbackContext context)
        {
                // Verificaci├│n de seguridad
                Debug.Log("Se ha entrado a OnTap");
                if (!context.started) return;

                // LEER POSICI├ôN UNIVERSAL
                // Al usar Pointer/Position en el Asset, ReadValue siempre devolver├í
                // la posici├│n del dedo si hay touch, o del mouse si no lo hay.
                Vector2 currentPosition = _pointerPositionAction.ReadValue<Vector2>();

                Debug.Log($"[InputHandler] Interaccion detectada. Hardware: {context.control.device.name}");
                Debug.Log($"[InputHandler] Posicion enviada al Raycaster: {currentPosition}");

                raycaster.ProcessTap(currentPosition);
        }
}