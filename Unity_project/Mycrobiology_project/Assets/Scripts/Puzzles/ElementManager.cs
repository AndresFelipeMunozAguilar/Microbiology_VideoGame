using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementManager : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    Element element;
    bool onDrag,onWarm;
    float currentTime;
    [SerializeField] float Temperature;
    Vector2 OriginPosition;
    [SerializeField] Image WarmBar;
    [SerializeField] float maxTime = 3f;
    DropZone dropzone;
    public void CreateElement(Element assigned)
    {
        element= assigned;
        GetComponent<SpriteRenderer>().sprite = element.image;    
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);

        GameObject blank = getPlace(transform.position);
        if(blank)Debug.Log("<color=red></color>");
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
        if (onWarm)
        {
            currentTime += Time.deltaTime;
            float target = currentTime / maxTime;
            Temperature = Mathf.Lerp(Temperature,target,Time.deltaTime);
            WarmBar.fillAmount = Mathf.Lerp(WarmBar.fillAmount,target,Time.deltaTime);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        onDrag=true;
        GetComponent<CapsuleCollider2D>().enabled=false;
        onWarm=false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onDrag = false;

        GameObject dropPlace = getPlace(eventData:eventData);
        transform.position=dropzone.getPosition(); //si genera error es porque no esta asignado al iniciar un espacio
        dropPlace = getPlace(transform.position);
        if(dropPlace && dropPlace.TryGetComponent<WarmManager>(out WarmManager warm)){
            onWarm=true;
        }
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    private GameObject getPlace(Vector2 pos=default,PointerEventData eventData=null)
    {
        Vector2 worldPos = eventData!=null ? Camera.main.ScreenToWorldPoint(eventData.position):pos;

        Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<DropZone>(out DropZone drop))
            {
                if(!drop.IsOccupied()){
                    if(dropzone)dropzone.setOccupied(false);
                    dropzone=drop;
                    dropzone.setOccupied(true);
                }
                return hit.gameObject;
            }
        }

        return null;
    }
    public void PlaceWarm(Vector2 newPos)
    { 
        OriginPosition= newPos;
        transform.position = OriginPosition;
    }
}
