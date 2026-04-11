using System.Collections.Generic;
using UnityEngine;

public class PuzzleGameplayContainer : AbstractPuzzleGameplay
{
    [SerializeField] List<disposal> objects= new List<disposal>();
    [SerializeField] List<Transform> positions = new List<Transform>();
    PuzzleEvaluation score;
    public override void Defeat()
    {
        throw new System.NotImplementedException();
    }

    public override void StartGameplay()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        score=GetComponent<PuzzleEvaluation>();
    }

    public override void Victory()
    {
        throw new System.NotImplementedException();
    }

}
