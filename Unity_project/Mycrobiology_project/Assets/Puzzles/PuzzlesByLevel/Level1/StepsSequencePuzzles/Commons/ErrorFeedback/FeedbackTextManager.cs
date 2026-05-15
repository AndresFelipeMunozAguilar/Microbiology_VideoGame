using UnityEngine;
using TMPro;

[ExecuteAlways] // Para que se vea el cambio incluso en el Editor sin darle a Play
[RequireComponent(typeof(TextMeshPro))]
public class FeedbackTextManager : MonoBehaviour
{
    [Header("Configuración de Capas")]
    [SerializeField] private string _sortingLayerName = "Puzzles";
    [SerializeField] private int _orderInLayer = 70;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        ApplySorting();
    }

    private void OnValidate()
    {
        // Se ejecuta cuando cambias valores en el Inspector
        ApplySorting();
    }

    public void ApplySorting()
    {
        if (_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();

        _meshRenderer.sortingLayerName = _sortingLayerName;
        _meshRenderer.sortingOrder = _orderInLayer;
    }
}