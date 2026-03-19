using System;
using UnityEngine;

public class PuzzleCompletionCheckZone : MonoBehaviour, IGameOverSubscriber
{

    [SerializeField] private string playerTag = "Player";
    [SerializeField] private int _completedPuzzles;
    [SerializeField] private int _totalPuzzles;



    public Action<int, int> OnPlayerGetsClose;
    public Action OnPlayerLeaves;

    public void OnEnable()
    {
        GameManager.GetInstance().SubscribeToGameOver(this);
    }

    public void OnDisable()
    {
        GameManager.GetInstance().UnsubscribeToGameOver(this);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"<color=cyan>PuzzleCompletionCheckZone:</color> Trigger entered by: {other.gameObject.name}");

        if (!other.CompareTag(playerTag)) return;

        _completedPuzzles = CalculateCompletedPuzzles();
        // Calcular el total de Puzzles aquí evita
        // condiciones de carrera con el instanciamento 
        // de los puzzles y permite que se añadan 
        // nuevos puzzles dinámicamente
        _totalPuzzles = CalculateTotalPuzzles();


        OnPlayerGetsClose?.Invoke(_completedPuzzles, _totalPuzzles);

        if (_completedPuzzles >= _totalPuzzles && _totalPuzzles > 0) GameManager.GetInstance().Victory();
    }

    public int CalculateCompletedPuzzles()
    {
        // Implementar lógica para preguntarle 
        // al evaluation system cuantos puzzles
        // se han completado
        int noOfCompletedPuzzles = EvaluationSystem.Instance.GetPuzzlesComplete();
        Debug.Log($"<color=cyan>PuzzleCompletionCheckZone:</color> Se encontraron {noOfCompletedPuzzles} puzzles completados en escena");
        return noOfCompletedPuzzles;
    }

    public int CalculateTotalPuzzles()
    {
        // Implementar lógica para preguntarle 
        // al evaluation system cuantos puzzles
        // se han completado
        int noOfActivePuzzles = EvaluationSystem.Instance.GetPuzzlesAmount();
        Debug.Log($"<color=cyan>PuzzleCompletionCheckZone:</color> Se encontraron {noOfActivePuzzles} puzzles activos en escena");
        return noOfActivePuzzles;
    }


    public void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"<color=cyan>PuzzleCompletionCheckZone:</color> Trigger exited by: {other.gameObject.name}");

        if (other.CompareTag(playerTag))
        {
            OnPlayerLeaves?.Invoke();
        }
    }

    public void OnGameOver()
    {
        Debug.Log("PuzzleCompletionCheckZone: Fui pausado OnGameOver");
        this.enabled = false;
    }
}