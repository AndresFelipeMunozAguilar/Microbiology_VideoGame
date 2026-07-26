using UnityEngine;

[CreateAssetMenu(fileName = "TutorialData",
    menuName = "ScriptableObject/Tutorials Repository/Tutorial Data")]
public class TutorialDataSO : ScriptableObject
{
    [Header("Identificadores Únicos")]
    [Tooltip("Debe coincidir exactamente con el ID registrado en el JSON")]
    [SerializeField] private string _puzzleID;

    [Header("Contenido de la Galería")]
    [SerializeField] private string _tutorialTitle;
    [SerializeField] private Sprite _thumbnailSprite;

    [Header("Contenido del Visor Expandido")]
    [Tooltip("Imagen estática en alta resolución que se mostrará al maximizar")]
    [SerializeField] private Sprite _fullTutorialSprite;

    public string PuzzleID => _puzzleID;
    public string TutorialTitle => _tutorialTitle;
    public Sprite ThumbnailSprite => _thumbnailSprite;
    public Sprite FullTutorialSprite => _fullTutorialSprite;
}