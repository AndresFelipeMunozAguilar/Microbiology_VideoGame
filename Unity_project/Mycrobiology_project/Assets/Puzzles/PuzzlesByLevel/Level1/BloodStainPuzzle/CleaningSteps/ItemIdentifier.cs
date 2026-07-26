using UnityEngine;

public class ItemIdentifier : MonoBehaviour
{
    [Header("Identidad del Ítem")]
    [Tooltip("Este ID debe coincidir con el 'Required Item Id' en el ScriptableObject.")]
    [SerializeField] private string _itemId;
    [SerializeField] private CleaningStepSO _stepInformation;

    public string ItemId => _itemId;

    public void Awake()
    {
        _itemId = _stepInformation.RequiredItemId;
    }

    // Se podría añadir lógica aquí para resaltar el ítem al pasar el mouse
}