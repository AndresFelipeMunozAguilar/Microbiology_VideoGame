using System;
using UnityEngine;
using UnityEngine.Events;

public class SequentialProccessPuzzleGameplay : AbstractPuzzleGameplay
{
    [Header("Configuración de Datos")]
    [SerializeField] private PuzzleSequenceSO _puzzleSequence;
    [SerializeField] private BloodStainVisuals _visuals;
    [SerializeField] private BloodStainDropZone _dropZone;

    [Header("Estado del Juego (Solo Lectura)")]
    [SerializeField] private int _currentStepIndex = 0;
    [SerializeField] private int _errorCount = 0;

    [Header("Eventos de Salida")]
    public Action OnPuzzleWin;
    public Action OnPuzzleLost;
    public Action<int, int> OnErrorChanged; // (errores actuales, max errores)

    [Header("Calificación del Desempeño")]
    // Este atributo debe coincidir con el valor de la 
    // DamageTable asociada para cuando se arrastra 
    // el item en el orden correcto
    [SerializeField] private string _puzzleEvaluationSuccesKey = "acierto";
    [SerializeField] private string _puzzleEvaluationErrorKey = "error";

    protected override void OnInstanceAwake()
    {
        if (_puzzleSequence == null)
        {
            Debug.LogError($"<color=yellow>{GetType().Name}:</color> Falta PuzzleSequenceSO en {gameObject.name}");
            return;
        }

        Debug.Log($"<color=yellow>{GetType().Name}:</color> Iniciado. Esperando primer paso.");

    }

    protected override void OnStartGameplay()
    {
        GetAssociatedComponents();
        _dropZone.OnDraggableItemDropped += ProcessItemInteraction;
    }

    private void GetAssociatedComponents()
    {
        _visuals = GetComponentInChildren<BloodStainVisuals>();

        if (_visuals == null)
        {
            Debug.LogError($"<color=yellow>{GetType().Name}:</color> No se encontró el componente BloodStainVisuals en los hijos de {this.gameObject.name}");
            return;
        }

        _dropZone = GetComponentInChildren<BloodStainDropZone>();
        if (_dropZone == null)
        {
            Debug.LogError($"<color=yellow>{GetType().Name}:</color> No se encontró el componente DropZone en los hijos de {this.gameObject.name}");
            return;
        }

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
        Debug.Log($"<color=yellow>{GetType().Name}:</color> Paso Correcto: {_currentStepIndex}/{_puzzleSequence.TotalSteps}");

        _puzzleEvaluation.AddPoints(_puzzleEvaluationSuccesKey);

        // Notificar a los visuales para cambiar el sprite de la mancha
        _visuals.UpdateVisuals(step.StepResultSprite);

        if (_currentStepIndex >= _puzzleSequence.TotalSteps)
        {
            _visuals.UpdateVisuals(_puzzleSequence.FinalCleanSprite);

            Debug.Log("<color=yellow>{GetType().Name}:</color> ¡Puzzle Completado!");

            NotifyPuzzleVictory(true);
        }
    }

    private void HandleIncorrectStep()
    {
        _errorCount++;
        Debug.Log($"<color=red>Error detectado:</color> {_errorCount}/{_puzzleSequence.MaxAllowedErrors}");

        _puzzleEvaluation.RemovePoints(_puzzleEvaluationErrorKey);

        Debug.Log($"<color=yellow>{GetType().Name}:</color> Listeners suscritos a OnErrorChanged: {OnErrorChanged?.GetInvocationList().Length ?? 0}");
        OnErrorChanged?.Invoke(_errorCount, _puzzleSequence.MaxAllowedErrors);

        if (_errorCount >= _puzzleSequence.MaxAllowedErrors)
        {
            _visuals.DisableStain();

            Debug.Log($"<color=yellow>{GetType().Name}:</color> Puzzle Fallido por exceso de errores");
            OnPuzzleLost?.Invoke();

            NotifyPuzzleVictory(false);
        }
    }


    public override void Victory()
    {
        Debug.Log("PuzzleGamelay: You won the Puzzle: Victory!");
        Destroy(this.gameObject);
    }

    public override void Defeat()
    {
        Debug.Log("PuzzleGamelay: You lost the Puzzle: Defeat!");
        Destroy(this.gameObject);
    }
}