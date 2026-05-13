using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class BloodStainDropZone : MonoBehaviour
{

    [Header("Objetos Asociados")]
    [SerializeField] SequentialProccessPuzzleGameplay _sequentialProccessGameplay;


    [Header("Configuración de Detección")]
    [SerializeField] private string _draggableItemTag = "BloodStainPuzzleItem";

    // Evento para que el Manager escuche (Desacoplamiento)
    public Action<string> OnDraggableItemDropped;

    private void Start()
    {
        if (GetComponentInParent<SequentialProccessPuzzleGameplay>() == null)
        {
            Debug.LogError("BloodStainFeedbackVisuals: Parent object with SequentialProccessPuzzleGameplay component not found.");
            return;
        }
        _sequentialProccessGameplay = GetComponentInParent<SequentialProccessPuzzleGameplay>();

        _sequentialProccessGameplay.OnPuzzleLost += DisableComponents;
    }


    private void OnDestroy()
    {
        _sequentialProccessGameplay.OnPuzzleLost -= DisableComponents;
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