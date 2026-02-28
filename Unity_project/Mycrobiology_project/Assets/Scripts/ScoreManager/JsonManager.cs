using UnityEngine;
using System.IO;

public class JsonManager : MonoBehaviour
{
    public static JsonManager Instance;

    public EvaluationData playerData;

    private string path;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            path = Application.persistentDataPath + "/evaluation.json";
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(path, json);
        Debug.Log("Datos guardados en: " + path);
    }

    public void Load()
    {
        if (!File.Exists(path))
        {
            playerData = new EvaluationData();
            return;
        }

        string json = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<EvaluationData>(json);
    }

    public PuzzleResultData GetPuzzleByID(string id)
    {
        if (playerData.puzzles == null) return null;

        foreach (var puzzle in playerData.puzzles)
        {
            if (puzzle.puzzleID == id)
                return puzzle;
        }

        return null;
    }
}