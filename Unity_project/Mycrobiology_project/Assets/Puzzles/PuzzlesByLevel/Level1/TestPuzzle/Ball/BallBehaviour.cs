using UnityEngine;
using UnityEngine.EventSystems;

public class BallBehaviour : AbstractDraggableWorldObject
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private float zDistanceToCamera;

    [SerializeField]
    private Rigidbody2D rb;


    public override void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Basket BALL: OnPOinterDown");
    }

    protected override void OnDragEnded()
    {
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }
}