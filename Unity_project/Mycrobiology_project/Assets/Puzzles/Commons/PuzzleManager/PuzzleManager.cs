using TMPro;
using UnityEngine;

public class PuzzleManager : MonoBehaviour, ITappable, IPuzzleManager, IPuzzlePausable
{


    [Header("PuzzleManager")]
    [SerializeField]
    private PuzzleHalo halo;

    public bool isVictoryAchieved = false;

    [Header("PuzzleGameplay")]
    public AbstractPuzzleGameplay _gameplay;

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

    [SerializeField] TextMeshProUGUI PerformanceResultTx;
    private bool puzzleAlreadyCompleted = false;
    public void Start()
    {
        _gameManager = GameManager.GetInstance();
        _gameManager.SubscribePuzzlePausable(this);
        PerformanceResultTx.text="";
    }


    public void OnTap()
    {
        if (!halo.isPlayerClose) return;

        PrepareScene();

        Debug.Log($"<color=green>PuzzleManager:</color> Vamos a instanciar el puzzleGameplayPrefab con padre {this.transform.gameObject.name}");
        SpawnGameplay();
        if (_gameplay == null)
        {
            Debug.LogWarning("<color=green>PuzzleManager:</color> No se encontró el componente AbstractPuzzleGameplay dentro de PuzzleGameplay");
            return;
        }

        StartPuzzle();

    }

    private void PrepareScene()
    {
        _gameManager.SwitchIsPuzzleActive();
        _gameManager.PuzzlePauseAll();
    }

    private void SpawnGameplay()
    {
        Instantiate(puzzleGameplayPrefab, this.transform)
                .TryGetComponent<AbstractPuzzleGameplay>(out AbstractPuzzleGameplay puzzelGameplayOut);


        Debug.Log("<color=green>PuzzleManager:</color> Vamos a asignar el componente PuzzleGamplay a la variable gameplay");
        _gameplay = puzzelGameplayOut;

        Debug.Log($"<color=green>PuzzleManager:</color> El PuzzleGameplay fue instanciado en posicion: {_gameplay.transform.position} y posicion local: {_gameplay.transform.localPosition}");
    }
    public void StartPuzzle()
    {
        Debug.Log($"<color=green>PuzzleManager:</color> Entrando en StartPuzzle. El valor de _gameplay es: {_gameplay}");
        if (_gameplay.IsFirstTime())
        {
            _gameplay.ShowTutorial();
        }
        else
        {
            _gameplay.StartGameplay();
        }
    }

    public void CompletePuzzle(bool didPlayerWin)
    {
        if (puzzleAlreadyCompleted) return;
        gameplay.puzzleEvaluation.FinishGame(didPlayerWin);
        PerformanceResultTx.text=gameplay.puzzleEvaluation.getPerformance();
        ColorUtility.TryParseHtmlString(gameplay.puzzleEvaluation.GetColor(), out Color c);
        PerformanceResultTx.color = c;
        puzzleAlreadyCompleted = true;

        isVictoryAchieved = didPlayerWin;

        Debug.Log("Puzzle completed!");

        if (isVictoryAchieved) ExecuteVictoryLogic(); else ExecuteDefeatLogic();

        _playerDamageDealer.CalculateDamage(GetScore());
        if (_playerDamageDealer.CanApplyDamage()) _playerDamageDealer.DealDamage(_playerHealthLogic);

        Destroy(halo.gameObject);

        _gameManager.SwitchIsPuzzleActive();
        _gameManager.PuzzleResumeAll();
    }

    public void ExecuteVictoryLogic()
    {
        Debug.Log("<color=green>PuzzleManager:</color> Felicidades, ganaste el puzzle!");
        _gameplay.Victory();
    }

    public void ExecuteDefeatLogic()
    {
        Debug.Log("<color=green>PuzzleManager:</color> Lo siento, perdiste el puzzle.");

        _gameplay.Defeat();
    }

    public int GetScore()
    {
        int currentGameplayScore = _gameplay.GetPuzzleEvaluation().GetCurrentScore();
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
        this.GetComponent<Collider2D>().enabled = true;
    }


    public void OnDestroy()
    {
        _gameManager.UnsubscribePuzzlePausable(this);
    }
}