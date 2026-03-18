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
    public void CreateElement(Element assigned)
    {
        element= assigned;
        GetComponent<SpriteRenderer>().sprite = element.image;    
        OriginPosition = Vector2.zero;    
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

       WarmManager warm = GetWarmManager(eventData);
        if(warm){

            warm.StartWarming(this);
            onWarm=true;
        }
        else
        {
            transform.localPosition = Vector2.zero;
        }

        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    private WarmManager GetWarmManager(PointerEventData eventData)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);

        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        if (hit != null && hit.GetComponent<WarmManager>() != null)
        {
            WarmManager warm = hit.GetComponent<WarmManager>();
            return warm;
        }
        else
        {
            return null;
        }

    }
    public void PlaceWarm(Vector2 newPos)
    { 
        OriginPosition= newPos;
        transform.position = OriginPosition;
    }
}
