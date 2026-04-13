using System.Collections.Generic;
using UnityEngine;

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
    public abstract void StartGameplay();

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
        Debug.Log($"Is the first time playing the puzzle? {isFirstTimePlaying}");
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
}