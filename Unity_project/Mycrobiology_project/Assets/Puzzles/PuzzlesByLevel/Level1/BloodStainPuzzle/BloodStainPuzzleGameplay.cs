using System;
using UnityEngine;
using UnityEngine.Events;

public class BloodStainPuzzleGameplay : AbstractPuzzleGameplay
{
    [Header("Configuración de Datos")]
    [SerializeField] private PuzzleSequenceSO _puzzleSequence;
    [SerializeField] private PuzzleVisuals _visuals;
    [SerializeField] private DropZone _dropZone;

    [Header("Estado del Juego (Solo Lectura)")]
    [SerializeField] private int _currentStepIndex = 0;
    [SerializeField] private int _errorCount = 0;

    [Header("Eventos de Salida")]
    public Action OnPuzzleWin;
    public Action OnPuzzleLost;
    public Action<int, int> OnErrorChanged; // (errores actuales, max errores)

    public void Start()
    {
        if (_puzzleSequence == null)
        {
            Debug.LogError($"<color=yellow>BloodStainPuzzleGameplay:</color> Falta PuzzleSequenceSO en {gameObject.name}");
            return;
        }

        Debug.Log("<color=yellow>BloodStainPuzzleGameplay:</color> Iniciado. Esperando primer paso.");

    }

    public override void StartGameplay()
    {
        Vector3 inFrontOfCamera = Camera.main.transform.position;
        inFrontOfCamera.z = 0f;

        Debug.Log("<color=yellow>BloodStainPuzzleGameplay:</color>: Vamos a instanciar el fondo y los objetos");

        SpawnBackground(inFrontOfCamera, Quaternion.identity, transform);
        SpawnElements();

        _visuals = GetComponentInChildren<PuzzleVisuals>();
        if (_visuals == null)
        {
            Debug.LogError($"<color=yellow>BloodStainPuzzleGameplay:</color> No se encontró el componente PuzzleVisuals en los hijos de {this.gameObject.name}");
            return;
        }

        _dropZone = GetComponentInChildren<DropZone>();
        if (_dropZone == null)
        {
            Debug.LogError($"<color=yellow>BloodStainPuzzleGameplay:</color> No se encontró el componente DropZone en los hijos de {this.gameObject.name}");
            return;
        }
        _dropZone.OnDraggableItemDropped += ProcessItemInteraction;

    }

    public void OnDestroy()
    {
        _dropZone.OnDraggableItemDropped -= ProcessItemInteraction;
    }



    // Método principal que procesa la lógica de comparación.
    // Se suscribe al evento OnItemDropped del DropZone.
    public void ProcessItemInteraction(string droppedItemId)
    {

        CleaningStepSO expectedStep = _puzzleSequence.Steps[_currentStepIndex];

        if (droppedItemId == expectedStep.RequiredItemId)
        {
            HandleCorrectStep(expectedStep);
        }
        else
        {
            HandleIncorrectStep();
        }
    }

    private void HandleCorrectStep(CleaningStepSO step)
    {
        _currentStepIndex++;
        Debug.Log($"<color=yellow>BloodStainPuzzleGameplay:</color> Paso Correcto: {_currentStepIndex}/{_puzzleSequence.TotalSteps}");

        // Notificar a los visuales para cambiar el sprite de la mancha
        _visuals.UpdateVisuals(step.StepResultSprite);

        if (_currentStepIndex >= _puzzleSequence.TotalSteps)
        {
            _visuals.UpdateVisuals(_puzzleSequence.FinalCleanSprite);

            Debug.Log("<color=yellow>BloodStainPuzzleGameplay:</color> ¡Puzzle Completado!");
            OnPuzzleWin?.Invoke();

            NotifyPuzzleVictory(true);
        }
    }

    private void HandleIncorrectStep()
    {
        _errorCount++;
        Debug.Log($"<color=red>Error detectado:</color> {_errorCount}/{_puzzleSequence.MaxAllowedErrors}");

        _visuals.ShowErrorEffect();
        OnErrorChanged?.Invoke(_errorCount, _puzzleSequence.MaxAllowedErrors);

        if (_errorCount >= _puzzleSequence.MaxAllowedErrors)
        {
            _visuals.DisableStain();

            Debug.Log("<color=yellow>BloodStainPuzzleGameplay:</color> Puzzle Fallido por exceso de errores");
            OnPuzzleLost?.Invoke();

            NotifyPuzzleVictory(false);
        }
    }


    public override void Victory()
    {
        Debug.Log("PuzzleGamelay: You won the Puzzle: Victory!");
        puzzleEvaluation.AddPoints("ganar");
        //Falta añadir la lógica de calcular la performance en el puzzle
        puzzleEvaluation.FinishGame(true);
        Destroy(this.gameObject);


    }

    public override void Defeat()
    {
        Debug.Log("PuzzleGamelay: You lost the Puzzle: Defeat!");
        puzzleEvaluation.RemovePoints("perder");
        //Falta añadir la lógica de calcular la performance en el puzzle
        puzzleEvaluation.FinishGame(false);
        Destroy(this.gameObject);
    }
}