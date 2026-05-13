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

    private void Awake()
    {
        _dataManager = DataManager.Instance;

        Debug.Log($"<color=magenta>{this.GetType().Name}:</color> La instancia de DataManager (en start) fue encontrada?: {(_dataManager != null)}");


        OnInstanceAwake();
    }

    public void StartGameplay()
    {
        Vector3 inFrontOfCamera = Camera.main.transform.position;
        inFrontOfCamera.z = 0f;

        Vector3 spawnPosition = inFrontOfCamera + _spawnOffset;

        Debug.Log($"<color=yellow>{GetType().Name}:</color> Vamos a instanciar el fondo y los objetos");

        SetGlobalPositionTo(spawnPosition);

        SpawnBackground(spawnPosition, Quaternion.identity, transform);
        ActivateSonObjects();

        OnStartGameplay();
    }

    protected void SetGlobalPositionTo(Vector3 worldPosition)
    {
        Vector3 targetWorldPosition = worldPosition;

        targetWorldPosition.z = transform.position.z;

        transform.position = targetWorldPosition;
    }

    protected void ActivateSonObjects()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void SpawnBackground(Vector3 position, Quaternion rotation, Transform parent)
    {
        Instantiate(_background, position, rotation, parent);
    }

    public abstract void Victory();

    public abstract void Defeat();

    public void ShowTutorial()
    {
        Debug.Log("Showing Puzzle Tutorial");
        Instantiate(_tutorialPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
    }

    public bool IsFirstTime()
    {
        Debug.Log($"<color=magenta>AbstractPuzzleGameplay:</color> Entramos en IsFirstTime. El _dataManager es null?: {_dataManager == null}");
        if (_dataManager == null) return false;

        _isFirstTimePlaying = !_dataManager
                                .HasPuzzleBeenPlayed(GetComponent<PuzzleEvaluation>().puzzleID);

        Debug.Log($"<color=red>AbstractPuzzleGameplay:</color> Is the first time playing the puzzle '{GetComponent<PuzzleEvaluation>().puzzleID}'? {_isFirstTimePlaying}");
        return _isFirstTimePlaying;
    }

    public PuzzleEvaluation GetPuzzleEvaluation()
    {
        return _puzzleEvaluation;
    }

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

    // Hook para ser usado por las clases, de 
    // requerir usar el método Awake() para 
    // inicializar campos o propiedades
    protected virtual void OnInstanceAwake() { }

    // Hook que se ejecuta al final de StartGameplay
    protected virtual void OnStartGameplay() { }
}