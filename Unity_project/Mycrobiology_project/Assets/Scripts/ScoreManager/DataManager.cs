using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    private EvaluationData CurrentData;
    public static DataManager Instance;

    private void Start()
    {
        Instance = this;
        CurrentData = LoadEvaluation();
    }

    public void SaveEvaluation(EvaluationData data)
    {
        // Debug de entrada: Verificar que no estemos intentando guardar datos nulos
        if (data == null)
        {
            Debug.LogError("SaveEvaluation: Se intentó guardar un objeto EvaluationData nulo.");
            return;
        }

        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/evaluation.json";

        try
        {
            File.WriteAllText(path, json);
            Debug.Log($"<color=green>SaveEvaluation:</color> Datos guardados exitosamente en: {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"SaveEvaluation: Fallo crítico al escribir el archivo en {path}. Error: {e.Message}");
        }
    }

    private EvaluationData LoadEvaluation()
    {
        string path = Application.persistentDataPath + "/evaluation.json";

        if (!File.Exists(path))
        {
            Debug.LogWarning($"LoadEvaluation: No se encontró el archivo en {path}. Se retornará null.");
            return null;
        }

        string json = File.ReadAllText(path);
        EvaluationData data = JsonUtility.FromJson<EvaluationData>(json);

        if (data == null)
        {
            Debug.LogError("LoadEvaluation: El JSON existe pero la deserialización falló (JSON corrupto o vacío).");
        }
        else
        {
            Debug.Log("<color=cyan>LoadEvaluation:</color> Datos cargados y deserializados correctamente.");
        }

        return data;
    }

    public PuzzleResultData GetPuzzleByID(string ID)
    {
        if (string.IsNullOrEmpty(ID))
        {
            Debug.LogWarning("GetPuzzleByID: El ID proporcionado está vacío o es nulo.");
            return null;
        }

        foreach (PuzzleResultData puzzle in CurrentData.puzzles)
        {
            if (puzzle.puzzleID.Equals(ID))
            {
                Debug.Log($"GetPuzzleByID: Se encontró coincidencia para el ID: {ID}");
                return puzzle;
            }
        }

        Debug.LogWarning($"GetPuzzleByID: No se encontró ningún puzzle con el ID: {ID} en CurrentData.");
        return null;
    }

    public bool LoadTutorialFlag(string ID)
    {
        Debug.Log($"LoadTutorialFlag: Consultando estado del tutorial para ID: {ID}");

        PuzzleResultData puzzle = GetPuzzleByID(ID);


        if (puzzle != null && puzzle.tutorialFlag)
        {
            Debug.Log($"LoadTutorialFlag: Puzzle encontrado. Valor de flag: {puzzle.tutorialFlag}");
            return true;
        }
        else
        {
            Debug.LogWarning($"LoadTutorialFlag: No se pudo cargar el flag porque el puzzle {ID} no existe.");
            return false;
        }


    }
}