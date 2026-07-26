using System;
using UnityEngine;
using UnityEngine.Events;

public class DropZone : MonoBehaviour
{
    [SerializeField] Transform pointPosition;
    bool Occupied=false;
    [SerializeField] bool isFinish;


    public bool IsOccupied()
    {
        return Occupied;
    }
    public void setOccupied(bool newValue)
    {
        TryGetComponent<WarmManager>(out WarmManager test);
        if(test) test.ChangeState(newValue);
        Occupied=newValue;
    }
    public bool IsFinishZone()
    {
        return isFinish;
    }
    public bool havePoint() => pointPosition != null;
    public Vector2 getPosition()
    {
        if(pointPosition!=null) return pointPosition.position;
        return transform.position;

    }
}
