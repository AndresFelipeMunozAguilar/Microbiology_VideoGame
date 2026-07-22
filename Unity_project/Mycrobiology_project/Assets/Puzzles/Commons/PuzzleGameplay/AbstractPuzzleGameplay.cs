using TMPro;
using UnityEngine;

[RequireComponent(typeof(PuzzleEvaluation))]
public abstract class AbstractPuzzleGameplay : MonoBehaviour
{
    [Header("Infraestructura de datos")]
    [Tooltip("Gestor de persistencia y carga de datos.")]
    [SerializeField] protected DataManager _dataManager;

    [Tooltip("Sistema encargado de registrar y calificar el desempeño del jugador.")]
    [SerializeField] public PuzzleEvaluation _puzzleEvaluation;

    [Header("Contenido Y Escena")]
    [Tooltip("Objeto visual que servirá como fondo del puzzle.")]
    [SerializeField] protected GameObject _background;

    [Tooltip("Define qué tan alejado del centro de la cámara se instanciará el puzzle.")]
    [SerializeField] protected Vector3 _spawnOffset = new Vector3(0f, 0f, 0f);

    [Header("Tutorial")]
    [Tooltip("Estado que define si el jugador verá la guía inicial.")]
    [SerializeField] protected bool _isFirstTimePlaying;

    [Tooltip("Prefab que contiene la interfaz o guía del tutorial.")]
    [SerializeField] protected GameObject _tutorialPrefab;

    [SerializeField] protected TextMeshProUGUI _title;

    // Este metodo inicializa las referencias base del puzzle.
    private void Awake()
    {
        _dataManager = DataManager.Instance;

        Debug.Log($"<color=magenta>{GetType().Name}:</color> La instancia de DataManager (en start) fue encontrada?: {(_dataManager != null)}");

        OnInstanceAwake();
    }

    // Este metodo inicia el minijuego despues del tutorial o al tocar el puzzle.
    public void StartGameplay()
    {
        Vector3 inFrontOfCamera = Camera.main.transform.position;
        inFrontOfCamera.z = 0f;

        Vector3 spawnPosition = inFrontOfCamera + _spawnOffset;

        Debug.Log($"<color=yellow>{GetType().Name}:</color> Vamos a instanciar el fondo y los objetos");

        FollowCenter followCenter = GetComponent<FollowCenter>();

        if (followCenter != null)
        {
            followCenter.SetOffset(_spawnOffset);
        }
        else
        {
            SetGlobalPositionTo(spawnPosition);
        }

        SpawnBackground(transform.position, Quaternion.identity, transform);
        ActivateSonObjects();

        OnStartGameplay();
    }

    // Este metodo ubica el puzzle en una posicion global conservando su profundidad.
    protected void SetGlobalPositionTo(Vector3 worldPosition)
    {
        Vector3 targetWorldPosition = worldPosition;

        targetWorldPosition.z = transform.position.z;

        transform.position = targetWorldPosition;
    }

    // Este metodo activa los objetos hijos del puzzle al comenzar.
    protected void ActivateSonObjects()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    // Este metodo instancia el fondo visual del puzzle.
    public void SpawnBackground(Vector3 position, Quaternion rotation, Transform parent)
    {
        Instantiate(_background, position, rotation, parent);
    }

    public abstract void Victory();

    public abstract void Defeat();

    // Este metodo muestra el tutorial contextual del puzzle.
    public void ShowTutorial()
    {
        Debug.Log("Showing Puzzle Tutorial");
        Instantiate(_tutorialPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
    }

    // Este metodo permite saber si el puzzle se juega por primera vez.
    public bool IsFirstTime()
    {
        Debug.Log($"<color=magenta>AbstractPuzzleGameplay:</color> Entramos en IsFirstTime. El _dataManager es null?: {_dataManager == null}");

        if (_dataManager == null) return false;

        _isFirstTimePlaying = !_dataManager
                                .HasPuzzleBeenPlayed(GetComponent<PuzzleEvaluation>().puzzleID);

        Debug.Log($"<color=red>AbstractPuzzleGameplay:</color> Is the first time playing the puzzle '{GetComponent<PuzzleEvaluation>().puzzleID}'? {_isFirstTimePlaying}");
        return _isFirstTimePlaying;
    }

    // Este metodo permite devolver la evaluacion asociada al puzzle.
    public PuzzleEvaluation GetPuzzleEvaluation()
    {
        return _puzzleEvaluation;
    }

    // Este metodo informa al PuzzleManager que el puzzle ya termino.
    protected void NotifyPuzzleVictory(bool didPlayerWin)
    {
        IPuzzleManager puzzleManager = GetComponentInParent<IPuzzleManager>();

        if (puzzleManager == null)
        {
            Debug.LogWarning("No se encontro el Componente PuzzleManager");
            return;
        }

        puzzleManager.CompletePuzzle(didPlayerWin);
    }

    // Este metodo permite extender Awake desde las clases hijas.
    protected virtual void OnInstanceAwake() { }

    // Este metodo permite ejecutar logica adicional al iniciar el gameplay.
    protected virtual void OnStartGameplay() { }
}
