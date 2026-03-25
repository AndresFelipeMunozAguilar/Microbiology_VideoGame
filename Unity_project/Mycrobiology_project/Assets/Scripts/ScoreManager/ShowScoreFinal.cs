using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowScoreFinal : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TittleTx,TextTx;
    [SerializeField] RectTransform PosColumn1;
    [SerializeField] float EspaciadoGrande,EspaciadoPequeño;

    private void Start() {
        PrintFinalFeedback();
    }

    void PrintFinalFeedback()
    {
        List<PuzzleResultData> resultados = EvaluationSystem.Instance.getPuzzles();
        Vector2 PosTx = PosColumn1.position;
        if (resultados.Count<=0)
        {
            TextMeshProUGUI title= Instantiate(TittleTx,PosColumn1);
            title.rectTransform.position = PosTx;
            title.text= "No se encontrarón resultados";
            return;
        }
        foreach(PuzzleResultData puzzle in resultados)
        {
            TextMeshProUGUI title= Instantiate(TittleTx,PosColumn1);
            title.rectTransform.position = PosTx;
            title.text= puzzle.puzzleID;

            PosTx.y -= EspaciadoPequeño;
            TextMeshProUGUI text= Instantiate(TextTx,PosColumn1);
            text.rectTransform.position = PosTx;
            text.text= "Puntaje: " + puzzle.score;

            PosTx.y -= EspaciadoPequeño;
            TextMeshProUGUI text2= Instantiate(TextTx,PosColumn1);
            text2.rectTransform.position = PosTx;
            text2.text= "Performance: " + puzzle.performance;

            PosTx.y -= EspaciadoPequeño;
            TextMeshProUGUI text3= Instantiate(TextTx,PosColumn1);
            text3.rectTransform.position = PosTx;
            text3.text= "Mejor Puntaje: " + puzzle.bestScore;

            PosTx.y -= EspaciadoGrande; 
        }

    }
}
