using System.Collections.Generic;
using UnityEngine;

public class EvaluationSystem : MonoBehaviour
{
    public static EvaluationSystem Instance
    {
        get;
        private set;
    }

    private string playerID = "player";
    private List<PuzzleResultData> results = new List<PuzzleResultData>();
    private Dictionary<string, int> puzzleFinalScores = new Dictionary<string, int>();
    private int totalScore = 0;
    private int finalAverageScore;

    [SerializeField] private int MaxTotalScore = 100;

    private string totalPerformance;
    private string colorPerformance;
    private int puzzlesAmount;

    // Este metodo mantiene una sola instancia del sistema de evaluacion.
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

    // Este metodo carga el identificador del jugador activo.
    private void Start()
    {
        playerID = PlayerPrefs.GetString("playerID", "");
    }

    // Este metodo registra o actualiza el resultado de un puzzle.
    public string RegisterPuzzleResult(string puzzleID, int finalScore, int maxScore, int bestScore)
    {
        Debug.Log($"EvaluationSystem: Registrando el resultado del puzzle:\n PuzzleID: {puzzleID}\n FinalScore: {finalScore}\n maxScore: {maxScore}\n bestScore: {bestScore}");

        if (puzzleFinalScores.ContainsKey(puzzleID))
        {
            int oldScore = puzzleFinalScores[puzzleID];
            totalScore -= oldScore;

            puzzleFinalScores[puzzleID] = finalScore;
            totalScore += finalScore;

            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].puzzleID == puzzleID)
                {
                    results[i].score = finalScore;
                    results[i].bestScore = Mathf.Max(results[i].bestScore, bestScore);
                    results[i].performance = CalculatePerformance(finalScore, maxScore);
                    SaveAllResults(playerID);
                    return results[i].performance;
                }
            }
        }

        Debug.Log($"EvaluationSystem: Añadiendo puntajes del puzzle {puzzleID} a los resultados finales");
        puzzleFinalScores.Add(puzzleID, finalScore);
        totalScore += finalScore;
        totalPerformance = CalculatePerformance(GetFinalScore(), MaxTotalScore);

        Debug.Log($"EvaluationSystem: Calculando el performance del puzzle {puzzleID} con finalScore: {finalScore} y maxScore: {maxScore}");
        string performance = CalculatePerformance(finalScore, maxScore);
        Debug.Log("Final Performance" + performance);
        results.Add(new PuzzleResultData
        {
            puzzleID = puzzleID,
            score = finalScore,
            bestScore = bestScore,
            performance = performance
        });

        Debug.Log($"Puzzle {puzzleID} registrado con {finalScore} puntos → {performance}");
        SaveAllResults(playerID);

        return performance;
    }

    // Este metodo limpia el progreso guardado en memoria para una nueva partida.
    public void ResetProgress()
    {
        results.Clear();
        puzzleFinalScores.Clear();
        totalScore = 0;
        finalAverageScore = 0;
        totalPerformance = string.Empty;
        puzzlesAmount = 0;
    }

    // Este metodo cuenta los puzzles disponibles en la escena actual.
    public int GetPuzzlesAmount()
    {
        PuzzleManager[] puzzles = Object.FindObjectsByType<PuzzleManager>(FindObjectsSortMode.None);
        puzzlesAmount = puzzles.Length;
        return puzzlesAmount;
    }

    // Este metodo permite devolver el color del rendimiento calculado.
    public string GetColor()
    {
        return colorPerformance;
    }

    // Este metodo permite devolver la lista de resultados registrados.
    public List<PuzzleResultData> getPuzzles()
    {
        if (results != null)
        {
            return results;
        }
        else
        {
            return new List<PuzzleResultData>();
        }
    }

    // Este metodo permite devolver la cantidad de puzzles completados.
    public int GetPuzzlesComplete()
    {
        return results.Count;
    }

    // Este metodo calcula el puntaje final promedio del jugador.
    public int GetFinalScore()
    {
        int totalPuzzles = GetPuzzlesAmount();

        if (totalPuzzles == 0) return 0;

        finalAverageScore = totalScore / totalPuzzles;
        return finalAverageScore;
    }

    // Este metodo permite devolver el rendimiento general calculado.
    public string GetTotalPerformance() => totalPerformance;

    // Este metodo permite consultar el puntaje final de un puzzle especifico.
    public int GetPuzzleScore(string puzzleID)
    {
        return puzzleFinalScores.TryGetValue(puzzleID, out int score) ? score : 0;
    }

    // Este metodo convierte el puntaje en una categoria de rendimiento.
    private string CalculatePerformance(int score, int maxScore)
    {
        float percentage = (float)score / maxScore * 100f;

        if (percentage >= 90f)
        {
            colorPerformance = "#2ECC71"; // Sobresaliente
            return "Rendimiento Sobresaliente";
        }
        else if (percentage >= 75f)
        {
            colorPerformance = "#7ED957"; // Satisfactorio
            return "Rendimiento Satisfactorio";
        }
        else if (percentage >= 60f)
        {
            colorPerformance = "#F1C40F"; // Aceptable
            return "Rendimiento Aceptable";
        }
        else if (percentage >= 40f)
        {
            colorPerformance = "#E67E22"; // En Progreso
            return "Rendimiento en Progreso";
        }
        else
        {
            colorPerformance = "#E74C3C"; // Deficiente
            return "Rendimiento Deficiente";
        }
    }

    // Este metodo guarda todos los resultados locales del jugador.
    public void SaveAllResults(string playerID)
    {
        EvaluationData data = new EvaluationData
        {
            playerID = playerID,
            totalScore = totalScore,
            date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            puzzles = results
        };

        DataManager.Instance.SaveEvaluation(data, playerID);
    }

    // Este metodo consulta el mejor puntaje guardado para un puzzle.
    public int? GetBestScore(string puzzleID)
    {
        PuzzleResultData data = DataManager.Instance.GetPuzzleByID(puzzleID);

        if (data == null)
            return null;

        return data.bestScore;
    }
}
