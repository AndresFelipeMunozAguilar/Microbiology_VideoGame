using Unity.VisualScripting;
using UnityEngine;

public class PlayerDamageDealer : MonoBehaviour
{

    private int _damageAmount = 3;

    [SerializeField]
    private PuzzleDamageTable _puzzleDamageTable;

    public void CalculateDamage(int gameplayScore)
    {
        foreach (DamageThreshold damageThresholdPair in _puzzleDamageTable.damageThresholdPairs)
        {
            if (gameplayScore <= damageThresholdPair.threshold)
            {
                _damageAmount = damageThresholdPair.damage;
                Debug.Log($"<color=red>PlayerDamageDealer</color> - Calculated damage amount: {_damageAmount} based on gameplay score: {gameplayScore}");
                return;
            }
        }

        Debug.Log("End of PlayerDamageDealer's function CalculateDamage");
    }

    public void DealDamage(IDamageable target)
    {

        Debug.Log($"<color=red>PlayerDamageDealer</color> - Dealing {_damageAmount} damage to target: {target}");
        target.TakeDamage(_damageAmount);
    }

}