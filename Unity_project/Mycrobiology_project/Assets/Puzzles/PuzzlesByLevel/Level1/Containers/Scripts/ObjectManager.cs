using System.Collections;
using UnityEngine;

public class ObjectManager : AbstractDraggableWorldObject
{
    private disposal Diposal;
    private PuzzleEvaluation score;
    private PuzzleGameplayContainer gamePlay;
    private Vector3 originPos;
    private Transform originalParent;
    private bool isAnimating;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Wrong Drop Jump")]
    [SerializeField] private float jumpDuration = 0.35f;
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float leftOffset = -0.8f;
    [SerializeField] private float edgeHeight = 0.15f;

    [Header("Return")]
    [SerializeField] private float returnDuration = 0.45f;

    [SerializeField] GameObject Check;
    private int Points,Decrese;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void CreateDiposal(disposal assigned, PuzzleEvaluation evaluation, PuzzleGameplayContainer gamePlayContainer,int value)
    {
        score = evaluation;
        gamePlay = gamePlayContainer;
        Diposal = assigned;
        Points=value;
        Decrese= Mathf.RoundToInt(value/3);
        Debug.Log("[Containers] decrease: "+ Decrese);
        spriteRenderer.sprite = Diposal.image;
        originPos = transform.position;
        originalParent = transform.parent;
    }

    protected override void OnDragStarted()
    {
        if (isAnimating) return;
        originPos = transform.position;
    }

    protected override void OnDragEnded()
    {
        if (isAnimating) return;

        Collider2D hit = Physics2D.OverlapPoint(transform.position, layerMask);

        if (hit != null)
        {
            Debug.Log("[Containers] " + hit.transform.name);

            if (hit.CompareTag("DropZone"))
            {
                originPos = transform.position;
            }
            else if (hit.TryGetComponent(out ContainerManager container))
            {
                if (container.getType() == Diposal.container)
                {
                    transform.SetParent(hit.transform, true);
                    transform.localPosition = Vector3.zero;
                    GoodDrop();
                }
                else
                {
                    BadDrop(hit.transform);
                }
                return;
            }
        }

        transform.position = originPos;
    }

    void GoodDrop()
    {
        AudioManager.Instance.PlaySFX("correct");
        score.AddPoints(Points);
        gamePlay.CompleteObject();
        GameObject check = Instantiate(Check,transform.parent);
        Destroy(check,5f);
        Destroy(gameObject,5f);
    }

    void BadDrop(Transform wrongContainer)
    {
        AudioManager.Instance.PlaySFX("wrong");
        Points-=Decrese;
        if(Points<=0)Points=1;
        if (isAnimating) return;
        StartCoroutine(WrongDropRoutine(wrongContainer));
    }

    private IEnumerator WrongDropRoutine(Transform wrongContainer)
    {
        isAnimating = true;

        transform.SetParent(wrongContainer, false);
        transform.localPosition = Vector3.zero;

        yield return StartCoroutine(JumpToContainerEdgeRoutine());

        transform.SetParent(originalParent, true);

        yield return StartCoroutine(MoveToWorldRoutine(Vector2.zero, returnDuration));

        isAnimating = false;
    }

    private IEnumerator JumpToContainerEdgeRoutine()
    {
        Vector3 start = Vector3.zero;
        Vector3 end = new Vector3(leftOffset, edgeHeight, 0f);

        float time = 0f;

        while (time < jumpDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / jumpDuration);
            Vector3 pos = Vector3.Lerp(start, end, t);

            float arc = 4f * jumpHeight * t * (1f - t);
            pos.y += arc;

            transform.localPosition = pos;
            yield return null;
        }

        transform.localPosition = end;
    }

    private IEnumerator MoveToWorldRoutine(Vector3 targetWorldPos, float duration)
    {
        Vector3 start = transform.localPosition;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.localPosition = Vector3.Lerp(start, targetWorldPos, t);
            yield return null;
        }

        transform.localPosition = Vector3.zero;
    }
}