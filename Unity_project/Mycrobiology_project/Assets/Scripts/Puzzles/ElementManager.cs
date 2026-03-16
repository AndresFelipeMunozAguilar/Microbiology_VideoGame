using UnityEngine;
using UnityEngine.EventSystems;

public class ElementManager : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    Element element;
    bool onDrag;
    Vector2 OriginPosition;

    public void CreateElement(Element assigned)
    {
        element= assigned;
        GetComponent<SpriteRenderer>().sprite = element.image;    
        OriginPosition= transform.position;    
    }
    void FixedUpdate()
    {
        if (onDrag)
        {
            Vector2 screenPos = ControlsManager.getControls().PointerPosition.ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;

            transform.position = worldPos;
        }   
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        onDrag=true;
        GetComponent<CapsuleCollider2D>().enabled=false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<CapsuleCollider2D>().enabled=true;
        onDrag=false;
        transform.position=OriginPosition;
        
    }
    public void PlaceWarm(Vector2 newPos)
    {
        OriginPosition= newPos;
    }
}
