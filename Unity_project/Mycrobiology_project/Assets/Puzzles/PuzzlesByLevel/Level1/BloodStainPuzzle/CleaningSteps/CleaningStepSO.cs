using UnityEngine;

[CreateAssetMenu(fileName = "CleaningStepSO", menuName = "ScriptableObject/Puzzles/Cleaning Step")]
public class CleaningStepSO : ScriptableObject
{
    [Header("Configuración del Paso")]
    [Tooltip("ID único del ítem que el jugador debe arrastrar (ej. 'Papel', 'Cloro').")]
    [SerializeField] private string _requiredItemId;

    [Header("Feedback Visual")]
    [Tooltip("El sprite que la mancha mostrará una vez que este paso se complete con éxito.")]
    [SerializeField] private Sprite _stepResultSprite;

    public string RequiredItemId => _requiredItemId;
    public Sprite StepResultSprite => _stepResultSprite;
}