using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public bool isGameOver = false;



    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Evitar la instanciación externa
    private GameManager() { }

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }

        return instance;
    }


}
