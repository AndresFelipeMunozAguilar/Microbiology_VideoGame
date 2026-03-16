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

    [Header("Player Damage and Health Logic")]
    [SerializeField]
    private PlayerHealthLogic _playerHealthLogic;

    [SerializeField]
    private PlayerDamageDealer _playerDamageDealer;

    [Header("Others")]
    [SerializeField]
    private PerformanceResult performanceResult;

    [SerializeField]
    private GameManager _gameManager;




    public void Start()
    {
        _gameManager = GameManager.GetInstance();
        _gameManager.SubscribePuzzlePausable(this);
    }


    public void OnTap()
    {
        if (halo.isPlayerClose)
        {
            _gameManager.SwitchIsPuzzleActive();
            _gameManager.PuzzlePauseAll();

            Debug.Log("<color=green>PuzzleManager:</color> Vamos a instanciar el puzzleGameplayPrefab con padre puzzlegameplay");

            Instantiate(puzzleGameplayPrefab, this.transform)
                .TryGetComponent<AbstractPuzzleGameplay>(out AbstractPuzzleGameplay puzzelGameplayOut);

            if (puzzelGameplayOut == null)
            {
                Debug.LogWarning("No se encontró el componente AbstractPuzzleGameplay dentro de PuzzleGameplay");
                return;
            }

            Debug.Log("<color=green>PuzzleManager:</color> Vamos a asignar el componente PuzzleGamplay a la variable gameplay");
            gameplay = puzzelGameplayOut;


            StartPuzzle();


        }

    }

    public void StartPuzzle()
    {
        if (gameplay.IsFirstTime())
        {
            gameplay.ShowTutorial();
        }
        else
        {
            gameplay.StartGameplay();
        }
    }

    public void CompletePuzzle()
    {
        Debug.Log("Puzzle completed!");

        if (isVictoryAchieved)
        {
            ExecuteVictoryLogic();
        }
        else
        {
            ExecuteDefeatLogic();
        }


        _gameManager.SwitchIsPuzzleActive();
        _gameManager.PuzzleResumeAll();
    }

    public void ExecuteVictoryLogic()
    {
        Debug.Log("<color=green>PuzzleManager:</color> Felicidades, ganaste el puzzle!");
        gameplay.Victory();
    }

    public void ExecuteDefeatLogic()
    {
        Debug.Log("<color=green>PuzzleManager:</color> Lo siento, perdiste el puzzle.");


        _playerDamageDealer.CalculateDamage(GetScore());
        _playerDamageDealer.DealDamage(_playerHealthLogic);

        gameplay.Defeat();
    }

    public int GetScore()
    {
        int currentGameplayScore = gameplay.GetPuzzleEvaluation().GetCurrentScore();
        Debug.Log($"<color=green>PuzzleManager:</color> El puntaje obtenido fue: {currentGameplayScore}");
        return currentGameplayScore;
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
        _gameManager.UnsubscribePuzzlePausable(this);
    }
}