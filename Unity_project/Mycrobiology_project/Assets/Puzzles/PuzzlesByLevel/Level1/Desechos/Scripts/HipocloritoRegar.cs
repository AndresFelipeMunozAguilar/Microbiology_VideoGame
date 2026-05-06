using UnityEngine;

public class HipocloritoRegar : AbstractDraggableWorldObject
{
    private PuzzleEvaluation score;
    private gameplayDesechos gamePlay;

    private Vector3 originPos;

    [Header("Detección")]
    [SerializeField] private LayerMask desechosMask;

    public void CreateElement(
        PuzzleEvaluation evaluation,
        gameplayDesechos gameplay
    )
    {
        score = evaluation;
        gamePlay = gameplay;
        originPos = transform.position;
    }

    protected override void OnDragStarted()
    {
        // No necesita lógica especial al iniciar.
    }

    protected override void OnDragEnded()
    {
        TryApplyHipoclorito();
        transform.position = originPos;
    }

    private void TryApplyHipoclorito()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, desechosMask);

        if (hit == null)
        {
            return;
        }

        if (!hit.TryGetComponent(out Desecho desecho))
        {
            return;
        }

        desecho.AplicarHipocloritoDirecto(transform.position);
    }
}