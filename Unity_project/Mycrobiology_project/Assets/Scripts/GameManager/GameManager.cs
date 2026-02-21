using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    // WARNING: Este enum debe coincidir con 
    // los nombres y orden de las escenas en 
    // Build Settings, o el sistema de cambio 
    // de escenas no funcionará.
    public enum GameScenes
    {
        DevAndres,
        DevBrandon,
        Develop,
        TempGameOver,
    }

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

        LoadScene(GameScenes.TempGameOver);
    }

    public void LoadScene(GameScenes scene)
    {
        SceneManager.LoadScene((int)scene);
    }

}
