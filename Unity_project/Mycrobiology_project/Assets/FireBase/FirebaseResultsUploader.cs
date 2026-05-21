using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;

public class FirebaseResultsUploader : MonoBehaviour
{
    private FirebaseFirestore db;
    public static FirebaseResultsUploader Instance;
    private bool firebaseReady = false;
    private string PlayerID ="PlayerIncognito";
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
    public string getPlayerId(){
        return PlayerID;
    }
    public void setPlayerId(string id){
        PlayerID = id; 
    }
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                firebaseReady = true;
            }
            else
            {
                
            }
        });
    }
    IEnumerator InitFirebase()
    {
        var task = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Result == DependencyStatus.Available)
        {
            db = FirebaseFirestore.DefaultInstance;
            firebaseReady = true;
            Debug.Log("[Firebase] Listo");
        }
        else
        {
            Debug.LogError("[Firebase] Error: " + task.Result);
        }
    }
    public void UploadEvaluation(EvaluationData data,TextMeshProUGUI tx)
    {
        if (!firebaseReady)
        {
            Debug.LogWarning("[Firebase] Todavía no está listo.");
            tx.text= "Firebase no listo " +data.playerID+" : " +PlayerPrefs.GetString("playerID", "");
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
        data.playerID = PlayerPrefs.GetString("playerID", "");
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
                tx.text= "Resultados enviados al profesor";
            }
            else
            {
                Debug.LogError("[Firebase] Error subiendo resultado: " + task.Exception);
                tx.text=  "Error subiendo resultados: " +task.Exception;
            }
        });
        
    }

    public void UploadEvaluationFromFile(TextMeshProUGUI tx)
    {
        string path = Application.persistentDataPath + "/evaluation.json";

        if (!File.Exists(path))
        {
            Debug.LogWarning("[Firebase] No existe evaluation.json en: " + path);
            tx.text= "No se encontraron resultados";
        }

        string json = File.ReadAllText(path);
        EvaluationData data = JsonUtility.FromJson<EvaluationData>(json);

        UploadEvaluation(data,tx);
    }
}