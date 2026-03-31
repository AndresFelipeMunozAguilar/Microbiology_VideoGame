using System;
using UnityEngine;

public class PuzzleTutorial : MonoBehaviour
{
    [SerializeField]
    private AbstractPuzzleGameplay puzzleGameplay;

    [SerializeField]
    private String puzzleName = "PuzzleManager";

    private void Start()
    {

        if (transform.parent != null)
        {
            RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
            if (parentRect != null)
            {
                parentRect.localPosition = Vector3.zero;
            }
            else
            {
                Debug.LogError("RectTransform not found on parent of " + gameObject.name);
            }
        }
        else
        {
            Debug.LogError("No parent found for " + gameObject.name);
        }
    }

    public void StartPuzzleGameplay()
    {

        puzzleGameplay = GameObject.Find(puzzleName).GetComponentInChildren<AbstractPuzzleGameplay>();

        if (puzzleGameplay == null)
        {
            Debug.LogError("PuzzleGameplay component not found on " + puzzleName);
        }

        RectTransform tutorialRectTransform = transform.parent.GetComponent<RectTransform>();
        tutorialRectTransform.localScale = Vector3.one * 0.2f;

        puzzleGameplay.StartGameplay();

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
