using UnityEngine;

public class PuzzleManager : MonoBehaviour, ITappable, IPuzzleManager, IPuzzlePausable
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

    public void PuzzlePauseMe()
    {
        Debug.Log("Puzzle paused.");
    }

    public void PuzzleResumeMe()
    {
        Debug.Log("Puzzle resumed.");
    }

    public void OnEnable()
    {
        GameManager.GetInstance().SubscribePuzzlePausable(this);
    }

    public void OnDisable()
    {
        GameManager.GetInstance().UnsubscribePuzzlePausable(this);
    }
}