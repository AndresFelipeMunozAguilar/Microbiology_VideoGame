using UnityEngine;

public class PuzzleDefeat : MonoBehaviour
{

    public void PerderPuzzle()
    {
        PuzzleManager puzzleManager = FindObjectsByType<PuzzleManager>(FindObjectsSortMode.None)[0];
        puzzleManager.isVictoryAchieved = false;
        puzzleManager.CompletePuzzle();
    }
}
