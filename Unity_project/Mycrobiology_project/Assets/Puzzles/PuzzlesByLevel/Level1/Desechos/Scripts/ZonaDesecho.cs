using UnityEngine;

public class ZonaDesecho : MonoBehaviour
{
    [Header("Método que representa esta zona")]
    [SerializeField] private MetodoDescontaminacion metodoZona;

    [Header("Punto opcional de feedback")]
    [SerializeField] private Transform feedbackPoint;

    public MetodoDescontaminacion GetMetodoZona()
    {
        return metodoZona;
    }

    public Vector2 GetFeedbackPosition()
    {
        if (feedbackPoint != null)
        {
            return feedbackPoint.position;
        }

        if (TryGetComponent(out DropZone dropZone))
        {
            return dropZone.getPosition();
        }

        return transform.position;
    }
}