using UnityEngine;

public class TubeManager : AbstractDraggableWorldObject
{
    Animator anim;
    Element element;
    Vector2 OriginPosition;
    bool isFill, onWarm;
    [Header("Player Damage and Health Logic")]
    [SerializeField]
    private PlayerHealthLogic _playerHealthLogic;    
    DropZone dropzone;
    PuzzleEvaluation score;
    GamePlayElements gamePlay;
    WarmManager CurrentWarm;
    bool usedWrongMethod = false;
    [SerializeField] private float temperatureSpeed = 20f; // grados por segundo
    [SerializeField] private float Temperature = 0f;
    private Vector3 originPos;
    [SerializeField] private LayerMask layerMask,ObjectMask;
    DropZone currentDropZone;
    public void CreateElement(PuzzleEvaluation Evaluation,GamePlayElements gamePlayElements)
    {
        score=Evaluation;
        gamePlay = gamePlayElements;
        //GameObject blank = getPlace(transform.position);
        //if (dropzone != null)transform.position = dropzone.getPosition();
        anim = GetComponent<Animator>();
        dropzone=GetComponent<DropZone>();
        originPos = transform.position;
    }

    void FixedUpdate()
    {
        if (onWarm)
        {
            Temperature += temperatureSpeed * Time.deltaTime;
            Temperature = Mathf.Clamp(Temperature, 0f, 100f);

            CurrentWarm.newTemperature(Temperature);
        }
    }

    public bool IsFill() => element != null;
    public void Filling(Element newElement)
    {
        anim.Play("Filling");
        element = newElement;
    }

    protected override void OnDragStarted()
    {
        if(currentDropZone){
            currentDropZone.setOccupied(false);
        }
        dropzone.setOccupied(false);
        onWarm=false;
    }

    protected override void OnDragEnded()
    {
        MoveZone();
        MoveWarm();
        MoveFinish();
        transform.position=originPos;
    }

    void MoveWarm()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, ObjectMask);
        if(hit != null )
        { 
            Debug.Log("[Elements] movewarm " + hit.name);
            if(hit.TryGetComponent(out WarmManager warm)){
                Debug.Log("[Elements] si tiene warm ");
                if(IsFill()){
                     Debug.Log("[Elements] si esta lleno ");
                    CurrentWarm = warm;
                    if (warm.getSwitch())
                    {
                        if(element.isMechero){
                            onWarm=true;
                            score.AddPoints("GoodPlace");
                        } 
                        else{
                            score.RemovePoints("BadPlace");
                        }

                    }
                    else
                    {
                        if(element.isBañoMaria){
                            onWarm=true;
                            score.AddPoints("GoodPlace");
                        } 
                        else{
                            score.RemovePoints("BadPlace");
                        }
                    }
                }
                Debug.Log("[Elements] Score: "+score.GetCurrentScore());
                if(hit.TryGetComponent(out DropZone drop) && !drop.IsOccupied()){
                    Debug.Log("[Elements] Warm " + hit.transform.name);
                    originPos=drop.getPosition();
                    drop.setOccupied(true);
                    dropzone.setOccupied(true);
                    currentDropZone=drop;          
                }
            }
        }
    }
    void MoveZone()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, layerMask);

        if (hit != null)
        {
            Debug.Log("[Elements] zone " + hit.transform.name);
            originPos = transform.position;
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
                    Debug.Log("[Elements] Finish " + hit.transform.name);
                    originPos=drop.getPosition();
                    FinishElement();
                }
             }
        }
    }
   
    public void DestroyElement()
    {
        if(Temperature >= element.targetTemperature-10f && Temperature <= element.targetTemperature + 10f)
        {
            score.AddPoints("FinishElement");
            Debug.Log("[ELEMENT] bien hecho temperatura: "+Temperature+" estuvo en el rango: "+element.targetTemperature);
        }
        else
        {
            score.AddPoints("FinishBadElement");
            _playerHealthLogic=FindAnyObjectByType<PlayerHealthLogic>();
            GetComponentInParent<PlayerDamageDealer>().CalculateDamage(score.GetCurrentScore());
            GetComponentInParent<PlayerDamageDealer>().DealDamage(_playerHealthLogic);
            Debug.Log("[ELEMENT] mal hecho temperatura: "+Temperature+" no estuvo en el rango: "+element.targetTemperature);
        }
        Debug.Log("[ELEMENT] Score: "+score.GetCurrentScore());
    }    
    void FinishElement()
    {
        Destroy(gameObject,1f);
        
    }

}
