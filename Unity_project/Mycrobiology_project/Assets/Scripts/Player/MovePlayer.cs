using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour, IPausable, IPuzzlePausable, IGameOverSubscriber
{
    Rigidbody2D rb;
    [SerializeField] private float speed;
    // Transform objectCollision;
    private Vector2 _input;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameManager.GetInstance().SubscribePuzzlePausable(this);

    }

    public void OnEnable()
    {
        // if (GameManager.GetInstance() == null) Debug.LogError("MovePlayer: GameManager instance es null en OnEnable. Esto no deberia pasar.");
        GameManager.SubscribeToGameOver(this);
    }

    public void OnDisable()
    {
        GameManager.UnsubscribeToGameOver(this);
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

    public void OnDestroy()
    {
        GameManager.GetInstance().UnsubscribePuzzlePausable(this);
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
