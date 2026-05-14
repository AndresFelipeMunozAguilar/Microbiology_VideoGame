using UnityEngine;
using System.Collections;

public class DraggableStep : AbstractDraggableWorldObject
{
    [SerializeField] private Vector3 _globalStartPosition;
    private Vector3 _localStartPosition;
    [SerializeField] private float returnSpeed = 5f;

    protected override void OnInstanceStart()
    {

        if (GetComponentInParent<AbstractPuzzleGameplay>() == null)
        {
            Debug.LogError($"DragCleaningStep: No se pudo encontrar el componente de tipo AbstractPuzzleGameplay");
            return;
        }


        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        Vector3 cameraWorldPos = mainCamera.transform.position;
        cameraWorldPos.z = _globalStartPosition.z;

        Vector3 targetWorldPos = cameraWorldPos + _globalStartPosition;

        _localStartPosition = GetComponentInParent<AbstractPuzzleGameplay>()
                            .transform
                            .InverseTransformPoint(targetWorldPos);


    }

    protected override void OnDragEnded()
    {
        // Comportamiento propio: Regresar al punto de origen con un Smooth
        StartCoroutine(ReturnToStart());
    }

    private IEnumerator ReturnToStart()
    {
        while (Vector3.Distance(transform.localPosition, _localStartPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _localStartPosition, Time.deltaTime * returnSpeed);
            yield return null;
        }
        transform.localPosition = _localStartPosition;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (GetComponentInParent<AbstractPuzzleGameplay>() == null)
        {
            Debug.LogError($"DragCleaningStep: No se pudo encontrar el componente de tipo AbstractPuzzleGameplay");
            return;
        }
        AbstractPuzzleGameplay puzzleGameplay = GetComponentInParent<AbstractPuzzleGameplay>();

        // Dibuja una esfera en la posición del Vector3
        // transform.position permite que sea relativo al objeto
        Gizmos.DrawLine(puzzleGameplay.transform.position, _globalStartPosition);
    }
}
