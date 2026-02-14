using UnityEngine;

public class GameplayDirector : MonoBehaviour
{

    [SerializeField]
    private GameObject floorPrefab;

    [SerializeField]
    private GameObject ballPrefab;

    [SerializeField]
    private GameObject victoryBasketPrefab;

    [SerializeField]
    private GameObject defeatBasketPrefab;

    [SerializeField]
    private Vector3 ballSpawnPlace;

    [SerializeField]
    private Vector3 floorSpawnPlace;

    [SerializeField]
    private Vector3 vicBasketSpawnPlace;

    [SerializeField]
    private Vector3 defBasketSpawnPlace;


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

    public void StartGameplay(Transform parent)
    {
        InstantiateObjects(parent);
    }
}