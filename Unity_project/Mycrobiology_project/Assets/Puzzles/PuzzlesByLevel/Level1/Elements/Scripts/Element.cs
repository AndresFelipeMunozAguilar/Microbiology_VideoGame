using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "ScriptableObject/Element", order = 0)]
public class Element : ScriptableObject
{
    public Sprite image;
    public float targetTemperature;
    public bool isMechero;
    public bool isBañoMaria;
}

