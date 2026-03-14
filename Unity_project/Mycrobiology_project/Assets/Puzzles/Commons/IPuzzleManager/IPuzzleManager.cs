// Interfaz para los objetos pausables

public interface IPuzzleManager
{
    // Lógica que inicia el puzzle
    public void StartPuzzle();

    public void CompletePuzzle();

    public int GetScore();

    public PerformanceResult GetPerformanceResult();
}