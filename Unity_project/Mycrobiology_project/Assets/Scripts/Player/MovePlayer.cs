using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour, IPausable, IPuzzlePausable, IGameOverSubscriber
{
    Rigidbody2D rb;
    [SerializeField] private float speed;
    // Transform objectCollision;
    private Vector2 _input;

    [SerializeField]
    private GameManager _gameManager;

    public void Awake()
    {

        _gameManager = GameManager.GetInstance();

        if (_gameManager == null)
        {
            Debug.Log($"MovePlayer: GameManager instance not found: Is null ");
        }
        else
        {
            Debug.Log($"MovePlayer: GameManager instance found: {_gameManager.gameObject.name}");
        }

    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _gameManager.SubscribePuzzlePausable(this);
    }

    public void OnEnable()
    {
        // if (GameManager.GetInstance() == null) Debug.LogError("MovePlayer: GameManager instance es null en OnEnable. Esto no deberia pasar.");
        _gameManager.SubscribeToGameOver(this);
    }

    public void OnDisable()
    {
        _gameManager.UnsubscribeToGameOver(this);
    }


    public void Update()
    {
        // 1. Capturamos el input en cada frame de renderizado (Máxima precisión)
        _input = ControlsManager.getControls().Move.ReadValue<Vector2>();
    }
    public void FixedUpdate()
    {
        // 2. Aplicamos la física basándonos en el último input capturado
        // Usamos sqrMagnitude por eficiencia (evita el cálculo de raíz cuadrada)
        if (_input.sqrMagnitude > 0.01f)
        {
            // Nota: En FixedUpdate, multiplicamos por Time.fixedDeltaTime
            Vector2 targetPos = rb.position + _input * speed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);
        }
    }

    public void Pause()
    {
        DeactivateScript();
    }

    public void PuzzlePauseMe()
    {
        DeactivateScript();
    }

    public void PuzzleResumeMe()
    {
        ActivateScript();
    }

    public void OnGameOver()
    {
        Debug.Log("MovePlayer: I have received the GameOver event. Executing logic");
        DeactivateScript();

    }

    public void DeactivateScript()
    {
        Debug.Log("MovePlayer: He sido PAUSADO.");
        this.enabled = false;
    }
    public void ActivateScript()
    {
        Debug.Log("MovePlayer: He sido REANUDADO.");
        this.enabled = true;
    }
}
