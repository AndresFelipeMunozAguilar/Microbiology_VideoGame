using UnityEngine;

public class DropZone : MonoBehaviour
{
    [SerializeField] Transform pointPosition;
    bool Occupied;
    public bool IsOccupied()
    {
        return Occupied;
    }
    public void setOccupied(bool newValue)
    {
        Occupied=newValue;
    }
    public Vector2 getPosition()
    {
        if(pointPosition!=null) return pointPosition.position;
        return transform.position;

    }
}
