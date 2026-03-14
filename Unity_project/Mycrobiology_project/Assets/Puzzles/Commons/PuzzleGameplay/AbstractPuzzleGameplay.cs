using UnityEngine;

public abstract class AbstractPuzzleGameplay : MonoBehaviour
{
    [SerializeField]
    protected DataManager dataManager;

    [SerializeField]
    protected GameObject background;

    [SerializeField]
    protected bool isFirstTimePlaying;

    [SerializeField]
    protected GameObject tutorialPrefab;

    [SerializeField]
    protected PuzzleEvaluation puzzleEvaluation;

    public abstract void StartGameplay();

    public void SpawnBackground(Vector3 position, Quaternion rotation, Transform parent)
    {
        Instantiate(background, position, rotation, parent);
    }

    public abstract void Victory();

    public abstract void Defeat();

    public void ShowTutorial()
    {
        // Debug.Log("Showing Puzzle Tutorial");
        Instantiate(tutorialPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
    }

    public bool IsFirstTime()
    {
        Debug.Log($"Is the first time playing the puzzle? {isFirstTimePlaying}");
        return isFirstTimePlaying;
    }

    public PuzzleEvaluation GetPuzzleEvaluation()
    {
        return puzzleEvaluation;
    }

}