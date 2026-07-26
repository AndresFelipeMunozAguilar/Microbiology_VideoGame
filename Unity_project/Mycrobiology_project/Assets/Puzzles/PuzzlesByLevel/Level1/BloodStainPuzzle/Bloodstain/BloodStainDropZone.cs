using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class BloodStainDropZone : MonoBehaviour
{

    [Header("Objetos Asociados")]
    [SerializeField] BloodStainPuzzleGameplay _bloodStainGameplay;


    [Header("Configuración de Detección")]
    [SerializeField] private string _draggableItemTag = "BloodStainPuzzleItem";

    // Evento para que el Manager escuche (Desacoplamiento)
    public Action<string> OnDraggableItemDropped;

    private void Start()
    {
        if (GetComponentInParent<BloodStainPuzzleGameplay>() == null)
        {
            Debug.LogError("BloodStainFeedbackVisuals: Parent object with BloodStainPuzzleGameplay component not found.");
            return;
        }
        _bloodStainGameplay = GetComponentInParent<BloodStainPuzzleGameplay>();

        _bloodStainGameplay.OnPuzzleLost += DisableComponents;
    }


    private void OnDestroy()
    {
        _bloodStainGameplay.OnPuzzleLost -= DisableComponents;
    }

    private void DisableComponents()
    {
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si lo que entró es un ítem válido
        if (!other.CompareTag(_draggableItemTag)) return;

        ItemIdentifier draggableItem = other.GetComponent<ItemIdentifier>();

        if (draggableItem == null)
        {
            Debug.LogWarning($"BloodStainDropZone: El objeto con tag: {other.tag} no tiene componente ItemIdentifier.");
            return;
        }

        Debug.Log($"BloodStainDropZone: Se detectó el ítem {draggableItem.ItemId}");
        OnDraggableItemDropped?.Invoke(draggableItem.ItemId);

    }
}