using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowScoreFinal : MonoBehaviour
{
    [SerializeField] GameObject Modulo;
    [SerializeField] RectTransform content;
    [SerializeField] TextMeshProUGUI messageStatus;
    [SerializeField] GameObject btReenviar;

    private void Start() {
        PrintFinalFeedback();
        UserRegister.Instance.UploadEvaluationFromFile2(messageStatus, btReenviar);
        btReenviar.SetActive(false);
    }
    public void Reenviar()
    {
        UserRegister.Instance.UploadEvaluationFromFile2(messageStatus, btReenviar);
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
