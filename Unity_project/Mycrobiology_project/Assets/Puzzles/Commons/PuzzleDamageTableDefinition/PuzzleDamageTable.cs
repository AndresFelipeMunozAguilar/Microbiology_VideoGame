using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DamageThreshold
{
    public int threshold;
    public int damage;
}

[CreateAssetMenu(fileName = "DamageTable", menuName = "ScrptableObjects/Puzzles/DamageTable")]
public class PuzzleDamageTable : ScriptableObject
{
    public List<DamageThreshold> damageThresholdPairs = new List<DamageThreshold>();
}