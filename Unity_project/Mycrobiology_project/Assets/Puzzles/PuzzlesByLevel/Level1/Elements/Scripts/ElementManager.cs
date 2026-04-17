using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementManager : AbstractDraggableWorldObject
{
    Element element;
    Vector2 OriginPosition;
    DropZone currentDropZone;
    PuzzleEvaluation score;
    GamePlayElements gamePlay;
    Animator anim;
    private Vector3 originPos;
    [SerializeField] private LayerMask layerMask,ObjectMask;
    public void CreateElement(Element assigned,PuzzleEvaluation Evaluation,GamePlayElements gamePlayElements)
    {
        score=Evaluation;
        gamePlay = gamePlayElements;
        element= assigned;
        GetComponent<SpriteRenderer>().sprite = element.image;    
        //GameObject blank = getPlace(transform.position);
        //if (dropzone != null)transform.position = dropzone.getPosition();
        anim = GetComponent<Animator>();
        originPos = transform.position;
    }
    protected override void OnDragStarted()
    {
        if(currentDropZone){
            currentDropZone.setOccupied(false);
        }
    }

    protected override void OnDragEnded()
    {
        MoveZone();
        MoveTube();
        MoveWarm();
        transform.position = originPos;
        //Vector2 worldPos = Camera.main.ScreenToWorldPoint(ControlsManager.getControls().PointerPosition.ReadValue<Vector2>());
        //GameObject dropPlace = getPlace(worldPos);
    }
    void MoveZone()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, layerMask);

        if (hit != null)
        {
            Debug.Log("[Containers] " + hit.transform.name);
            originPos = transform.position;
        }
    }
    void MoveTube()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position,ObjectMask);
        if(hit != null )
        {
            if(hit.TryGetComponent(out TubeManager tube) && !tube.IsFill())
            {
                if(hit.TryGetComponent(out DropZone drop)){
                    if (!drop.IsOccupied())
                    {
                        originPos=drop.getPosition();
                        anim.Play("DropOut");
                        tube.Filling(element);
                        Debug.Log("Dropeado");
                    }
                }
            }
        }
    }
    void MoveWarm()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position,ObjectMask);
        if(hit != null )
        {
            if(hit.TryGetComponent(out DropZone drop)){
                if (!drop.IsOccupied())
                {
                    originPos=drop.getPosition();
                    drop.setOccupied(true);
                    currentDropZone=drop;
                    Debug.Log("Dropeado");
                }
            }
        }
    }


    public void DestroyElement()
    {

        //dropzone.setOccupied(false);
        //gamePlay.addFinishElement();
        Destroy(gameObject);   
    }
    public void PlaceWarm(Vector2 newPos)
    {
        OriginPosition= newPos;
        transform.position = OriginPosition;
    }
}
