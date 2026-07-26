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


    void Start()
    {
        puzzleEvaluation = GetComponent<PuzzleEvaluation>();
    }
    protected override void OnStartGameplay()
    {
        Vector3 inFrontOfCamera = Camera.main.transform.position;
        inFrontOfCamera.z = 0f;

        Debug.Log("PuzzleGameplay son: Vamos a instanciar el fondo y los objetos");

        SpawnBackground(inFrontOfCamera, Quaternion.identity, transform);
        SetObjects(transform);
    }


    public void SetObjects(Transform parent)
    {
        Debug.Log($"PuzzleGameplay: Al instanciar los objetos, el padre es: {parent.gameObject.name} y su posicion es: {parent.position}");
        Debug.Log($"PuzzleGameplay: Instanciando pelota de basket en la posiciones: {ballSpawnPlace}");

        BasketTriggerZone[] basketTriggers = GetComponentsInChildren<BasketTriggerZone>();
        basketTriggers[0].isThisVictoryTrigger = false;
        basketTriggers[1].isThisVictoryTrigger = true;

    }

    public override void Victory()
    {
        Debug.Log("PuzzleGamelay: You won the Puzzle: Victory!");
        puzzleEvaluation.AddPoints("ganar");
        //Falta añadir la lógica de calcular la performance en el puzzle
        puzzleEvaluation.FinishGame(true);
        Destroy(this.gameObject);
    }

    public override void Defeat()
    {
        Debug.Log("PuzzleGamelay: You lost the Puzzle: Defeat!");
        puzzleEvaluation.RemovePoints("perder");
        //Falta añadir la lógica de calcular la performance en el puzzle
        puzzleEvaluation.FinishGame(false);
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
