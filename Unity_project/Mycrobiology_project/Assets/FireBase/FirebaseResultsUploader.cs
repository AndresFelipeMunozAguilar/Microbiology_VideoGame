using System.Collections.Generic;
using System.IO;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseResultsUploader : MonoBehaviour
{
    private FirebaseFirestore db;
    private bool firebaseReady = false;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                firebaseReady = true;
                Debug.Log("[Firebase] Firestore listo.");
            }
            else
            {
                Debug.LogError("[Firebase] Error de dependencias: " + task.Result);
            }
        });
    }

    public void UploadEvaluation(EvaluationData data)
    {
        if (!firebaseReady)
        {
            Debug.LogWarning("[Firebase] Todavía no está listo.");
            return;
        }

        string rawJson = JsonUtility.ToJson(data, true);

        List<Dictionary<string, object>> puzzleList = new List<Dictionary<string, object>>();

        foreach (PuzzleResultData puzzle in data.puzzles)
        {
            Dictionary<string, object> puzzleData = new Dictionary<string, object>
            {
                { "puzzleID", puzzle.puzzleID },
                { "score", puzzle.score },
                { "bestScore", puzzle.bestScore },
                { "performance", puzzle.performance }
            };

            puzzleList.Add(puzzleData);
        }

        Dictionary<string, object> result = new Dictionary<string, object>
        {
            { "playerID", data.playerID },
            { "totalScore", data.totalScore },
            { "date", data.date },
            { "puzzles", puzzleList },
            { "rawJson", rawJson },
            { "createdAt", Timestamp.GetCurrentTimestamp() }
        };

        db.Collection("game_results").AddAsync(result).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                Debug.Log("[Firebase] Resultado subido correctamente.");
            }
            else
            {
                Debug.LogError("[Firebase] Error subiendo resultado: " + task.Exception);
            }
        });
    }

    public void UploadEvaluationFromFile()
    {
        string path = Application.persistentDataPath + "/evaluation.json";

        if (!File.Exists(path))
        {
            Debug.LogWarning("[Firebase] No existe evaluation.json en: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        EvaluationData data = JsonUtility.FromJson<EvaluationData>(json);

        UploadEvaluation(data);
    }
}