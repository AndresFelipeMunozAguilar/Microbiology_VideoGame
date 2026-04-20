using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TubeManager : AbstractDraggableWorldObject
{
    Animator anim;
    Element element;
    bool onWarm;

    DropZone dropzone;
    PuzzleEvaluation score;
    GamePlayElements gamePlay;
    WarmManager CurrentWarm;
    [SerializeField] private float temperatureSpeed = 20f; // grados por segundo
    [SerializeField] private float Temperature = 0f;
    private Vector3 originPos;
    [SerializeField] private LayerMask layerMask,ObjectMask;
    DropZone currentDropZone;
    [SerializeField] private AnimationClip fillingClip,VanishClip;
    float wrongMethodTimer = 0f;
    [SerializeField] float wrongThreshold = 1f;
    bool inWrongMethod = false;
    bool isDamaged = false;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] SpriteRenderer sustance1,sustance2;
    public void CreateElement(PuzzleEvaluation Evaluation,GamePlayElements gamePlayElements)
    {
        score=Evaluation;
        gamePlay = gamePlayElements;
        anim = GetComponent<Animator>();
        dropzone=GetComponent<DropZone>();
        originPos = transform.position;
        title.text="";
    }

    void FixedUpdate()
    {
        if (onWarm)
        {
            if (!IsCorrectWarm())
            {
                wrongMethodTimer +=Time.deltaTime;
                if (wrongMethodTimer >= wrongThreshold)
                {
                    StartCoroutine(DarkenCoroutine());
                }
            }
            Temperature += temperatureSpeed * Time.deltaTime;
            Temperature = Mathf.Clamp(Temperature, 0f, 100f);
            CurrentWarm.setColor(GetColorTemperature(Temperature));
            CurrentWarm.newTemperature(Temperature);
        }
    }

    IEnumerator DarkenCoroutine()
    {
        Color startColor = element.colorSustance;
        Color targetColor = startColor * 0.6f; // más bajo = más oscuro

        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            sustance1.color = Color.Lerp(startColor, targetColor, lerp);
            sustance2.color = Color.Lerp(startColor, targetColor, lerp);
            yield return null;
        }
        isDamaged=true;
         title.text= "Dañado";
        sustance1.color = targetColor;
        sustance2.color = targetColor;
    }


    Color GetColorTemperature(float temperature)
    {
        if (temperature < element.minTemperature)
            return Color.yellow;

        if (temperature < element.maxTemperature)
            return Color.green;

        if (temperature < 100f)
            return Color.red;

        return Color.black;
    }

    public bool IsFill() => element != null;
    public void Filling(Element newElement)
    {
        sustance1.color=newElement.colorSustance;
        sustance2.color=newElement.colorSustance;
        anim.Play("Filling");
        StartCoroutine(BlockMove(fillingClip.length));
        element = newElement;
        title.text= newElement.Name;
    }
    IEnumerator BlockMove(float amountTime)
    {
        GetComponent<CapsuleCollider2D>().enabled=false;
        yield return new WaitForSeconds(amountTime);
        GetComponent<CapsuleCollider2D>().enabled=true;

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
        if(currentDropZone)currentDropZone.setOccupied(true);
        if(CurrentWarm){
            onWarm=true;
        }
        transform.position=originPos;
    }
    bool IsCorrectWarm()
    {
        if (CurrentWarm)
        {
           if (CurrentWarm.getSwitch())
            {
                if(element.isMechero){
                    return true;
                } 
                else{
                    return false;
                }

            }
            else
            {
                if(element.isBañoMaria){
                    return true;
                } 
                else{
                    return false;
                }
            }
        }
        else return false;
    }
    void StartWarm(WarmManager warm)
    {
        if(IsFill()){
            Debug.Log("[Elements] si esta lleno ");
            CurrentWarm = warm;
            onWarm=true;
            if (!IsCorrectWarm())
            {
                inWrongMethod=true;
                score.RemovePoints("Spam");
                gamePlay.CreateFeedback(transform.position,"Metodo incorrecto");
            }
        }
        else
        {
            score.RemovePoints("Spam");
            gamePlay.CreateFeedback(transform.position,"Tubo vacío");
        }
 
    }
    void DropPosition(DropZone drop)
    {
        originPos=drop.getPosition();
        drop.setOccupied(true);
        dropzone.setOccupied(true);
        currentDropZone=drop; 
    }
    void MoveWarm()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, ObjectMask);
        if(hit != null )
        { 
            if(hit.TryGetComponent(out DropZone drop)){
                if(!drop.IsOccupied()){
                    if(hit.TryGetComponent(out WarmManager warm)){
                        DropPosition(drop);
                        StartWarm(warm);
                    }
                }
                else
                {
                    score.RemovePoints("Spam");
                    gamePlay.CreateFeedback(transform.position,"Ocupado");
                }
            }
        }
    }
    void MoveZone()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, layerMask);

        if (hit != null)
        {
            currentDropZone=null;
            Debug.Log("[Elements] zone " + hit.transform.name);
            CurrentWarm=null;
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
                    if (IsFill())
                    {
                        currentDropZone=null;
                        CurrentWarm=null;
                        originPos=drop.getPosition();
                        FinishElement();
                    }
                    else
                    {
                        score.RemovePoints("Spam");
                        gamePlay.CreateFeedback(transform.position,"Tubo vacío");
                    }
                }
             }
        }
    } 
    void FinishElement()
    {
        anim.Play("VanishTube");
        if (inWrongMethod)
        {
            gamePlay.NoPerfect();
        }
        StartCoroutine(BlockMove(VanishClip.length));
        if (isDamaged)
        {
            gamePlay.NoPerfect();
            score.AddPoints("DamageElement");
            gamePlay.CreateFeedback(transform.position,"Compuesto dañado");
        }
        else if (Temperature == 0)
        {
            gamePlay.NoPerfect();
            score.AddPoints("NoWarm");
            gamePlay.CreateFeedback(transform.position,"No calentado");
        }
        else if (Temperature < element.minTemperature || Temperature > element.maxTemperature)
        {
            gamePlay.NoPerfect();
            score.AddPoints("BadTemperature");
            gamePlay.CreateFeedback(transform.position,"Temperatura incorrecta");
        }
        else if (Temperature > element.minTemperature || Temperature < element.maxTemperature)
        {
            score.AddPoints("FinishElement");
            gamePlay.CorrectFeedback(transform.position);
        }
        gamePlay.addFinishElement();
        Destroy(gameObject,2f);
        
    }

}
