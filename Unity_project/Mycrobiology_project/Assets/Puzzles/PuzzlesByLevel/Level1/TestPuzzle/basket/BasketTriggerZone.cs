using UnityEngine;

public class BasketTriggerZone : MonoBehaviour
{
    [SerializeField]
    private string ballTag = "TestPuzzleBall";

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(ballTag)) Debug.Log($"Se ha chocado con {ballTag}");
    }

}