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

    [SerializeField]
    private GameObject ScreenScore;

    private string performanceFinal;

    private Dictionary<string, int> runtimeDict;

    // Este metodo carga los valores de puntuacion configurados para el puzzle.
    private void Awake()
    {
        runtimeDict = new Dictionary<string, int>();

        foreach (var pair in values)
        {
            runtimeDict[pair.key] = pair.value;
        }
    }

    // Este metodo permite devolver el rendimiento final del puzzle.
    public string getPerformance()
    {
        return performanceFinal;
    }

    // Este metodo permite devolver el color asociado al rendimiento.
    public string GetColor()
    {
        return EvaluationSystem.Instance.GetColor();
    }

    // Este metodo permite obtener el valor configurado para una accion.
    public int GetValue(string key)
    {
        return runtimeDict.TryGetValue(key, out int v) ? v : 0;
    }

    // Este metodo suma puntos usando una clave de accion.
    public void AddPoints(string key)
    {
        if (finished) return;

        currentScore += Mathf.Abs(GetValue(key));
        ClampScore();
        CheckAutoEnd();
    }

    // Este metodo suma puntos usando una cantidad directa.
    public void AddPoints(int amount)
    {
        if (finished) return;

        currentScore += Mathf.Abs(amount);
        ClampScore();
        CheckAutoEnd();
    }

    // Este metodo resta puntos usando una clave de accion.
    public void RemovePoints(string key)
    {
        if (finished) return;

        currentScore -= Mathf.Abs(GetValue(key));
        ClampScore();
        CheckAutoEnd();
    }

    // Este metodo resta puntos usando una cantidad directa.
    public void RemovePoints(int amount)
    {
        if (finished) return;

        currentScore -= Mathf.Abs(amount);
        ClampScore();
        CheckAutoEnd();
    }

    // Este metodo permite aplicar una modificacion personalizada al puntaje.
    public void AddCustom(int amount)
    {
        if (finished) return;

        currentScore += amount;
        ClampScore();
        CheckAutoEnd();
    }

    // Este metodo finaliza la evaluacion y envia el resultado al sistema global.
    public void FinishGame(bool victory)
    {
        if (finished) return;

        finished = true;
        SendResultToGlobal();
    }

    // Este metodo permite devolver el puntaje actual del puzzle.
    public int GetCurrentScore()
    {
        return currentScore;
    }

    // Este metodo mantiene el puntaje dentro de los limites definidos.
    private void ClampScore()
    {
        currentScore = Mathf.Clamp(currentScore, minScore, maxScore);
    }

    // Este metodo revisa si el puntaje ya cumple una condicion de cierre.
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

    // Este metodo registra el resultado final en EvaluationSystem.
    private void SendResultToGlobal()
    {
        if (currentScore < 0) currentScore = 0; //mantener el minimo como 0

        int? best = EvaluationSystem.Instance.GetBestScore(puzzleID);

        if (!best.HasValue || best.Value < currentScore)
        {
            best = currentScore;
        }

        int bestScore = best.Value;
        performanceFinal = EvaluationSystem.Instance.RegisterPuzzleResult(puzzleID, currentScore, maxScore, bestScore);

        Debug.Log($"MiniGame {puzzleID} terminó con {currentScore} → {performanceFinal}");
        ShowResults(currentScore, maxScore, bestScore);
    }

    // Este metodo instancia la pantalla de resultados del minijuego.
    public void ShowResults(int score, int maxScore, int bestScore)
    {
        GameObject scoreScreen = Instantiate(ScreenScore);
        scoreScreen.GetComponent<ScoreScreen>().SendData(score, maxScore, performanceFinal, bestScore);
    }
}
