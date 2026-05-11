using System.Collections;
using UnityEngine;

public class Desecho : AbstractDraggableWorldObject
{
    private DesechoElement desecho;
    private PuzzleEvaluation score;
    private gameplayDesechos gamePlay;

    private Vector2 originPos;
    private bool resolved = false;
    private bool draggable = true;

    [SerializeField] private LayerMask zonasMask;

    private SpriteRenderer spriteRenderer;

    public DesechoElement ElementData => desecho;
    public bool IsResolved => resolved;

    public void CreateElement(
        DesechoElement assigned,
        PuzzleEvaluation evaluation,
        gameplayDesechos gameplay
    )
    {
        desecho = assigned;
        score = evaluation;
        gamePlay = gameplay;

        originPos = transform.position;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null && desecho != null)
        {
            spriteRenderer.sprite = desecho.image;
        }

        draggable = desecho.metodoCorrecto != MetodoDescontaminacion.Hipoclorito;

        if (!draggable)
        {
            ConvertirEnManchaFija();
        }

    }

    private void ConvertirEnManchaFija()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        if (_rigidbody != null)
        {
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.angularVelocity = 0;
            _rigidbody.gravityScale = 0;
        }
        enabled = false;
    }

    protected override void OnDragStarted()
    {
        if (!draggable || resolved)
        {
            if (desecho.metodoCorrecto == MetodoDescontaminacion.Hipoclorito)
            {
                gamePlay.CreateFeedback(
                    transform.position,
                    "No se puede mover"
                );
                
            }
            return;
        }
    }

    protected override void OnDragEnded()
    {
        if (!draggable || resolved)
        {
            return;
        }

        bool correctDrop = TryDropOnZone();

        if (!correctDrop && !resolved)
        {
            transform.position = originPos;
        }
    }
    private bool EsErrorMenorEntreAutoclaveYSumergir(MetodoDescontaminacion metodoZona)
    {
        return
            desecho.metodoCorrecto == MetodoDescontaminacion.Autoclave &&
            metodoZona == MetodoDescontaminacion.Sumergirse
            ||
            desecho.metodoCorrecto == MetodoDescontaminacion.Sumergirse &&
            metodoZona == MetodoDescontaminacion.Autoclave;
    }
    private void WrongElement(string message)
    {
        score.RemovePoints("WrongElement");
        gamePlay.NoPerfect();

        if (string.IsNullOrEmpty(message))
        {
            message = gamePlay.GetFeedbackIncorrectoGeneral();
        }

        gamePlay.CreateFeedback(transform.position, message);
    }

    private void WrongDrop(string message)
    {
        score.RemovePoints("WrongDrop");
        gamePlay.NoPerfect();

        if (string.IsNullOrEmpty(message))
        {
            message = gamePlay.GetFeedbackIncorrectoGeneral();
        }

        gamePlay.CreateFeedback(transform.position, message);
    }
    private bool TryDropOnZone()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, zonasMask);

        if (hit == null)
        {
            return false;
        }

        if (!hit.TryGetComponent(out ZonaDesecho zona))
        {
            originPos = transform.position;
            return false;
        }

        if (desecho.metodoCorrecto == MetodoDescontaminacion.Hipoclorito)
        {
            WrongDrop(
                "Incorrecto"
            );

            return false;
        }

        if (zona.GetMetodoZona() == desecho.metodoCorrecto)
        {
            if (!zona.PuedeRecibir())
            {
                score.RemovePoints("Spam");
                gamePlay.CreateFeedback(transform.position, zona.GetFeedbackOcupado());
                return false;
            }

            zona.ActivarZona();
            if (desecho.metodoCorrecto == MetodoDescontaminacion.Sumergirse)
            {
                transform.position=zona.GetFeedbackPosition();
                StartCoroutine(DesvanecerYCompletarPorHipoclorito(zona.tiempoOcupado));
            }
            else{
                CorrectDrop();
            }
            
            return true;
        }
        if (EsErrorMenorEntreAutoclaveYSumergir(zona.GetMetodoZona()))
        {
            WrongDrop(GetWrongFeedback());
        }
        else
        {
            WrongElement(GetWrongFeedback());
        }

        return false;
    }

    private void CorrectDrop()
    {
        score.AddPoints("FinishElement");
        Destroy(gameObject);
    }
    private string GetWrongFeedback()
    {
        if (!string.IsNullOrEmpty(desecho.feedbackIncorrecto))
        {
            return desecho.feedbackIncorrecto;
        }

        return gamePlay.GetFeedbackIncorrectoGeneral();
    }
    public bool PuedeLimpiarseConHipoclorito()
    {
        if (resolved)
        {
            return false;
        }

        if (desecho == null)
        {
            return false;
        }

        return desecho.metodoCorrecto == MetodoDescontaminacion.Hipoclorito;
    }

    public void FalloPorHipocloritoIncorrecto(Vector2 feedbackPosition)
    {
        if (resolved)
        {
            return;
        }

        score.RemovePoints("WrongDrop");
        gamePlay.NoPerfect();

        gamePlay.CreateFeedback(
            feedbackPosition,
            "Incorrecto"
        );
    }

    public IEnumerator DesvanecerYCompletarPorHipoclorito(float duration)
    {
        draggable=false;
        if (resolved)
        {
            yield break;
        }

        if (desecho == null)
        {
            yield break;
        }

        resolved = true;

        Vector2 feedbackPosition = transform.position;

        if (spriteRenderer != null)
        {
            Color initialColor = spriteRenderer.color;
            Color targetColor = initialColor;
            targetColor.a = 0f;

            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / duration);

                spriteRenderer.color = Color.Lerp(initialColor, targetColor, t);

                yield return null;
            }

            spriteRenderer.color = targetColor;
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }

        score.AddPoints("FinishElement");
        if (desecho.metodoCorrecto == MetodoDescontaminacion.Hipoclorito)
        {
            gamePlay.CorrectFeedback(feedbackPosition);
            Invoke("CorrectElement",1f);
        }
    }

    void CorrectElement()
    {
        Debug.Log("[Desechos] completado");
        gamePlay.addFinishElement();
        Destroy(gameObject);
    }
}