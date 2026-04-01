using Unity.VisualScripting;
using UnityEngine;

public class PlayerDamageDealer : MonoBehaviour
{

    private int _damageAmount;

    private bool _canApplyDamage = false;

    [SerializeField]
    private PuzzleDamageTable _puzzleDamageTable;

    public void CalculateDamage(int gameplayScore)
    {
        // Simepre suponemos que no se puede hacer daño
        _canApplyDamage = false;

        foreach (DamageThreshold damageThresholdPair in _puzzleDamageTable.damageThresholdPairs)
        {
            if (gameplayScore <= damageThresholdPair.threshold)
            {
                // Pero si encuentra el valor del daño que se aplicará al jugador
                // entonces, concluimos que sí se puede dañar al jugador
                _canApplyDamage = true;
                _damageAmount = damageThresholdPair.damage;
                Debug.Log($"<color=red>PlayerDamageDealer</color> - Calculated damage amount: {_damageAmount} based on gameplay score: {gameplayScore}");
                return;
            }
        }

        Debug.Log("End of PlayerDamageDealer's function CalculateDamage");
    }

    public bool CanApplyDamage()
    {
        return _canApplyDamage;
    }

    public void DealDamage(IDamageable target)
    {

        Debug.Log($"<color=red>PlayerDamageDealer</color> - Dealing {_damageAmount} damage to target: {target}");
        target.TakeDamage(_damageAmount);
    }

}