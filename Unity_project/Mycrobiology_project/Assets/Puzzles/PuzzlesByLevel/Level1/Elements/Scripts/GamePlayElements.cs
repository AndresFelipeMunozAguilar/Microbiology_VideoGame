using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayElements : AbstractPuzzleGameplay
{
    [SerializeField] List<Element> elements = new List<Element>();
    [SerializeField] List<Transform> positions = new List<Transform>();
    [SerializeField] List<Transform> positionsTubes = new List<Transform>();
    [SerializeField] GameObject blankElement, Tube, FeedbackElement, CorrectElement;
    int elementsFinished = 0;
    PuzzleEvaluation score;
    bool IsPerfect = true;
    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);

    }
    public void NoPerfect() { IsPerfect = false; }
    void SelectElements()
    {
        List<Element> selectElements = new List<Element>(elements);
        for (int i = 0; i < positions.Count; i++)
        {
            int pos = Random.Range(0, selectElements.Count);
            GameObject newElement = Instantiate(blankElement, positions[i]);
            newElement.GetComponent<ElementManager>().CreateElement(selectElements[pos], score, this);
            selectElements.RemoveAt(pos);
        }
        for (int i = 0; i < positionsTubes.Count; i++)
        {
            GameObject newElement = Instantiate(Tube, positionsTubes[i]);
            newElement.GetComponent<TubeManager>().CreateElement(score, this);

        }
    }
    public void CreateFeedback(Vector2 posSpawn, string message)
    {
        GameObject feedback = Instantiate(FeedbackElement, posSpawn, Quaternion.identity, transform);
        feedback.GetComponentInChildren<TextMeshProUGUI>().text = message;
        Destroy(feedback, 5f);
    }
    public void CorrectFeedback(Vector2 posSpawn)
    {
        GameObject feedback = Instantiate(CorrectElement, posSpawn, Quaternion.identity, transform);
        Destroy(feedback, 5f);
    }
    public void addFinishElement()
    {
        elementsFinished++;
        Debug.Log("[ELEMENT] cuenta:" + elementsFinished + " : " + positions.Count);
        if (elementsFinished >= positions.Count)
        {
            GetComponentInParent<PlayerDamageDealer>().CalculateDamage(score.GetCurrentScore());
            bool Finish = score.GetCurrentScore() <= 60 ? false : true;
            score.FinishGame(Finish);
            if (Finish)
            {
                if (IsPerfect) score.AddPoints("BonusPerfect");
                Victory();
            }
            else
            {
                Defeat();
            }

        }
    }

    protected override void OnStartGameplay()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        score = GetComponent<PuzzleEvaluation>();
        if (_title) _title.text = score.puzzleID.ToString();
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
