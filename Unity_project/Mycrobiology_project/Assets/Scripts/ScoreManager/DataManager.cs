using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private EvaluationData currentData;
    private string currentPlayerID;

    public static DataManager Instance
    {
        get;
        private set;
    }

    // Este metodo mantiene una sola instancia del gestor de datos.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Este metodo carga los datos del jugador guardado en PlayerPrefs.
    private void Start()
    {
        string playerID = PlayerPrefs.GetString("playerID", "");
        currentPlayerID = playerID;
        currentData = LoadEvaluation(playerID);
    }

    // Este metodo actualiza el jugador activo y carga su evaluacion.
    public void SetPlayerID(string playerID)
    {
        currentPlayerID = playerID;
        currentData = LoadEvaluation(playerID);
    }

    // Este metodo guarda la evaluacion local del jugador.
    public void SaveEvaluation(EvaluationData data, string playerID = null)
    {
        if (!string.IsNullOrEmpty(playerID))
        {
            currentPlayerID = playerID;
        }
        else
        {
            playerID = currentPlayerID;
        }

        currentData = data;
        string json = JsonUtility.ToJson(data, true);
        string path = GetEvaluationFilePath(playerID);

        File.WriteAllText(path, json);
        Debug.Log("Datos guardados en: " + path);
    }

    // Este metodo carga desde archivo la evaluacion del jugador.
    private EvaluationData LoadEvaluation(string playerID = null)
    {
        string path = GetEvaluationFilePath(playerID);

        if (!File.Exists(path))
        {
            return null;
        }

        string json = File.ReadAllText(path);

        return JsonUtility.FromJson<EvaluationData>(json);
    }

    // Este metodo construye la ruta local donde se guarda la evaluacion.
    public static string GetEvaluationFilePath(string playerID = null)
    {
        string id = playerID;

        if (string.IsNullOrEmpty(id))
        {
            id = PlayerPrefs.GetString("playerID", "");
        }

        string fileName = string.IsNullOrEmpty(id) ? "evaluation.json" : $"evaluation_{id}.json";
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    // Este metodo busca el resultado de un puzzle por su identificador.
    public PuzzleResultData GetPuzzleByID(string id)
    {
        Debug.Log($"<color=blue>{GetType().Name}:</color> Buscando datos del puzzle con ID '{id}' en currentData. currentData es null? {currentData == null}");

        if (currentData != null)
        {
            foreach (PuzzleResultData puzzle in currentData.puzzles)
            {
                if (puzzle.puzzleID.Equals(id))
                {
                    Debug.Log($"<color=blue>{GetType().Name}:</color> Puzzle encontrado: {puzzle.puzzleID}");
                    return puzzle;
                }
            }
        }

        Debug.Log($"<color=red>{GetType().Name}:</color> Puzzle no encontrado: {id}");
        return null;
    }

    // Este metodo verifica si el jugador ya registro ese puzzle anteriormente.
    public bool HasPuzzleBeenPlayed(string puzzleID)
    {
        currentData = LoadEvaluation(currentPlayerID);

        Debug.Log($"<color=blue>{GetType().Name}:</color> La ruta de Application.persistentDataPath es: {Application.persistentDataPath}");

        // Si el puzzle ya existe en los datos cargados, es porque ya se jugó
        PuzzleResultData puzzleResultData = GetPuzzleByID(puzzleID);

        Debug.Log($"<color=blue>{GetType().Name}:</color> Verificando si el puzzle '{puzzleID}' ha sido jugado. PuzzleResultData encontrado: {puzzleResultData != null}");
        return puzzleResultData != null;
    }
}
