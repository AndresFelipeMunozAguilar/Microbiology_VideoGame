using System;
using UnityEngine;

public class TutorialTestPuzzle : MonoBehaviour
{
    [SerializeField]
    private PuzzleGameplay puzzleGameplay;

    private String puzzleName = "PuzzleManager";

    private void Start()
    {
        puzzleGameplay = GameObject.Find(puzzleName).GetComponent<PuzzleGameplay>();

        if (puzzleGameplay == null)
        {
            Debug.LogError("PuzzleGameplay component not found on " + puzzleName);
        }

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

        RectTransform tutorialRectTransform = transform.parent.GetComponent<RectTransform>();
        tutorialRectTransform.localScale = Vector3.one * 0.2f;

        puzzleGameplay.StartGameplay();

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
