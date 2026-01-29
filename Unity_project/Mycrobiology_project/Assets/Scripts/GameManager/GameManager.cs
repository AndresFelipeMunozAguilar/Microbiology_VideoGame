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
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        puzzlePausables = new List<IPuzzlePausable>();

    }

    // Evitar la instanciación externa
    private GameManager() { }

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }

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
    }

}
