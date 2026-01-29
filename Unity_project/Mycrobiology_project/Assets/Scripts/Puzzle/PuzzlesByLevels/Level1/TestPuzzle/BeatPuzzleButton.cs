using UnityEngine;

public class BeatPuzzleButton : MonoBehaviour
{

    public void GanarPuzzle()
    {
        PuzzleManager puzzleManager = FindObjectsByType<PuzzleManager>(FindObjectsSortMode.None)[0];
        puzzleManager.isVictoryAchieved = true;
        puzzleManager.CompletePuzzle();
    }

    public void PerferPuzzle()
    {
        PuzzleManager puzzleManager = FindObjectsByType<PuzzleManager>(FindObjectsSortMode.None)[0];
        puzzleManager.isVictoryAchieved = false;
        puzzleManager.CompletePuzzle();
    }
}
