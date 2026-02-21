using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public bool isGameOver = false;

    public bool isPuzzleActive = false;

    private List<IPuzzlePausable> puzzlePausables;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        puzzlePausables = new List<IPuzzlePausable>();

    }


    // Evitar la instanciación externa
    private GameManager() { }

    public static GameManager GetInstance()
    {
        return instance;
    }

    public void SubscribePuzzlePausable(IPuzzlePausable puzzlePausable)
    {
        puzzlePausables.Add(puzzlePausable);
    }

    public void UnsubscribePuzzlePausable(IPuzzlePausable puzzlePausable)
    {
        puzzlePausables.Remove(puzzlePausable);
    }

    public void PuzzlePauseAll()
    {
        Debug.Log("Pausing all puzzles...");
        Debug.Log($"Puzzle is active: {isPuzzleActive}");

        foreach (IPuzzlePausable puzzlePausable in puzzlePausables)
        {
            puzzlePausable.PuzzlePauseMe();
        }
    }

    public void PuzzleResumeAll()
    {
        Debug.Log("Resuming all puzzles...");
        Debug.Log($"Puzzle is active: {isPuzzleActive}");

        foreach (IPuzzlePausable puzzlePausable in puzzlePausables)
        {
            puzzlePausable.PuzzleResumeMe();
        }
    }

    public void SwitchIsPuzzleActive()
    {
        isPuzzleActive = !isPuzzleActive;
        Debug.Log($"is the puzzle active?: {isPuzzleActive}");
    }

    public void OnGameOver(string reason)
    {
        Debug.Log($"Game Over! Reason: {reason}");
        isGameOver = true;
    }

}
