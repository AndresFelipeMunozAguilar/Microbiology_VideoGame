using UnityEngine;

public class PuzzleGameplay : MonoBehaviour
{
    [SerializeField]
    private GameObject background;
    public void StartGameplay()
    {
        Vector3 inFrontOfCamera = Camera.main.transform.position;
        inFrontOfCamera.z = 0f;
        SpawnBackground(inFrontOfCamera, Quaternion.identity, this.transform);

    }

    public void SpawnBackground(Vector3 position, Quaternion rotation, Transform parent)
    {
        Instantiate(background, position, rotation, parent);
    }
}
