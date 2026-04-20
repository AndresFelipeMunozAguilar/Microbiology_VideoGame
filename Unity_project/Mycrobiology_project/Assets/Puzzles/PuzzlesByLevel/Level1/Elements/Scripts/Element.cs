using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "ScriptableObject/Element", order = 0)]
public class Element : ScriptableObject
{
    public Color colorSustance;
    public string Name;
    public Sprite image;
    public float minTemperature,maxTemperature;
    public bool isMechero;
    public bool isBañoMaria;
}

