using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreTx, PerformanceTx, BestScoreTx;

    public void SendData(int score, int maxScore, string performance,int besScore)
    {
        StartCoroutine(ScoreAnimation(ScoreTx,score, maxScore));
         StartCoroutine(ScoreAnimation(BestScoreTx,besScore, maxScore));
        StartCoroutine(PerformanceAnimation(performance));
    }

    IEnumerator ScoreAnimation(TextMeshProUGUI text,int score, int maxScore)
    {
        for (int i=0; i<(score+1); i++)
        {
            text.SetText(i + "/" + maxScore);
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator PerformanceAnimation(string performance)
    {
        string current = "";

        for (int i = 0; i < performance.Length; i++)
        {
            current += performance[i];
            PerformanceTx.SetText(current);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}