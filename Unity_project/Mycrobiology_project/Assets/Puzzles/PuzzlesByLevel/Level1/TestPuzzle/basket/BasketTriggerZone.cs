using Unity.VisualScripting;
using UnityEngine;

public class BasketTriggerZone : MonoBehaviour
{
    [SerializeField]
    private string ballTag = "TestPuzzleBall";

    [Serialize]
    public bool isThisVictoryTrigger { get; set; }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(ballTag))
        {
            IPuzzleManager puzzleManager = GetComponentInParent<IPuzzleManager>();

            if (puzzleManager == null)
            {
                Debug.LogWarning("No se encontro el Componente PuzzleManager");
                return;
            }

            bool didPlayerWin = isThisVictoryTrigger;
            puzzleManager.CompletePuzzle(didPlayerWin);


        }
    }

}