using UnityEngine;

public class Desecho : AbstractDraggableWorldObject
{
    private DesechoElement desecho;
    private PuzzleEvaluation score;
    private gameplayDesechos gamePlay;

    private Vector3 originPos;
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

        /*
         * Importante:
         * Desactivamos este MonoBehaviour para que Unity no ejecute
         * los eventos de drag heredados de AbstractDraggableWorldObject.
         * El Collider2D queda activo, entonces HipocloritoRegar todavía
         * puede detectar este objeto con Physics2D.OverlapPoint.
         */
        enabled = false;
    }

    protected override void OnDragStarted()
    {
        if (!draggable || resolved)
        {
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

    private bool TryDropOnZone()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, zonasMask);

        if (hit == null)
        {
            return false;
        }

        if (!hit.TryGetComponent(out ZonaDesecho zona))
        {
            return false;
        }

        if (desecho.metodoCorrecto == MetodoDescontaminacion.Hipoclorito)
        {
            WrongDrop(
                "Este tipo de contaminación no se arrastra. Debes aplicar hipoclorito directamente sobre la mancha."
            );

            return false;
        }

        if (zona.GetMetodoZona() == desecho.metodoCorrecto)
        {
            CorrectDrop(zona.GetFeedbackPosition());
            return true;
        }

        WrongDrop(GetWrongFeedback());
        return false;
    }

    public bool AplicarHipocloritoDirecto(Vector2 feedbackPosition)
    {
        if (resolved)
        {
            return false;
        }

        if (desecho == null)
        {
            return false;
        }

        if (desecho.metodoCorrecto != MetodoDescontaminacion.Hipoclorito)
        {
            WrongDrop(
                "Incorrecto. El hipoclorito directo se aplica sobre derrames, manchas o superficies contaminadas."
            );

            return false;
        }

        CorrectDrop(feedbackPosition);
        return true;
    }

    private void CorrectDrop(Vector2 feedbackPosition)
    {
        if (resolved)
        {
            return;
        }

        resolved = true;

        score.AddPoints("FinishElement");

        string message = string.IsNullOrEmpty(desecho.feedbackCorrecto)
            ? GetDefaultCorrectFeedback()
            : desecho.feedbackCorrecto;

        gamePlay.CreateFeedback(feedbackPosition, message);
        gamePlay.CorrectFeedback(feedbackPosition);
        gamePlay.addFinishElement();

        Destroy(gameObject);
    }

    private void WrongDrop(string message)
    {
        score.RemovePoints("Spam");
        gamePlay.NoPerfect();

        if (string.IsNullOrEmpty(message))
        {
            message = gamePlay.GetFeedbackIncorrectoGeneral();
        }

        gamePlay.CreateFeedback(transform.position, message);
    }

    private string GetWrongFeedback()
    {
        if (!string.IsNullOrEmpty(desecho.feedbackIncorrecto))
        {
            return desecho.feedbackIncorrecto;
        }

        return gamePlay.GetFeedbackIncorrectoGeneral();
    }

    private string GetDefaultCorrectFeedback()
    {
        switch (desecho.metodoCorrecto)
        {
            case MetodoDescontaminacion.Autoclave:
                return "Correcto. Este residuo sólido contaminado debe descontaminarse mediante autoclave.";

            case MetodoDescontaminacion.Hipoclorito:
                return "Correcto. Los derrames, manchas y superficies contaminadas deben desinfectarse aplicando hipoclorito.";

            case MetodoDescontaminacion.Sumergirse:
                return "Correcto. Este objeto reutilizable contaminado debe sumergirse en hipoclorito.";

            default:
                return "Correcto.";
        }
    }
}