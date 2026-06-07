using UnityEngine;
using System;
using System.Collections.Generic;

public class TutorialsRepositoryManager : MonoBehaviour
{
    [Header("Base de Datos de Contenido")]
    [SerializeField] private TutorialDatabaseSO _tutorialDatabase;

    [Header("Referencias de la UI (Contenedores)")]
    [Tooltip("El GameObject que tiene el componente Grid/Vertical Layout Group.")]
    [SerializeField] private Transform _gridContainer;
    [SerializeField] private TutorialCard _cardPrefab;
    [Header("Referencia al Visor Expandido")]
    [SerializeField] private TutorialPopupView _popupViewer;

    private void Start()
    {
        ValidateDependencies();
        PopulateGallery();
    }

    private void PopulateGallery()
    {
        CleanContainer();

        List<TutorialDataSO> allTutorials = _tutorialDatabase.AllTutorials;

        for (int i = 0; i < allTutorials.Count; i++)
        {
            TutorialDataSO tutorialData = allTutorials[i];

            bool isUnlocked = false;

            if (DataManager.Instance != null) isUnlocked = DataManager.Instance.HasPuzzleBeenPlayed(tutorialData.PuzzleID);

            TutorialCard newCard = Instantiate(_cardPrefab, _gridContainer);

            newCard.Setup(
                tutorialData,
                isUnlocked,
                OnTutorialCardClicked
            );
        }
    }

    private void CleanContainer()
    {
        foreach (Transform child in _gridContainer)
        {
            Destroy(child.gameObject);
        }
    }

    // Delegado que se ejecuta cuando una tarjeta desbloqueada es presionada.
    private void OnTutorialCardClicked(TutorialDataSO clickedTutorialData)
    {
        if (_popupViewer == null) return;

        // Le ordenamos al visor central que se muestre y se cargue con estos datos especificos
        _popupViewer.OpenWindow(clickedTutorialData);
    }

    private void ValidateDependencies()
    {
        if (_tutorialDatabase == null) Debug.LogError($"Falta asignar el {nameof(_tutorialDatabase)} en el GalleryManager.");
        if (_gridContainer == null) Debug.LogError($"Falta asignar el {nameof(_gridContainer)} en el GalleryManager.");
        if (_cardPrefab == null) Debug.LogError($"Falta asignar el {nameof(_cardPrefab)} en el GalleryManager.");
        if (_popupViewer == null) Debug.LogError($"Falta asignar el {nameof(_popupViewer)} en el GalleryManager.");
    }
}
