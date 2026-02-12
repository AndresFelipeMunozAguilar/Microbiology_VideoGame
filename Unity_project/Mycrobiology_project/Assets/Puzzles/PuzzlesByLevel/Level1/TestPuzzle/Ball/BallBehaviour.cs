using UnityEngine;
using UnityEngine.EventSystems;

public class BallBehaviour : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Basket BALL: OnPOinterDown");
    }
}