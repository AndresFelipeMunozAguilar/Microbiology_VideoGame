using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    private EvaluationData CurrentData;
    public static DataManager Instance;
    
    private void Start() {
        Instance=this;   
        CurrentData = LoadEvaluation();
    }
    
    public void SaveEvaluation(EvaluationData data)
    {
        string json = JsonUtility.ToJson(data, true);

        string path = Application.persistentDataPath + "/evaluation.json";

        File.WriteAllText(path, json);

        Debug.Log("Datos guardados en: " + path);
    }

    private EvaluationData LoadEvaluation()
    {
        string path = Application.persistentDataPath + "/evaluation.json";

        if (!File.Exists(path))
            return null;

        string json = File.ReadAllText(path);

        return JsonUtility.FromJson<EvaluationData>(json);
    }
    public PuzzleResultData GetPuzzleByID(string ID)
    {
        foreach (PuzzleResultData puzzle in CurrentData.puzzles)
        {
            if (puzzle.puzzleID.Equals(ID))
            {
                return puzzle;
            }
        }
        return null;
    }

    public bool LoadTutorialFlag(string ID)
    {
        PuzzleResultData puzzle = GetPuzzleByID(ID);
        if(puzzle != null && puzzle.tutorialFlag)return true;
        else return false;
    }
}