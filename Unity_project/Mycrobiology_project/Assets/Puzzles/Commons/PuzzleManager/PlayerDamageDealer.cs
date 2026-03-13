using Unity.VisualScripting;
using UnityEngine;

public class PlayerDamageDealer : MonoBehaviour
{
    [SerializeField]
    private int _damageAmount = 3;

    public void CalculateDamage()
    {
        _damageAmount = 2;
    }

    public void DealDamage(IDamageable target)
    {

        Debug.Log($"<color=red>PlayerDamageDealer</color> - Dealing {_damageAmount} damage to target: {target}");
        target.TakeDamage(_damageAmount);
    }

}