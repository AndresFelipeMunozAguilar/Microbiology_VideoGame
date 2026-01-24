using UnityEngine;

public class PuzzleManager : MonoBehaviour
{

    public void OnTap(GameObject target)
    {
        Debug.Log($"Tap detectado sobre: {target.name}");
    }

}
