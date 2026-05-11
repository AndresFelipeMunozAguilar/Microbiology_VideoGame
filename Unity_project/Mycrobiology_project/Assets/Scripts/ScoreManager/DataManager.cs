using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    private EvaluationData CurrentData;
    public static DataManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CurrentData = LoadEvaluation();
    }

    public void SaveEvaluation(EvaluationData data)
    {

        CurrentData = data;
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
        Debug.Log($"<color=blue>{this.GetType().Name}:</color> Buscando datos del puzzle con ID '{ID}' en CurrentData. CurrentData es null? {CurrentData == null}");
        if (CurrentData != null)
        {
            foreach (PuzzleResultData puzzle in CurrentData.puzzles)
            {
                if (puzzle.puzzleID.Equals(ID))
                {
                    Debug.Log($"<color=blue>{this.GetType().Name}:</color> Puzzle encontrado: {puzzle.puzzleID}");
                    return puzzle;
                }
            }
        }
        Debug.Log($"<color=red>{this.GetType().Name}:</color> Puzzle no encontrado: {ID}");
        return null;
    }

    // FUNCIÓN OBSOLETA, MALA, DEPRECATED: BORRAR SI NO SE NECESITA
    // public bool LoadTutorialFlag(string ID)
    // {
    //     PuzzleResultData puzzle = GetPuzzleByID(ID);
    //     Debug.Log($"<color=blue>{this.GetType().Name}:</color> Cargando bandera de tutorial para el puzzle '{ID}'. PuzzleResultData encontrado: {puzzle != null}");

    //     if (puzzle != null && puzzle.tutorialFlag) return true;
    //     else return false;
    // }


    public bool HasPuzzleBeenPlayed(string puzzleID)
    {
        CurrentData = LoadEvaluation();

        // Si el puzzle ya existe en los datos cargados, es porque ya se jugó
        PuzzleResultData puzzleResultData = GetPuzzleByID(puzzleID);

        Debug.Log($"<color=blue>{this.GetType().Name}:</color> Verificando si el puzzle '{puzzleID}' ha sido jugado. PuzzleResultData encontrado: {puzzleResultData != null}");
        return puzzleResultData != null;
    }

}