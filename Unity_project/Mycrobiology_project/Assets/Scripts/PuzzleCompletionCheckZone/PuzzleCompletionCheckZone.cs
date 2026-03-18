using System;
using UnityEngine;

public class PuzzleCompletionCheckZone : MonoBehaviour
{

    [SerializeField] private string playerTag = "Player";
    [SerializeField] private int _completedPuzzles;
    [SerializeField] private int _totalPuzzles;



    public Action<int, int> OnPlayerGetsClose;
    public Action OnPlayerLeaves;

    public void Start()
    {
        // Preguntar cuantos puzzles totales hay.
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"<color=cyan>PuzzleCompletionCheckZone:</color> Trigger entered by: {other.gameObject.name}");

        if (other.CompareTag(playerTag))
        {
            _completedPuzzles = CalculateCompletedPuzzles();
            // Calcular el total de Puzzles aquí evita
            // condiciones de carrera con el instanciamento 
            // de los puzzles y permite que se añadan 
            // nuevos puzzles dinámicamente
            _totalPuzzles = CalculateTotalPuzzles();


            OnPlayerGetsClose?.Invoke(_completedPuzzles, _totalPuzzles);
            // Si los puzzles completados son menores a los iguales y mayor que cero, mostrar el texto
            // De lo contrario Si es igual al numero total de puzzles, enviar al gameManger que se ganó el juego.    
        }


    }

    public int CalculateCompletedPuzzles()
    {
        // Implementar lógica para preguntarle 
        // al evaluation system cuantos puzzles
        // se han completado
        return _completedPuzzles;
    }

    public int CalculateTotalPuzzles()
    {
        // Implementar lógica para preguntarle 
        // al evaluation system cuantos puzzles
        // se han completado
        return _totalPuzzles;
    }


    public void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"<color=cyan>PuzzleCompletionCheckZone:</color> Trigger exited by: {other.gameObject.name}");

        if (other.CompareTag(playerTag))
        {
            OnPlayerLeaves?.Invoke();
        }
    }
}