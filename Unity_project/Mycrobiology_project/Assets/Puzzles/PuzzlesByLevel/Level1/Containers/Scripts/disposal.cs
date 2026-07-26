using UnityEngine;
public enum Containers
{
    Cortopunzantes,
    Biologico,
    Basura,
}
[CreateAssetMenu(fileName = "disposal", menuName = "ScriptableObject/disposal", order = 0)]
public class disposal : ScriptableObject
{
    public Sprite image;
    public Containers container;
}

