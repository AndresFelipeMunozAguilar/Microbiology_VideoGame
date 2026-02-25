using System;
using UnityEngine;

public class PlayerHealthLogic : MonoBehaviour, IDamageable, IGameOverSubscriber
{
    [Header("Player Health Settings")]
    [SerializeField] private int maxLives = 3;

    [SerializeField] private int currentLives;

    [SerializeField] private bool isDead;

    public static Action<int> OnHealthChanged;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnEnable()
    {
        GameManager.SubscribeToGameOver(this);
        currentLives = maxLives;
        OnHealthChanged?.Invoke(currentLives);
        isDead = false;
    }

    public void OnDisable()
    {
        GameManager.UnsubscribeToGameOver(this);
    }

    public void TakeDamage(int damageAmount)
    {
        // Se necesita limitar las vidas a ser positivas 
        // para evitar bugs con vidas negativas
        ReduceLivesNBound(damageAmount);

        OnHealthChanged?.Invoke(currentLives);

        if (currentLives <= 0)
        {
            Debug.Log("Player: I'm dead, executing death logic");
            isDead = true;
            GameManager.GetInstance().GameOver("Player has died");
        }

    }

    // Esta función es necesaria, ya que garantiza 
    // que el número de vidas no sea negativo, 
    // evitando bugs relacionados con vidas negativas.
    public int ReduceLivesNBound(int damageAmount)
    {
        currentLives -= damageAmount;
        currentLives = Mathf.Max(0, currentLives);

        Debug.Log("Player: I took damage. Current lives: " + currentLives);
        return currentLives;
    }

    public void OnGameOver()
    {
        Debug.Log("PlayerHealthLogic: I have received the GameOver event. Executing logic");

        // Aquí podríamos agregar lógica adicional
        //  que queramos que suceda en el PlayerHealthLogic 
        // cuando se active el Game Over, como desactivar 
        // el script para evitar que siga recibiendo 
        // daño o actualizando la UI.
        this.enabled = false;
    }
}
