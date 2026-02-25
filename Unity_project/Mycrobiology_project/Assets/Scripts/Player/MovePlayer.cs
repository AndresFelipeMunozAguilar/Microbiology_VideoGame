using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour, IPausable, IPuzzlePausable, IGameOverSubscriber
{
    Rigidbody2D rb;
    [SerializeField] private float speed;
    // Transform objectCollision;
    private Vector2 input;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameManager.GetInstance().SubscribePuzzlePausable(this);

    }

    public void OnEnable()
    {
        GameManager.GetInstance().OnGameOver += OnGameOver;
    }

    public void OnDisable()
    {
        GameManager.GetInstance().OnGameOver -= OnGameOver;
    }

    // void OnCollisionEnter2D(Collision2D other)
    // {
    //     objectCollision = other.transform;
    // }

    // void OnCollisionExit2D(Collision2D other)
    // {
    //     if (other.transform == objectCollision)
    //     {
    //         objectCollision = null;
    //     }
    // }

    void FixedUpdate()
    {
        input = ControlsManager.getControls().Move.ReadValue<Vector2>();
        if (input.sqrMagnitude > 0.01f)
        {
            Vector2 targetPos = rb.position + input * speed * Time.fixedDeltaTime;
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
