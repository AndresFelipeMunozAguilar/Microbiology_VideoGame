using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleSequenceSO", menuName = "ScriptableObject/Puzzles/Sequence")]
public class PuzzleSequenceSO : ScriptableObject
{
    [Header("Configuración de la Secuencia")]
    [Tooltip("Lista ordenada de pasos que el jugador debe seguir.")]
    [SerializeField] private List<ProcessStepSO> _steps;

    [Header("Reglas de Dificultad")]
    [Tooltip("Número máximo de errores permitidos antes de que el puzzle se bloquee.")]
    [Range(1, 10)]
    [SerializeField] private int _maxAllowedErrors = 3;

    [Header("Feedback Final")]
    [Tooltip("Sprite que muestra la mesa totalmente limpia (Paso final).")]
    [SerializeField] private Sprite _finalCleanSprite;

    public IReadOnlyList<ProcessStepSO> Steps => _steps;
    public int MaxAllowedErrors => _maxAllowedErrors;
    public Sprite FinalCleanSprite => _finalCleanSprite;

    public int TotalSteps => _steps.Count;
}