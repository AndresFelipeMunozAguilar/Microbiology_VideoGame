using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour, IPausable, IPuzzlePausable
{
    Rigidbody2D rb;
    [SerializeField] private float speed;
    Transform objectCollision;
    private Vector2 input;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameManager.GetInstance().SubscribePuzzlePausable(this);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        objectCollision = other.transform;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform == objectCollision)
        {
            objectCollision = null;
        }
    }

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
        this.enabled = false;
    }

    public void PuzzlePauseMe()
    {
        Debug.Log("Soy el MOVIMIENTO del jugador y he sido PAUSADO.");
        this.enabled = false;
    }

    public void PuzzleResumeMe()
    {
        Debug.Log("Soy el MOVIMIENTO del jugador y he sido RESUMIDO.");
        this.enabled = true;
    }

    public void OnDestroy()
    {
        GameManager.GetInstance().UnsubscribePuzzlePausable(this);
    }
}
