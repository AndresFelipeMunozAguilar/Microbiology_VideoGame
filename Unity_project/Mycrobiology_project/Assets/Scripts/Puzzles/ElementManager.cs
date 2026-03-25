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
    [SerializeField] RectTransform pointWarm;
    [SerializeField] float maxTime = 10f;
    DropZone dropzone;
    PuzzleEvaluation score;
    GamePlayElements gamePlay;
    public void CreateElement(Element assigned,PuzzleEvaluation Evaluation,GamePlayElements gamePlayElements)
    {
        score=Evaluation;
        gamePlay = gamePlayElements;
        element= assigned;
        GetComponent<SpriteRenderer>().sprite = element.image;    
        float posy = Mathf.Lerp(3.6f, -3.6f, element.timeWarm / 100f);
        Vector2 posPoint = new Vector2(pointWarm.anchoredPosition.x,posy);
        pointWarm.anchoredPosition = posPoint;
        GameObject blank = getPlace(transform.position);
        transform.position=dropzone.getPosition();
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
            Temperature = target;
            WarmBar.fillAmount = target;
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
        if(dropPlace && dropPlace.TryGetComponent(out WarmManager warm)){
            onWarm=true;
            if (warm.getSwitch())
            {
               if(element.isMechero) score.AddPoints("GoodPlace"); 
               else score.RemovePoints("BadPlace");
               if(element.isMechero) Debug.Log("[ELEMENT] bien puesto en el mechero");
               else  Debug.Log("[ELEMENT] mal puesto en el mechero");
            }
            else
            {
                if(element.isBañoMaria) score.AddPoints("GoodPlace");
               else score.RemovePoints("BadPlace");
                if(element.isBañoMaria) Debug.Log("[ELEMENT] bien puesto en el baño maria");
               else  Debug.Log("[ELEMENT] mal puesto en el baño maria");
            }
            Debug.Log("[ELEMENT] Score: "+score.GetCurrentScore());
            
        }
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    private GameObject getPlace(Vector2 pos=default,PointerEventData eventData=null)
    {
        Vector2 worldPos = eventData!=null ? Camera.main.ScreenToWorldPoint(eventData.position):pos;

        Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out DropZone drop))
            {   
                if(!drop.IsOccupied()){
                    if(dropzone)dropzone.setOccupied(false);
                    dropzone=drop;
                    bool isFinish= dropzone.setOccupied(true);
                    if(isFinish)FinishElement();
                }
                return hit.gameObject;
            }
        }

        return null;
    }
    void FinishElement()
    {
        GetComponent<Animator>().Play("Fade");
        
    }
    public void DestroyElement()
    {
        Temperature*=100;
        if(Temperature >= element.timeWarm-10f && Temperature <= element.timeWarm + 10f)
        {
            score.AddPoints("FinishElement");
            Debug.Log("[ELEMENT] bien hecho temperatura: "+Temperature+" estuvo en el rango: "+element.timeWarm);
        }
        else
        {
            score.AddPoints("FinishBadElement");
            Debug.Log("[ELEMENT] mal hecho temperatura: "+Temperature+" no estuvo en el rango: "+element.timeWarm);
        }
        Debug.Log("[ELEMENT] Score: "+score.GetCurrentScore());
        dropzone.setOccupied(false);
        gamePlay.addFinishElement();
        Destroy(gameObject);   
    }
    public void PlaceWarm(Vector2 newPos)
    {
        OriginPosition= newPos;
        transform.position = OriginPosition;
    }
}
