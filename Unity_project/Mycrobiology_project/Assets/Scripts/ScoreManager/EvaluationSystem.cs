using UnityEngine;
using System.Collections.Generic;

public class EvaluationSystem : MonoBehaviour
{
    public static EvaluationSystem Instance
    {
        get;
        private set;
    }

    string playerID = "player";
    private List<PuzzleResultData> results = new List<PuzzleResultData>();
    private Dictionary<string, int> puzzleFinalScores = new Dictionary<string, int>();
    private int totalScore = 0;
    private int FinalScore;
    [SerializeField] private int MaxTotalScore = 100;
    private string totalPerformance,ColorPerformance;
    int puzzlesAmount;

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
    void Start()
    {
        playerID = PlayerPrefs.GetString("playerID", "");
    }

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

    public int GetPuzzlesAmount()
    {
        PuzzleManager[] puzzles = Object.FindObjectsByType<PuzzleManager>(FindObjectsSortMode.None);
        puzzlesAmount = puzzles.Length;
        return puzzlesAmount;
    }
    public string GetColor(){
        return ColorPerformance;
    }
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

    public int GetPuzzlesComplete()
    {
        return results.Count;
    }

    public int GetFinalScore() => (totalScore / GetPuzzlesAmount());
    public string GetTotalPerformance() => totalPerformance;

    public int GetPuzzleScore(string puzzleID)
    {
        return puzzleFinalScores.TryGetValue(puzzleID, out int score) ? score : 0;
    }

    private string CalculatePerformance(int score, int maxScore)
    {
        float percentage = (float)score / maxScore * 100f;

        if (percentage >= 90f)
            {
                ColorPerformance = "#2ECC71"; // Sobresaliente
                return "Rendimiento Sobresaliente";
            }
            else if (percentage >= 75f)
            {
                ColorPerformance = "#7ED957"; // Satisfactorio
                return "Rendimiento Satisfactorio";
            }
            else if (percentage >= 60f)
            {
                ColorPerformance = "#F1C40F"; // Aceptable
                return "Rendimiento Aceptable";
            }
            else if (percentage >= 40f)
            {
                ColorPerformance = "#E67E22"; // En Progreso
                return "Rendimiento en Progreso";
            }
            else
            {
                ColorPerformance = "#E74C3C"; // Deficiente
                return "Rendimiento Deficiente";
            }
    }

    public void SaveAllResults(string playerID)
    {
        EvaluationData data = new EvaluationData
        {
            playerID = playerID,
            totalScore = totalScore,
            date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            puzzles = results
        };

        DataManager.Instance.SaveEvaluation(data);
    }
    public int? GetBestScore(string puzzleID)
    {

        PuzzleResultData data = DataManager.Instance.GetPuzzleByID(puzzleID);

        if (data == null)
            return null;

        return data.bestScore;
    }
}