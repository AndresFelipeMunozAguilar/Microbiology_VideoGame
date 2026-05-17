using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TutorialDatabase",
    menuName = "ScriptableObject/Tutorials Repository/Tutorial Database")]
public class TutorialDatabaseSO : ScriptableObject
{
    [SerializeField] private List<TutorialDataSO> allTutorials = new List<TutorialDataSO>();

    public List<TutorialDataSO> AllTutorials => allTutorials;

    public TutorialDataSO FindTutorialByID(string id)
    {
        return allTutorials.Find(tutorial => tutorial.PuzzleID == id);
    }
}