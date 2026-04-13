using System.Collections;
using UnityEngine;

public class DragFeedback : MonoBehaviour
{
    [SerializeField] private float bounceDistance = 0.5f;
    [SerializeField] private float bounceOutTime = 0.08f;
    [SerializeField] private float bounceBackTime = 0.12f;
    [SerializeField] private AnimationCurve easeOut;
    [SerializeField] private AnimationCurve easeBack;

    private bool isAnimating;

    public void PlayWrongDropFeedback(Transform wrongContainer)
    {
        if (isAnimating) return;
        StartCoroutine(WrongDropRoutine(wrongContainer));
    }

    private IEnumerator WrongDropRoutine(Transform wrongContainer)
    {
        isAnimating = true;

        Vector3 startPos = transform.position;

        Vector3 awayDir = (transform.position - wrongContainer.position).normalized;

        if (awayDir == Vector3.zero)
            awayDir = Vector3.up;

        Vector3 bounceTarget = startPos + awayDir * bounceDistance;

        yield return MoveOverTime(startPos, bounceTarget, bounceOutTime, easeOut);
        yield return MoveOverTime(bounceTarget, startPos, bounceBackTime, easeBack);

        isAnimating = false;
    }

    private IEnumerator MoveOverTime(Vector3 from, Vector3 to, float duration, AnimationCurve curve)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            float curvedT = curve != null ? curve.Evaluate(t) : t;

            transform.position = Vector3.Lerp(from, to, curvedT);
            yield return null;
        }

        transform.position = to;
    }
}