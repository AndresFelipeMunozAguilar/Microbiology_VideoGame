// Interfaz para los objetos pausables

public interface IPuzzleManager
{
    // Lógica que inicia el puzzle
    public void StartPuzzle();

    public void CompletePuzzle();

    public float GetScore();

    public PerformanceResult GetPerformanceResult();
}