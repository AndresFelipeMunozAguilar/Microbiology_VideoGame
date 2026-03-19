using UnityEngine;


public class WarmManager : MonoBehaviour
{
    [SerializeField] Transform warmPoint;

    public void StartWarming(ElementManager element)
    {
        element.PlaceWarm(warmPoint.position);
    }


}
