using System.Collections;
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
    [SerializeField] private AnimationClip DropAnimation;
    public void CreateElement(Element assigned,PuzzleEvaluation Evaluation,GamePlayElements gamePlayElements)
    {
        score=Evaluation;
        gamePlay = gamePlayElements;
        element= assigned;
        GetComponent<SpriteRenderer>().sprite = element.image;    
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
        MoveTube();
        MoveZone();
        MoveWarm();
        MoveFinish();
        if(currentDropZone)currentDropZone.setOccupied(true);
        transform.position = originPos;

    }
    IEnumerator BlockMove(float amountTime)
    {
        GetComponent<CapsuleCollider2D>().enabled=false;
        yield return new WaitForSeconds(amountTime);
        GetComponent<CapsuleCollider2D>().enabled=true;

    }

    void MoveZone()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, layerMask);

        if (hit != null)
        {
            currentDropZone=null;
            Debug.Log("[Containers] " + hit.transform.name);
            originPos = transform.position;
        }
    }
    void MoveTube()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position,ObjectMask);
        if(hit != null )
        {
            if(hit.TryGetComponent(out TubeManager tube))
            {
                if (tube.IsFill())
                {
                    score.RemovePoints("Spam");
                    gamePlay.CreateFeedback(transform.position,"Tubo ocupado");
                    return;
                }
                if(hit.TryGetComponent(out DropZone drop)){
                    if (!drop.IsOccupied())
                    {
                        originPos=drop.getPosition();
                        anim.Play("DropOut");
                        StartCoroutine(BlockMove(DropAnimation.length));
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
            if(hit.TryGetComponent(out DropZone drop) && !hit.TryGetComponent(out TubeManager tube)){
                if (!drop.IsOccupied()&& !drop.IsFinishZone())
                {
                     score.RemovePoints("Spam");
                    originPos=drop.getPosition();
                    drop.setOccupied(true);
                    currentDropZone=drop;
                    gamePlay.CreateFeedback(transform.position,"Usa un tubo");
                    Debug.Log("Dropeado");
                }
            }
        }
    }
    void MoveFinish()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position,ObjectMask);
        if(hit != null )
        {
            
             if(hit.TryGetComponent(out DropZone drop)){
                if (!drop.IsOccupied() && drop.IsFinishZone())
                {
                    score.RemovePoints("Spam");
                    gamePlay.CreateFeedback(transform.position,"No es el objeto");
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
