using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class AbstractPuzzleGameplay : MonoBehaviour
{
    [Header("Infraestructura de datos")]
    [Tooltip("Gestor de persistencia y carga de datos.")]
    [SerializeField] protected DataManager dataManager;

    [Tooltip("Sistema encargado de registrar y calificar el desempeño del jugador.")]
    [SerializeField] public PuzzleEvaluation puzzleEvaluation;

    [Header("Contenido Y Escena")]
    [Tooltip("Lista de elementos dinámicos que se instanciarán al iniciar.")]
    [SerializeField] protected List<SpawnableElement> elementsToSpawn;

    [Tooltip("Objeto visual que servirá como fondo del puzzle.")]
    [SerializeField] protected GameObject background;

    [Header("Tutorial")]
    [Tooltip("Estado que define si el jugador verá la guía inicial.")]
    [SerializeField] protected bool isFirstTimePlaying;

    [Tooltip("Prefab que contiene la interfaz o guía del tutorial.")]
    [SerializeField] protected GameObject tutorialPrefab;
    [SerializeField] protected TextMeshProUGUI Title;
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

    // Instancia todos los elementos definidos como hijos de este objeto.
    protected void SpawnElementsRelativeTo(Transform reference)
    {
        if (elementsToSpawn == null || elementsToSpawn.Count == 0) return;
        if (reference == null)
        {
            Debug.LogError($"<color=red>{name}:</color> No se puede spawnear, el objeto de referencia es nulo.");
            return;
        }

        foreach (SpawnableElement element in elementsToSpawn)
        {
            if (element.prefab == null) continue;

            // 1. Tomamos la posición 'Global' definida en el Scriptable/Lista como un OFFSET.
            // 2. Calculamos el punto de destino en el mundo: Centro del Referente + Desplazamiento deseado.
            Vector3 targetWorldPosition = reference.position + element.globalPosition;

            // Aseguramos que la Z sea consistente para 2D (usualmente la del Puzzle o 0)
            targetWorldPosition.z = transform.position.z;

            // 3. Convertimos esa posición de mundo al espacio local de este AbstractPuzzleGameplay.
            // Esto permite que el objeto sea hijo de 'transform' pero esté visualmente sobre el referente.
            Vector3 finalLocalPos = transform.InverseTransformPoint(targetWorldPosition);

            // Instanciación limpia
            GameObject instance = Instantiate(element.prefab, transform);
            instance.transform.localPosition = finalLocalPos;
            instance.transform.localRotation = element.localRotation;

            Debug.Log($"<color=cyan>{name}:</color> {element.name} instanciado a {element.globalPosition} de {reference.name}");
        }
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