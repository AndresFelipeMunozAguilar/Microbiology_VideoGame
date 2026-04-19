using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PuzzleEvaluation))]
public abstract class AbstractPuzzleGameplay : MonoBehaviour
{
    [Header("Infraestructura de datos")]
    [Tooltip("Gestor de persistencia y carga de datos.")]
    [SerializeField] protected DataManager dataManager;

    [Tooltip("Sistema encargado de registrar y calificar el desempeño del jugador.")]
    [SerializeField] protected PuzzleEvaluation puzzleEvaluation;

    [Header("Contenido Y Escena")]
    [Tooltip("Objeto visual que servirá como fondo del puzzle.")]
    [SerializeField] protected GameObject background;

    [Header("Tutorial")]
    [Tooltip("Estado que define si el jugador verá la guía inicial.")]
    [SerializeField] protected bool isFirstTimePlaying;

    [Tooltip("Prefab que contiene la interfaz o guía del tutorial.")]
    [SerializeField] protected GameObject tutorialPrefab;

    protected void Awake()
    {
        dataManager = DataManager.Instance;

        if (dataManager == null)
        {
            Debug.LogError($"<color=magenta>{this.GetType().Name}:</color> No se encontró una instancia de DataManager (en start) en la escena. Asegúrate de que exista un GameObject con el componente DataManager.");
        }
        else
        {
            Debug.Log($"<color=magenta>{this.GetType().Name}:</color> Instancia de DataManager encontrada en awake.");
        }

        OnInstanceAwake();
    }

    public abstract void StartGameplay();

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
        Instantiate(background, position, rotation, parent);
    }

    public abstract void Victory();

    public abstract void Defeat();

    public void ShowTutorial()
    {
        Debug.Log("Showing Puzzle Tutorial");
        Instantiate(tutorialPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
    }

    public bool IsFirstTime()
    {
        Debug.Log($"<color=magenta>AbstractPuzzleGameplay:</color> Entramos en IsFirstTime. El DataManager es null?: {dataManager == null}");
        if (dataManager == null) return false;

        isFirstTimePlaying = !dataManager
                                .HasPuzzleBeenPlayed(GetComponent<PuzzleEvaluation>().puzzleID);

        Debug.Log($"<color=red>AbstractPuzzleGameplay:</color> Is the first time playing the puzzle '{GetComponent<PuzzleEvaluation>().puzzleID}'? {!isFirstTimePlaying}");
        return isFirstTimePlaying;
    }

    public PuzzleEvaluation GetPuzzleEvaluation()
    {
        return puzzleEvaluation;
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

    // Hook para ser usado por las clases, de requerir usar el método Start() para inicializar campos o propiedades
    protected virtual void OnInstanceAwake() { }
}