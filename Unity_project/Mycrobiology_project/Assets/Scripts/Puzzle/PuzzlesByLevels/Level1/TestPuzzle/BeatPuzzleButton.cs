using UnityEngine;

public class BeatPuzzleButton : MonoBehaviour
{

    public void GanarPuzzle()
    {
        FindObjectsByType<PuzzleManager>(FindObjectsSortMode.None)[0].CompletePuzzle();
    }
}
