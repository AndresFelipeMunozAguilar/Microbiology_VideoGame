using UnityEngine;

[System.Serializable]
public class SpawnableElement
{
    public string name; // Solo para organización en el Inspector
    public GameObject prefab;
    public Vector3 globalPosition;
    public Quaternion localRotation = Quaternion.identity;
}