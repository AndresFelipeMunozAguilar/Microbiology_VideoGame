using UnityEngine;

public class PuzzleGameplay : AbstractPuzzleGameplay
{
    // [SerializeField]
    // private GameObject background;

    // private bool isFirstTimePlaying = false;

    // [SerializeField]
    // private GameObject tutorialPrefab;

    [SerializeField]
    private GameplayDirector gameplayDirector;


    public override void StartGameplay()
    {
        Vector3 inFrontOfCamera = Camera.main.transform.position;
        inFrontOfCamera.z = 0f;
        SpawnBackground(inFrontOfCamera, Quaternion.identity, this.transform);
        gameplayDirector.StartGameplay(this.transform);

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

    public override void ShowTutorial()
    {
        Debug.Log("Showing Puzzle Tutorial");
        Instantiate(tutorialPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
    }

    public override bool IsFirstTime()
    {
        Debug.Log($"Is the first time playing the puzzle? {isFirstTimePlaying}");
        return isFirstTimePlaying;
    }
}
