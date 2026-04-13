using System.Collections.Generic;
using UnityEngine;

public class GamePlayElements : AbstractPuzzleGameplay
{
    [SerializeField] List<Element> elements= new List<Element>();
    [SerializeField] List<Transform> positions = new List<Transform>();
    [SerializeField] GameObject blankElement;
    int elementsFinished=0;
    PuzzleEvaluation score;
    private void Start() {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    void SelectElements()
    {
        List<Element> SelectElements = elements;
        for(int i = 0; i < positions.Count; i++) {
            int pos = Random.Range(0,SelectElements.Count);
            GameObject newElement = Instantiate(blankElement,positions[i]);
            newElement.GetComponent<ElementManager>().CreateElement(SelectElements[pos],score,this);
            SelectElements.RemoveAt(pos);
        }
    }

    public void addFinishElement()
    {
        elementsFinished++;
        Debug.Log("[ELEMENT] cuenta:"+ elementsFinished +" : " + positions.Count);
        if(elementsFinished >= positions.Count)
        {
           
            if (score.GetCurrentScore()>=60)
            {   
                score.AddPoints("VictoryBonus");
                Victory();
                 Debug.Log("[ELEMENT] ganaste");
            }
            else
            {
                Defeat();
                 Debug.Log("[ELEMENT] perdiste");
            }
            Debug.Log("[ELEMENT] Score: "+score.GetCurrentScore());
            
        }
    }

    public override void StartGameplay()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        score=GetComponent<PuzzleEvaluation>();
        SelectElements();
    }

    public override void Victory()
    {
        NotifyPuzzleVictory(true);  
        Destroy(this.gameObject);
    }

    public override void Defeat()
    {
        NotifyPuzzleVictory(false);
        Destroy(this.gameObject);
    }
}
