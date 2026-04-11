using System;
using UnityEngine;

public class PuzzleCompletionCheckZone : MonoBehaviour, IGameOverSubscriber, IPuzzlePausable
{

    [Header("Configuración de Detección")]
    [Tooltip("Tag que debe tener el objeto del jugador para ser reconocido.")]
    [SerializeField] private string playerTag = "Player";

    [Header("Estado de los Puzzles")]
    [Tooltip("Cantidad actual de puzzles resueltos.")]
    [SerializeField] private int _completedPuzzles;
    [Tooltip("Cantidad total de puzzles en el nivel.")]
    [SerializeField] private int _totalPuzzles;

    [Header("Referencias de Componentes")]
    [SerializeField] private Collider2D _collider;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] SpriteRenderer door;
    [SerializeField] Sprite openDoor;
    [Header("Eventos de Interacción")]
    // Nota: Los Action no se muestran en el Inspector por defecto, 
    // pero el Header ayuda a separar el código visualmente.
    public Action<int, int> OnPlayerGetsClose;
    public Action OnPlayerLeaves;


    public void Awake()
    {
        _gameManager = GameManager.GetInstance();
    }

    public void OnEnable()
    {
        _gameManager.SubscribeToGameOver(this);
    }

    public void Start()
    {
        _gameManager.SubscribePuzzlePausable(this);
    }

    public void OnDisable()
    {
        _gameManager.UnsubscribeToGameOver(this);
    }

    public void OnDestroy()
    {
        _gameManager.UnsubscribePuzzlePausable(this);
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

        if (_completedPuzzles >= _totalPuzzles && _totalPuzzles > 0){
             GameManager.GetInstance().Victory();
             door.sprite=openDoor;
        }
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
        Debug.Log("<color=cyan>PuzzleCompletionCheckZone:</color> Fui pausado OnGameOver");
        this.enabled = false;
    }

    public void PuzzlePauseMe()
    {
        Debug.Log($"<color=cyan>PuzzleCompletionCheckZone:</color> Yo y el collider fuimos PuzzlePausado");
        _collider.enabled = false;
        this.enabled = false;
    }

    public void PuzzleResumeMe()
    {
        Debug.Log($"<color=cyan>PuzzleCompletionCheckZone:</color> Yo y el collider fuimos PuzzleResumidos");
        _collider.enabled = true;
        this.enabled = true;
    }
}