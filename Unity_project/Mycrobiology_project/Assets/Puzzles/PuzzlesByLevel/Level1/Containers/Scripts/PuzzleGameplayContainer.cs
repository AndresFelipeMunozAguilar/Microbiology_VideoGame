using System.Collections.Generic;
using UnityEngine;

public class PuzzleGameplayContainer : AbstractPuzzleGameplay
{
    [SerializeField] List<disposal> objects = new List<disposal>();
    [SerializeField] List<Transform> positions = new List<Transform>();
    [SerializeField] GameObject BlankObject;
    PuzzleEvaluation score;
    int Amount_Complete;
    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public override void Defeat()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnStartGameplay()
    {
        Debug.Log("[Containers] empezamos gameplay");
        transform.GetChild(0).gameObject.SetActive(true);
        score = GetComponent<PuzzleEvaluation>();
        SpawnObjects();
    }

    public override void Victory()
    {
        PlayerHealthLogic _playerHealthLogic = FindAnyObjectByType<PlayerHealthLogic>();
        GetComponentInParent<PlayerDamageDealer>().CalculateDamage(score.GetCurrentScore());
        NotifyPuzzleVictory(true);
        bool Finish = score.GetCurrentScore() <= 60 ? false : true;
        score.FinishGame(Finish);
        Destroy(gameObject);
    }

    void SpawnObjects()
    {
        List<disposal> SelectDiposals = objects;
        for (int i = 0; i < positions.Count; i++)
        {
            int pos = Random.Range(0, SelectDiposals.Count);
            GameObject newElement = Instantiate(BlankObject, positions[i]);
            newElement.GetComponent<ObjectManager>().CreateDiposal(SelectDiposals[pos], score, this, Mathf.RoundToInt((100 / positions.Count)));
            SelectDiposals.RemoveAt(pos);
        }
    }
    public void CompleteObject()
    {
        Amount_Complete++;
        if (Amount_Complete >= positions.Count)
        {
            Victory();
        }
    }

}
