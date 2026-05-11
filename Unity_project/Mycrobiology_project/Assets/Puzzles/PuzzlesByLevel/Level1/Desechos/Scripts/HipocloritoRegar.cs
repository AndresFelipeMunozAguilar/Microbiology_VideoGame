using System.Collections;
using UnityEngine;

public class HipocloritoRegar : AbstractDraggableWorldObject
{
    private PuzzleEvaluation score;
    private gameplayDesechos gamePlay;

    private Vector3 originPos;
    private bool procesando = false;

    [Header("Detección")]
    [SerializeField] private LayerMask desechosMask;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteRegando;

    [Header("Animación")]
    [SerializeField] private float alturaSobreMancha = 0.65f;
    [SerializeField] private float tiempoDesvanecerMancha = 1.5f;
    [SerializeField] private float tiempoExtraRegando = 0.2f;

    public void CreateElement(
        PuzzleEvaluation evaluation,
        gameplayDesechos gameplay
    )
    {
        score = evaluation;
        gamePlay = gameplay;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        SetSpriteNormal();
    }

    protected override void OnDragStarted()
    {
        if(originPos==Vector3.zero)originPos = transform.position;
        if (procesando)
        {
            return;
        }
    }

    protected override void OnDragEnded()
    {
        if (procesando)
        {
            return;
        }

        TryApplyHipoclorito();
    }

    private void TryApplyHipoclorito()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, desechosMask);

        if (hit == null)
        {
            VolverAlOrigen();
            return;
        }

        if (!hit.TryGetComponent(out Desecho desecho))
        {
            VolverAlOrigen();
            return;
        }

        if (!desecho.PuedeLimpiarseConHipoclorito())
        {
            desecho.FalloPorHipocloritoIncorrecto(transform.position);
            VolverAlOrigen();
            return;
        }

        StartCoroutine(RegarMancha(desecho));
    }

    private IEnumerator RegarMancha(Desecho desecho)
    {
        procesando = true;

        if (_canvasGroup != null)
        {
            _canvasGroup.blocksRaycasts = false;
        }

        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null)
        {
            collider.enabled = false;
        }

        Vector3 posicionMancha = desecho.transform.position;

        Vector3 posicionRegado = posicionMancha + new Vector3(1f, alturaSobreMancha, 0f);
        transform.position = posicionRegado;

        SetSpriteRegando();

        yield return StartCoroutine(
            desecho.DesvanecerYCompletarPorHipoclorito(tiempoDesvanecerMancha)
        );

        yield return new WaitForSeconds(tiempoExtraRegando);

        SetSpriteNormal();
        VolverAlOrigen();

        if (collider != null)
        {
            collider.enabled = true;
        }

        if (_canvasGroup != null)
        {
            _canvasGroup.blocksRaycasts = true;
        }

        procesando = false;
    }

    private void VolverAlOrigen()
    {
        transform.position = originPos;
        SetSpriteNormal();
    }

    private void SetSpriteNormal()
    {
        if (spriteRenderer != null && spriteNormal != null)
        {
            spriteRenderer.sprite = spriteNormal;
            spriteRenderer.flipX=false;
        }
    }

    private void SetSpriteRegando()
    {
        if (spriteRenderer != null && spriteRegando != null)
        {
            spriteRenderer.sprite = spriteRegando;
            spriteRenderer.flipX=true;
        }
    }
}