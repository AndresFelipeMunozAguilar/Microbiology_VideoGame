using UnityEngine;

public enum MetodoDescontaminacion
{
    Autoclave,
    Hipoclorito,
    Sumergirse
}

[CreateAssetMenu(
    fileName = "DesechoElement",
    menuName = "ScriptableObject/Desecho Element",
    order = 1
)]
public class DesechoElement : ScriptableObject
{
    public string Name;

    public Sprite image;

    [Header("Método correcto")]
    public MetodoDescontaminacion metodoCorrecto;

    [Header("Retroalimentación")]
    [TextArea(2, 5)]
    public string feedbackCorrecto;

    [TextArea(2, 5)]
    public string feedbackIncorrecto;
}