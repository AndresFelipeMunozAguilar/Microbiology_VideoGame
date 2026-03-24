using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DropZone : MonoBehaviour
{
    [Header("Configuración de Detección")]
    [SerializeField] private string _draggableItemTag = "BloodStainPuzzleItem";

    // Evento para que el Manager escuche (Desacoplamiento)
    public Action<string> OnDraggableItemDropped;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si lo que entró es un ítem válido
        if (!other.CompareTag(_draggableItemTag)) return;

        ItemIdentifier draggableItem = other.GetComponent<ItemIdentifier>();

        if (draggableItem == null)
        {
            Debug.LogWarning($"DropZone: El objeto con tag: {other.tag} no tiene componente ItemIdentifier.");
            return;
        }

        Debug.Log($"DropZone: Se detectó el ítem {draggableItem.ItemId}");
        OnDraggableItemDropped?.Invoke(draggableItem.ItemId);

    }
}