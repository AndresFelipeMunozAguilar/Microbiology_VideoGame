using UnityEngine;

public class PuzzleManager : MonoBehaviour, ITappable, IPuzzleManager, IPuzzlePausable
{

    [SerializeField]
    private PuzzleHalo halo;

    [SerializeField]
    private PuzzleGameplay gameplay;

    [SerializeField]
    private PerformanceResult performanceResult;

    private GameManager gameManager;

    public bool isVictoryAchieved = false;

    public void OnTap()
    {
        if (halo.isPlayerClose)
        {
            gameManager.SwitchIsPuzzleActive();
            gameManager.PuzzlePauseAll();

            if (gameplay.IsFirstTime())
            {
                gameplay.ShowTutorial();
            }
            else
            {
                StartPuzzle();
            }


        }

    }

    public void Start()
    {
        gameManager = GameManager.GetInstance();
        gameManager.SubscribePuzzlePausable(this);
    }

    public void StartPuzzle()
    {
        gameplay.StartGameplay();
    }

    public void CompletePuzzle()
    {
        Debug.Log("Puzzle completed!");

        if (isVictoryAchieved)
        {
            Debug.Log("PuzzleManager: Felicidades, ganaste el puzzle!");
            gameplay.Victory();
        }
        else
        {
            Debug.Log("PuzzleManager: Lo siento, perdiste el puzzle.");
            gameplay.Defeat();
        }


        gameManager.SwitchIsPuzzleActive();
        gameManager.PuzzleResumeAll();
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
        Debug.Log("I am GAME MANAGER and i have been PAUSED.");
        this.GetComponent<Collider2D>().enabled = false;
    }

    public void PuzzleResumeMe()
    {
        Debug.Log("I am GAME MANAGER and i have been RESUMED.");
    }


    public void OnDestroy()
    {
        gameManager.UnsubscribePuzzlePausable(this);
    }
}