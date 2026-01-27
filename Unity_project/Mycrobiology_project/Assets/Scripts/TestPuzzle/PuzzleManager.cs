using UnityEngine;

public class PuzzleManager : MonoBehaviour, ITappable
{

    [SerializeField]
    private PuzzleHalo halo;

    public void OnTap()
    {
        if (halo.isPlayerClose)
        {
            Debug.Log($"Tap detectado sobre: {this.gameObject.name}");
        }

    }

}
