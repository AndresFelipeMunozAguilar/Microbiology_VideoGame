using UnityEngine;
using UnityEngine.EventSystems;

// La clase AbstractDraggableWorldObject es especifica 
// para objetos que están dentro del mundo de juego, 
// eso implica que no sirve para elementos de la GUI
[RequireComponent(typeof(Rigidbody2D), typeof(CanvasGroup))]
public abstract class AbstractDraggableWorldObject : MonoBehaviour,
    IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField] protected float _zDistanceToCamera;
    [SerializeField] protected Camera _mainCamera;
    [SerializeField] protected CanvasGroup _canvasGroup;
    [SerializeField] protected Rigidbody2D _rigidbody;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        _mainCamera = Camera.main;

        // Se guarda la distancia Z entre el objeto y la cámara para que no salte al arrastrar
        _zDistanceToCamera = Mathf.Abs(_mainCamera.transform.position.z - transform.position.z);

        OnInstanceStart();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // Lógica común: Resaltar objeto o sonido de click
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (_canvasGroup != null) _canvasGroup.blocksRaycasts = false;

        // Al empezar, le decimos al Rigidbody que ignore la gravedad y fuerzas
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0;

        OnDragStarted(); // Hook para subclases
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Lógica común: Transformación de coordenadas (Mundo)
        // Se obtiene la posición ABSOLUTA del puntero en 
        // pantalla (píxeles)
        Vector3 screenPointerPosition = eventData.position;

        // Se le asigna la profundidad Z que el objeto ya 
        // tenía en el mundo, para evitar empujones al fondo
        screenPointerPosition.z = _zDistanceToCamera;

        Vector3 pointerWorldPosition = _mainCamera.ScreenToWorldPoint(screenPointerPosition);

        // Se aplica la posición al objeto (manteniendo su Z 
        // original si es necesario)
        transform.position = pointerWorldPosition;

        // Finalmente, se elimina la velocidad del rigidbody, 
        // para evitar que mientras se arrastra siga aplicandose 
        // fuerza hacia abajo y, por tanto, al soltar el balón salga despedido 
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0;

        OnDuringDrag(); // Hook para subclases
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (_canvasGroup != null) _canvasGroup.blocksRaycasts = true;
        OnDragEnded(); // Hook para subclases
    }


    // "Hooks" o Métodos Plantilla: Las hijas deciden si usarlos o no
    protected virtual void OnInstanceStart() { }
    protected virtual void OnDragStarted() { }
    protected virtual void OnDuringDrag() { }
    protected virtual void OnDragEnded() { }
}