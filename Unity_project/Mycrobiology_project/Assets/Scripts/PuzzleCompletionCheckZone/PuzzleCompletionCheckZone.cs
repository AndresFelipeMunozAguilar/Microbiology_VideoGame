using UnityEngine;

public class PuzzleCompletionCheckZone : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"<color=cyan>PuzzleCompletionCheckZone:</color> Trigger entered by: {other.gameObject.name}");
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"<color=cyan>PuzzleCompletionCheckZone:</color> Trigger exited by: {other.gameObject.name}");
    }
}