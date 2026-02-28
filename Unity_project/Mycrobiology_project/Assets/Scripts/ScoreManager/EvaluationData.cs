using System;
using System.Collections.Generic;

[Serializable]
public class PuzzleResultData
{
    public string puzzleID;
    public int score;
    public int bestScore;
    public string performance;
}

[Serializable]
public class EvaluationData
{
    public string playerID;
    public int totalScore;
    public string date;
    public List<PuzzleResultData> puzzles;
}