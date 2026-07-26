using UnityEngine;

public class FirebaseResultsUploader : MonoBehaviour
{
    public static FirebaseResultsUploader Instance;
    private string PlayerID = "PlayerIncognito";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string getPlayerId()
    {
        return PlayerID;
    }

    public void setPlayerId(string id)
    {
        PlayerID = id;
    }
}