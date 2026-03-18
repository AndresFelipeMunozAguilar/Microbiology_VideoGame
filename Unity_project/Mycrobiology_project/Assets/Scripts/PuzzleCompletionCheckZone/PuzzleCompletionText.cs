using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;

public class PuzzleCompletionText : MonoBehaviour
{
    // Se usa TextMeshPro porque el texto no aparece en la UI
    [SerializeField] private TextMeshPro _textMeshPro;

    [SerializeField] private PuzzleCompletionCheckZone _puzzleCompletionCheckZone;

    public void OnEnable()
    {
        if (_puzzleCompletionCheckZone == null)
        {
            Debug.LogError("PuzzleCompletionText: PuzzleCompletionCheckZone reference is null. Please assign it in the inspector.");
            return;
        }

        _puzzleCompletionCheckZone.OnPuzzleCompletionStatusChanged += UpdatePuzzleCompletionText;
    }

    public void OnDisable()
    {
        _puzzleCompletionCheckZone.OnPuzzleCompletionStatusChanged -= UpdatePuzzleCompletionText;
    }

    public void UpdatePuzzleCompletionText(int completedPuzzles, int totalPuzzles)
    {
        string completionMessage = GetCompletionMessage(completedPuzzles, totalPuzzles);
        _textMeshPro.SetText(completionMessage);
    }

    public string GetCompletionMessage(int completedPuzzles, int totalPuzzles)
    {
        if (totalPuzzles <= 0) return "Espera... ¿Donde están los puzzles?";

        // Si hay puzzles, empecemos a contarlos!!
        if (completedPuzzles >= 0 && completedPuzzles < totalPuzzles) return $"Puzzles completados: {completedPuzzles}/{totalPuzzles}";
        if (completedPuzzles >= totalPuzzles) return "¡Has completado todos los puzzles!";

        return "Caso no especificado. Numero invalido";
    }

}