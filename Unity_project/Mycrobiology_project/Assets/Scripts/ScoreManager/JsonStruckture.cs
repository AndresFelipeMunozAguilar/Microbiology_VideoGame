[System.Serializable]
public class PuzzleData
{
    public string puzzleID;
    public int score;
    public string performance;
}

[System.Serializable]
public class PlayerData
{
    public string playerID;
    public int totalScore;
    public string date;
    public PuzzleData[] puzzles;
}