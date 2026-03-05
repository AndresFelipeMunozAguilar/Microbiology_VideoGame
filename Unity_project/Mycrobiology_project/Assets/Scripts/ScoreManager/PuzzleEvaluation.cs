using System.Collections.Generic;
using UnityEngine;

public class PuzzleEvaluation : MonoBehaviour
{
    [Header("Config")]
    public string puzzleID;
    public int maxScore = 100;
    public int minScore = -50;
    public int winScoreThreshold = 100;
    public int loseScoreThreshold = -30;

    private int currentScore = 0;
    private bool finished = false;

    public List<AcationKey> values;
    [SerializeField] GameObject ScreenScore;

    private Dictionary<string, int> runtimeDict;

    void Awake()
    {
        runtimeDict = new Dictionary<string, int>();

        foreach (var pair in values)
            runtimeDict[pair.key] = pair.value;
    }

    public int GetValue(string key)
    {
        return runtimeDict.TryGetValue(key, out int v) ? v : 0;
    }
    public void AddPoints(string key)
    {
        if (finished) return;

        currentScore += Mathf.Abs(GetValue(key));
        ClampScore();
        CheckAutoEnd();
    }
    public void AddPoints(int amount)
    {
        if (finished) return;

        currentScore += Mathf.Abs(amount);
        ClampScore();
        CheckAutoEnd();
    }
    public void RemovePoints(string key)
    {
        if (finished) return;

        currentScore -= Mathf.Abs(GetValue(key));
        ClampScore();
        CheckAutoEnd();
    }
    public void RemovePoints(int amount)
    {
        if (finished) return;

        currentScore -= Mathf.Abs(amount);
        ClampScore();
        CheckAutoEnd();
    }

    public void AddCustom(int amount)
    {
        if (finished) return;

        currentScore += amount;
        ClampScore();
        CheckAutoEnd();
    }

    public void FinishGame(bool victory)
    {
        if (finished) return;

        finished = true;
        SendResultToGlobal();
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }


    private void ClampScore()
    {
        currentScore = Mathf.Clamp(currentScore, minScore, maxScore);
    }

    private void CheckAutoEnd()
    {
        if (currentScore >= winScoreThreshold)
        {
            FinishGame(true);
        }
        else if (currentScore <= loseScoreThreshold)
        {
            FinishGame(false);
        }
    }

    private void SendResultToGlobal()
    {
        if(currentScore<0)currentScore=0; //mantener el minimo como 0
        int? best = EvaluationSystem.Instance.GetBestScore(puzzleID);
        if ((best.HasValue && best< currentScore) || !best.HasValue)
        {
            best=currentScore;
        }
        int bestScore = (int)best;
        string evaluation = EvaluationSystem.Instance.RegisterPuzzleResult(puzzleID, currentScore, maxScore,bestScore);
        Debug.Log($"MiniGame {puzzleID} terminó con {currentScore} → {evaluation}");
        ShowResults(currentScore,maxScore,evaluation,bestScore);
    }
    public void ShowResults(int Score,int MaxScore, string Performance,int bestScore)
    {
        GameObject scoreScreen = Instantiate(ScreenScore);
        scoreScreen.GetComponent<ScoreScreen>().SendData(Score,MaxScore,Performance,bestScore);
    }
}