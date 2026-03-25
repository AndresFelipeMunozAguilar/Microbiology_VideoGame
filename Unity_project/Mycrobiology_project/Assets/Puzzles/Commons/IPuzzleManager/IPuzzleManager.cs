// Interfaz para los objetos pausables

public interface IPuzzleManager
{
    // Lógica que inicia el puzzle
    public void StartPuzzle();

    public void CompletePuzzle(bool didPlayerWin);

    public int GetScore();

    public PerformanceResult GetPerformanceResult();
}