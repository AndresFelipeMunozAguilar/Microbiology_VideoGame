using UnityEngine;

public class PuzzleManager : MonoBehaviour, ITappable
{

    public void OnTap()
    {
        Debug.Log($"Tap detectado sobre: {this.gameObject.name}");
    }

}
