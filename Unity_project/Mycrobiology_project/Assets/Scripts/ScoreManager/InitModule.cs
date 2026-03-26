using System;
using TMPro;
using UnityEngine;

public class InitModule : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title,score,performance,bestScore;

    public void Init(String txTitle = "",String txScore = "",String txPerformance = "" ,String txBestScore = "")
    {
        title.text=txTitle;
        score.text=txScore;
        performance.text=txPerformance;
        bestScore.text=txBestScore;
    }


}
