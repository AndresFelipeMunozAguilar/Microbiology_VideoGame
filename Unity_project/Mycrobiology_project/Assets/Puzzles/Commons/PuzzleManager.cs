using UnityEngine;

public class PuzzleManager : MonoBehaviour, ITappable, IPuzzleManager, IPuzzlePausable
{


    [Header("PuzzleManager")]
    [SerializeField]
    private PuzzleHalo halo;

    public bool isVictoryAchieved = false;


    [Header("PuzzleGameplay")]
    public AbstractPuzzleGameplay gameplay;

    [SerializeField]
    private GameObject puzzleGameplayPrefab;

    [Header("Others")]
    [SerializeField]
    private PerformanceResult performanceResult;

    [SerializeField]
    private GameManager gameManager;




    public void Start()
    {
        gameManager = GameManager.GetInstance();
        gameManager.SubscribePuzzlePausable(this);
    }


    public void OnTap()
    {
        if (halo.isPlayerClose)
        {
            gameManager.SwitchIsPuzzleActive();
            gameManager.PuzzlePauseAll();

            Debug.Log("Puzzlemanager: Vamos a instanciar el puzzleGameplayPrefab con padre puzzlegameplay");

            Instantiate(puzzleGameplayPrefab, this.transform)
                .TryGetComponent<AbstractPuzzleGameplay>(out AbstractPuzzleGameplay puzzelGameplayOut);

            if (puzzelGameplayOut == null)
            {
                Debug.LogWarning("No se encontró el componente AbstractPuzzleGameplay dentro de PuzzleGameplay");
                return;
            }

            Debug.Log("Puzzlemanager: Vamos a asignar el componente PuzzleGamplay a la variable gameplay");
            gameplay = puzzelGameplayOut;


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
        Debug.Log("I am PuzzleMANAGER and my collider2d has been PAUSED.");
        this.GetComponent<Collider2D>().enabled = false;
    }

    public void PuzzleResumeMe()
    {
        Debug.Log("I am PuzzleMANAGER and i have been RESUMED without my collider2d.");
    }


    public void OnDestroy()
    {
        gameManager.UnsubscribePuzzlePausable(this);
    }
}