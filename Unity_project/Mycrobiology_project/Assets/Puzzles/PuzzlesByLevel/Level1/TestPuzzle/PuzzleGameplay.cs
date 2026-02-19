using UnityEngine;

public class PuzzleGameplay : AbstractPuzzleGameplay
{
    // [SerializeField]
    // private GameObject background;

    // private bool isFirstTimePlaying = false;

    // [SerializeField]
    // private GameObject tutorialPrefab;

    [Header("Ball Settings")]
    [Tooltip("Prefab de la pelota principal")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Vector3 ballSpawnPlace;


    [Header("Environment Setup")]
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private Vector3 floorSpawnPlace;


    [Header("Baskets (Win/Loss)")]
    [SerializeField] private GameObject victoryBasketPrefab;
    [SerializeField] private Vector3 vicBasketSpawnPlace;

    [Space(5)]
    [SerializeField] private GameObject defeatBasketPrefab;

    [SerializeField] private Vector3 defBasketSpawnPlace;

    public override void StartGameplay()
    {
        Vector3 inFrontOfCamera = Camera.main.transform.position;
        inFrontOfCamera.z = 0f;

        Debug.Log("PuzzleGameplay son: Vamos a instanciar el fondo y los objetos");

        SpawnBackground(inFrontOfCamera, Quaternion.identity, transform);
        InstantiateObjects(transform);
    }


    public void InstantiateObjects(Transform parent)
    {
        Instantiate(ballPrefab, ballSpawnPlace, Quaternion.identity, parent);

        Instantiate(victoryBasketPrefab, vicBasketSpawnPlace, Quaternion.identity, parent)
            .GetComponentInChildren<BasketTriggerZone>()
            .isThisVictoryTrigger = true;

        Instantiate(defeatBasketPrefab, defBasketSpawnPlace, Quaternion.identity, parent)
            .GetComponentInChildren<BasketTriggerZone>()
            .isThisVictoryTrigger = false;

        Instantiate(floorPrefab, floorSpawnPlace, Quaternion.identity, parent);
    }

    public override void Victory()
    {
        Debug.Log("PuzzleGamelay: You won the Puzzle: Victory!");

        //Falta añadir la lógica de calcular la performance en el puzzle

        Destroy(this.gameObject);
    }

    public override void Defeat()
    {
        Debug.Log("PuzzleGamelay: You lost the Puzzle: Defeat!");

        //Falta añadir la lógica de calcular la performance en el puzzle

        Destroy(this.gameObject);
    }

    public void Hola()
    {
        Debug.Log("Hola");
    }

    // public override void ShowTutorial()
    // {
    //     // Debug.Log("Showing Puzzle Tutorial");
    //     Instantiate(tutorialPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
    // }

    // public override bool IsFirstTime()
    // {
    //     Debug.Log($"Is the first time playing the puzzle? {isFirstTimePlaying}");
    //     return isFirstTimePlaying;
    // }
}
