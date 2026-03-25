using UnityEngine;

[System.Serializable]
public class SpawnableElement
{
    public string name; // Solo para organización en el Inspector
    public GameObject prefab;
    public Vector3 localPosition;
    public Quaternion localRotation = Quaternion.identity;
}