using UnityEngine;
using System.Collections.Generic;

public class EvaluationSystem : MonoBehaviour
{
    public static EvaluationSystem Instance;
    string playerID = "player";
    private List<PuzzleResultData> results = new List<PuzzleResultData>();
    private Dictionary<string, int> puzzleFinalScores = new Dictionary<string, int>();
    private int totalScore = 0;
    private int FinalScore;
    [SerializeField]private int MaxTotalScore=100;
    private string totalPerformance;
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

    public string RegisterPuzzleResult(string puzzleID, int finalScore, int maxScore, int bestScore)
    {
        Debug.Log($"EvaluationSystem: Registrando el resultado del puzzle:\n PuzzleID: {puzzleID}\n FinalScore: {finalScore}\n maxScore: {maxScore}\n bestScore: {bestScore}");
        if (puzzleFinalScores.ContainsKey(puzzleID))

        {
            Debug.Log($"EvaluationSystem: Se encontró la key {puzzleID} en los puzzleFinalScores");
            return CalculatePerformance(finalScore, maxScore);
        }

        Debug.Log($"EvaluationSystem: Añadiendo puntajes del puzzle {puzzleID} a los resultados finales");
        puzzleFinalScores.Add(puzzleID, finalScore);
        totalScore += finalScore;
        totalPerformance = CalculatePerformance(GetFinalScore(), MaxTotalScore);

        Debug.Log($"EvaluationSystem: Calculando el performance del puzzle {puzzleID} con finalScore: {finalScore} y maxScore: {maxScore}");
        string performance = CalculatePerformance(finalScore, maxScore);
        Debug.Log("Final Performance"+performance);
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
        if(puzzlesAmount == 0)
        {
            PuzzleManager[] puzzles = Object.FindObjectsByType<PuzzleManager>(FindObjectsSortMode.None);
            int total = puzzles.Length;
        }
        return puzzlesAmount;
    }
    public int GetFinalScore() => (totalScore/GetPuzzlesAmount());
    public string GetTotalPerformance() => totalPerformance;

    public int GetPuzzleScore(string puzzleID)
    {
        return puzzleFinalScores.TryGetValue(puzzleID, out int score) ? score : 0;
    }

    private string CalculatePerformance(int score, int maxScore)
    {
        float percentage = (float)score / maxScore * 100f;

        if (percentage >= 90f)
            return "Desempeño Óptimo";
        else if (percentage >= 75f)
            return "Desempeño Competente";
        else if (percentage >= 60f)
            return "Desempeño Básico";
        else if (percentage >= 40f)
            return "Desempeño en Desarrollo";
        else
            return "Desempeño Crítico";
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

        return data.score;
    }
}