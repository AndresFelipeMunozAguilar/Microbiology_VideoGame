using UnityEngine;

public abstract class AbstractPuzzleGameplay
{
    private DataManager dataManager;
    private GameObject background;
    private bool isFirstTimePlaying;
    private GameObject tutorialPrefab;


    public abstract void StartGameplay();

    public abstract void SpawnBackground(Vector3 position, Quaternion rotation, Transform parent);

    public abstract void Victory();

    public abstract void Defeat();

    public abstract void ShowTutorial();

    public abstract bool IsFirstTime();

}