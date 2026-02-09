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
        ControlsManager.getControls().PickUp.performed += ctr => pickup();
        ControlsManager.getControls().Pickdown.performed += ctr => pickdown();

        GameManager.GetInstance().SubscribePuzzlePausable(this);
    }

    void pickup()
    {
        if (objectCollision) objectCollision.SetParent(this.transform);
    }

    void pickdown()
    {
        transform.GetChild(0).SetParent(null);
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

    void Update()
    {
        input = ControlsManager.getControls().Move.ReadValue<Vector2>();
        if (input.sqrMagnitude > 0.01f)
        {
            Vector2 targetPos = rb.position + input * speed * Time.deltaTime;
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
