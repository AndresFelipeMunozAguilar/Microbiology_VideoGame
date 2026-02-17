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


    public abstract void StartGameplay();

    public void SpawnBackground(Vector3 position, Quaternion rotation, Transform parent)
    {
        Instantiate(background, position, rotation, parent);
    }

    public abstract void Victory();

    public abstract void Defeat();

    public abstract void ShowTutorial();

    public abstract bool IsFirstTime();

}