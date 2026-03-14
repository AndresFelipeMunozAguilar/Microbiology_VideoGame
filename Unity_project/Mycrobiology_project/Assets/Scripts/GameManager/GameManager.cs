using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    // WARNING: Este enum debe coincidir con 
    // los nombres y orden de las escenas en 
    // Build Settings, o el sistema de cambio 
    // de escenas no funcionará.
    public enum GameScenes
    {
        DevAndres,
        TestMenu,
        DevBrandon,
        Develop,
        TempGameOver,
        TestPuzzle,
    }

    public bool isGameOver = false;

    public bool isPuzzleActive = false;

    private List<IPuzzlePausable> puzzlePausables;

    // Este delegate funciona gracias al LazyInstantiation
    // de lo contrario, la manera como Unity carga
    // los objetos no lo permitiría.
    //
    // ========= Explicacion técnica (Too Long) =========
    // Debido a que los metodos Awake(), OnEnable() y Start() no 
    // se ejecutan cronologicamente paso por 
    // paso para todos los objetos, sino que se 
    // ejecutan por lotes, por tanto, un objeto 
    // que llame a la instancia de GameManager 
    // puede estar en un lote anterior a la 
    // instanciación de GameManager
    private Action OnGameOver;



    public void Awake()
    {
        Debug.Log("GameManager Awake called");

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            Debug.Log("GameManager DontDestroyOnLoad instance set in Awake");
        }
        else
        {
            Debug.LogWarning($"GameManager instance already exists. Destroying duplicate on GameObject: {this.gameObject.name}");
            Destroy(this.gameObject);
        }


        puzzlePausables = new List<IPuzzlePausable>();

    }


    // Evitar la instanciación externa
    private GameManager() { }


    // Este método viola el principio de una 
    // única fuente de verdad, ya que ese deber 
    // lo tiene el GameManager prefab (Establece 
    // la verdad sobre que compone a un GameManager) 
    // y, en cambio aquí instanciamos un nuevo 
    // GameManager confiando en que tú desarrollador
    // te basarás en este prefab. 
    private static GameManager CreateNewInstance()
    {

        GameObject newGameManager = new GameObject("GameManager");

        // Si hay más componente, añadirlos aquí

        return newGameManager.AddComponent<GameManager>();

    }

    public static GameManager GetInstance()
    {
        if (_instance == null)
        {
            Debug.Log("La instancia estática de GameManager es null, instanciando uno nuevo");

            // Dado que se entra al condicional cuando 
            // _instance == null, puedo garantizar que, 
            // al instanciar el prefab y entrar en el 
            // Awake del GameManager, se asignará la 
            // instancia estática correctamente y se
            //  asignará como DontDestroyOnLoad, evitando 
            // así problemas de acceso a la instancia 
            // desde otros objetos

            _instance = CreateNewInstance();

        }

        return _instance;
    }


    // ===========================================================
    // =========== LÓGICA DEL OBSERVER PUZZLE PAUSABLE ===========
    // ===========================================================
    public void SubscribePuzzlePausable(IPuzzlePausable puzzlePausable)
    {
        puzzlePausables.Add(puzzlePausable);
    }

    public void UnsubscribePuzzlePausable(IPuzzlePausable puzzlePausable)
    {
        puzzlePausables.Remove(puzzlePausable);
    }


    public void PuzzlePauseAll()
    {
        Debug.Log("Pausing all puzzles...");
        Debug.Log($"Puzzle is active: {isPuzzleActive}");

        foreach (IPuzzlePausable puzzlePausable in puzzlePausables)
        {
            puzzlePausable.PuzzlePauseMe();
        }
    }

    public void PuzzleResumeAll()
    {
        Debug.Log("Resuming all puzzles...");
        Debug.Log($"Puzzle is active: {isPuzzleActive}");

        foreach (IPuzzlePausable puzzlePausable in puzzlePausables)
        {
            puzzlePausable.PuzzleResumeMe();
        }
    }

    public void SwitchIsPuzzleActive()
    {
        isPuzzleActive = !isPuzzleActive;
        Debug.Log($"is the puzzle active?: {isPuzzleActive}");
    }


    // =====================================================
    // =========== LÓGICA DEL DELEGATE GAME OVER ===========
    // =====================================================

    // Nos aseguramos de que SÓLO los 
    // IGameOverSubscriber puedan suscribirse 
    // y desuscribirse al evento OnGameOver
    public void SubscribeToGameOver(IGameOverSubscriber subscriber)
    {
        OnGameOver += subscriber.OnGameOver;
    }

    public void UnsubscribeToGameOver(IGameOverSubscriber subscriber)
    {
        OnGameOver -= subscriber.OnGameOver;
    }

    public void GameOver(string reason)
    {
        Debug.Log($"Game Over! Reason: {reason}");
        isGameOver = true;

        // Se dispara la logica de muerte para 
        NotifyGameOverSubscribers();

        LoadScene(GameScenes.TempGameOver);
    }

    public void NotifyGameOverSubscribers()
    {
        OnGameOver?.Invoke();
    }

    public void LoadScene(GameScenes scene)
    {
        SceneManager.LoadScene((int)scene);
    }

}
