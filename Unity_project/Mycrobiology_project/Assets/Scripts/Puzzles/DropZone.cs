using UnityEngine;

public class DropZone : MonoBehaviour
{
    [SerializeField] Transform pointPosition;
    bool Occupied;
    [SerializeField] bool isFinish;
    public bool IsOccupied()
    {
        return Occupied;
    }
    public bool setOccupied(bool newValue)
    {
        Occupied=newValue;
        return isFinish;
    }
    public Vector2 getPosition()
    {
        if(pointPosition!=null) return pointPosition.position;
        return transform.position;

    }
}
