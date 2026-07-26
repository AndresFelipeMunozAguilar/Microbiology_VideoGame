using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowScoreFinal : MonoBehaviour
{
    [SerializeField] GameObject Modulo;
    [SerializeField] RectTransform content;

    private void Start() {
        PrintFinalFeedback();
    }

    void PrintFinalFeedback()
    {
        List<PuzzleResultData> resultados = EvaluationSystem.Instance.getPuzzles();
        if (resultados.Count<=0)
        {
            GameObject modulo = Instantiate(Modulo,content);
            modulo.GetComponent<InitModule>().Init("No se encontrarón resultados");
            return;
        }
        foreach(PuzzleResultData puzzle in resultados)
        {
            GameObject modulo = Instantiate(Modulo,content);
            modulo.GetComponent<InitModule>().Init(puzzle.puzzleID,"Puntaje: " + puzzle.score,"Performance: " + puzzle.performance,"Mejor Puntaje: " + puzzle.bestScore);
        }

    }
}
