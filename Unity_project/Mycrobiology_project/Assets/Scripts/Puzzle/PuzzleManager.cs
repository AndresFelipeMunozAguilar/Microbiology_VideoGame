using UnityEngine;

public class PuzzleManager : MonoBehaviour, ITappable, IPuzzleManager
{

    [SerializeField]
    private PuzzleHalo halo;

    [SerializeField]
    private PuzzleGameplay gameplay;

    [SerializeField]
    private PerformanceResult performanceResult;

    public void OnTap()
    {
        if (halo.isPlayerClose)
        {
            StartPuzzle();
        }

    }

    public void StartPuzzle()
    {
        gameplay.StartGameplay();
    }

    public void CompletePuzzle()
    {
        Debug.Log("Puzzle completed!");
    }

    public float GetScore()
    {
        Debug.Log($"Score is: 100");

        return 100f;
    }

    public PerformanceResult GetPerformanceResult()
    {
        return performanceResult;
    }
}