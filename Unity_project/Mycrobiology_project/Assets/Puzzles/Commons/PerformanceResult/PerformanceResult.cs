using UnityEngine;

public class PerformanceResult : MonoBehaviour
{
    public int numericScore = 0;
    public string qualitativeResult = "";

    // Calcula un puntaje numérico 
    public void CalculateNumericScore()
    {
        // Por defecto no hay texto de muestra: mantenemos el score tal cual o lo inicializamos
        numericScore = Mathf.Clamp(numericScore, 0, 100);
    }

    public void SetQualitativeResult()
    {
        if (numericScore >= 100)
            qualitativeResult = "Perfecto";
        else if (numericScore >= 85)
            qualitativeResult = "Excelente";
        else if (numericScore >= 70)
            qualitativeResult = "Bueno";
        else if (numericScore >= 50)
            qualitativeResult = "Regular";
        else
            qualitativeResult = "Insuficiente";
    }

    // Retorna una representación en cadena del resultado
    public override string ToString()
    {
        return $"numericScore: {numericScore}, qualitativeResult: {qualitativeResult}";
    }
}
