using TMPro;
using UnityEngine;

public class PuzzleManager : MonoBehaviour, ITappable, IPuzzleManager, IPuzzlePausable
{
    [Header("Puzzle Manager")]
    [SerializeField]
    private PuzzleHalo halo;

    public bool isVictoryAchieved = false;

    [Header("Puzzle Gameplay")]
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

    [SerializeField]
    private TextMeshProUGUI PerformanceResultTx;

    private bool puzzleAlreadyCompleted = false;

    // Este metodo suscribe el puzzle al sistema general de pausa.
    public void Start()
    {
        _gameManager = GameManager.GetInstance();
        _gameManager.SubscribePuzzlePausable(this);
        PerformanceResultTx.text = "";
    }

    // Este metodo responde al toque del jugador sobre el puzzle.
    public void OnTap()
    {
        if (!halo.isPlayerClose) return;

        PrepareScene();

        Debug.Log($"<color=green>PuzzleManager:</color> Vamos a instanciar el puzzleGameplayPrefab con padre {transform.gameObject.name}");
        SpawnGameplay();
        if (_gameplay == null)
        {
            Debug.LogWarning("<color=green>PuzzleManager:</color> No se encontró el componente AbstractPuzzleGameplay dentro de PuzzleGameplay");
            return;
        }

        StartPuzzle();
    }

    // Este metodo bloquea la escena antes de abrir el minijuego.
    private void PrepareScene()
    {
        _gameManager.SwitchIsPuzzleActive();
        _gameManager.PuzzlePauseAll();
    }

    // Este metodo crea la instancia jugable del puzzle seleccionado.
    private void SpawnGameplay()
    {
        Instantiate(puzzleGameplayPrefab, transform)
                .TryGetComponent<AbstractPuzzleGameplay>(out AbstractPuzzleGameplay puzzleGameplayOut);

        Debug.Log("<color=green>PuzzleManager:</color> Vamos a asignar el componente PuzzleGamplay a la variable gameplay");
        _gameplay = puzzleGameplayOut;

        Debug.Log($"<color=green>PuzzleManager:</color> El PuzzleGameplay fue instanciado en posicion: {_gameplay.transform.position} y posicion local: {_gameplay.transform.localPosition}");
    }

    // Este metodo decide si se muestra el tutorial o se inicia directo el juego.
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

    // Este metodo registra el cierre del puzzle y reanuda la exploracion.
    public void CompletePuzzle(bool didPlayerWin)
    {
        if (puzzleAlreadyCompleted) return;

        _gameplay._puzzleEvaluation.FinishGame(didPlayerWin);
        PerformanceResultTx.text = _gameplay._puzzleEvaluation.getPerformance();
        ColorUtility.TryParseHtmlString(_gameplay._puzzleEvaluation.GetColor(), out Color c);
        PerformanceResultTx.color = c;
        puzzleAlreadyCompleted = true;

        isVictoryAchieved = didPlayerWin;

        Debug.Log("Puzzle completed!");

        if (isVictoryAchieved)
        {
            ExecuteVictoryLogic();
        }
        else
        {
            ExecuteDefeatLogic();
        }

        _playerDamageDealer.CalculateDamage(GetScore());

        if (_playerDamageDealer.CanApplyDamage())
        {
            _playerDamageDealer.DealDamage(_playerHealthLogic);
        }

        Destroy(halo.gameObject);

        _gameManager.SwitchIsPuzzleActive();
        _gameManager.PuzzleResumeAll();
    }

    // Este metodo ejecuta las acciones visuales y de estado cuando el puzzle se gana.
    public void ExecuteVictoryLogic()
    {
        Debug.Log("<color=green>PuzzleManager:</color> Felicidades, ganaste el puzzle!");
        _gameplay.Victory();
    }

    // Este metodo ejecuta las acciones visuales y de estado cuando el puzzle se pierde.
    public void ExecuteDefeatLogic()
    {
        Debug.Log("<color=green>PuzzleManager:</color> Lo siento, perdiste el puzzle.");

        _gameplay.Defeat();
    }

    // Este metodo permite devolver el puntaje actual del minijuego.
    public int GetScore()
    {
        int currentGameplayScore = _gameplay.GetPuzzleEvaluation().GetCurrentScore();
        Debug.Log($"<color=green>PuzzleManager:</color> El puntaje obtenido fue: {currentGameplayScore}");
        return currentGameplayScore;
    }

    // Este metodo permite devolver el resultado de rendimiento configurado.
    public PerformanceResult GetPerformanceResult()
    {
        return performanceResult;
    }

    // Este metodo desactiva el collider del puzzle mientras otro puzzle esta activo.
    public void PuzzlePauseMe()
    {
        Debug.Log("I am PuzzleMANAGER and my collider2d has been PAUSED.");
        GetComponent<Collider2D>().enabled = false;
    }

    // Este metodo vuelve a activar el collider del puzzle.
    public void PuzzleResumeMe()
    {
        Debug.Log("I am PuzzleMANAGER and i have been RESUMED without my collider2d.");
        GetComponent<Collider2D>().enabled = true;
    }

    // Este metodo retira la suscripcion cuando el objeto se destruye.
    public void OnDestroy()
    {
        _gameManager.UnsubscribePuzzlePausable(this);
    }
}
