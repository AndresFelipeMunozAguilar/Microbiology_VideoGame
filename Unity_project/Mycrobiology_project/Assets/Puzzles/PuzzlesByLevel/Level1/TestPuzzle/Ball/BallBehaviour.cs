using UnityEngine;
using UnityEngine.EventSystems;

public class BallBehaviour : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private float zDistanceToCamera;

    [SerializeField]
    private Rigidbody2D rb;

    public void Start()
    {
        mainCamera = Camera.main;

        // Se guarda la distancia Z entre el objeto y la cámara para que no salte al arrastrar
        zDistanceToCamera = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);

        // Se obtiene el componente rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Se obtiene la posición ABSOLUTA del puntero en 
        // pantalla (píxeles)
        Vector3 screenPointerPosition = eventData.position;

        // Se le asigna la profundidad Z que el objeto ya 
        // tenía en el mundo
        screenPointerPosition.z = zDistanceToCamera;

        // Se convierte esa posición de pantalla a una coordenada 
        // exacta en el mundo
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPointerPosition);

        // Se aplica la posición al objeto (manteniendo su Z 
        // original si es necesario)
        transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);

        // Finalmente, se elimina la velocidad del rigidbody, 
        // para evitar que mientras se arrastra siga aplicandose 
        // fuerza hacia abajo y, por tanto, al soltar el balón salga despedido 
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Basket BALL: OnPOinterDown");
    }
}