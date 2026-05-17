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


    private void Start()
    {
        ValidateDependencies();
        PopulateGallery();
    }

    private void PopulateGallery()
    {
        // Limpiar el contenedor previamente
        foreach (Transform child in _gridContainer)
        {
            Destroy(child.gameObject);
        }

        List<TutorialDataSO> allTutorials = _tutorialDatabase.AllTutorials;

        for (int i = 0; i < allTutorials.Count; i++)
        {
            TutorialDataSO tutorialData = allTutorials[i];

            // Usamos la función del DataManager pasándole el ID único del puzzle.
            // Nota: Se asume que DataManager.Instance ya expone este método booleano.
            bool isUnlocked = false;

            if (DataManager.Instance != null)
            {
                isUnlocked = DataManager.Instance.HasPuzzleBeenPlayed(tutorialData.PuzzleID);
            }
#if UNITY_EDITOR
            else
            {
                // Salvaguarda para poder probar la UI en el editor sin cargar todo el juego
                isUnlocked = true;
            }
#endif

            TutorialCard newCard = Instantiate(_cardPrefab, _gridContainer);

            newCard.Setup(
                tutorialData,
                isUnlocked,
                OnTutorialCardClicked
            );
        }
    }

    // Delegado que se ejecuta cuando una tarjeta desbloqueada es presionada.
    private void OnTutorialCardClicked(TutorialDataSO clickedTutorialData)
    {
    }

    private void ValidateDependencies()
    {
        if (_tutorialDatabase == null) throw new NullReferenceException($"Falta asignar el {nameof(_tutorialDatabase)} en el GalleryManager.");
        if (_gridContainer == null) throw new NullReferenceException($"Falta asignar el {nameof(_gridContainer)} en el GalleryManager.");
        if (_cardPrefab == null) throw new NullReferenceException($"Falta asignar el {nameof(_cardPrefab)} en el GalleryManager.");
    }
}
