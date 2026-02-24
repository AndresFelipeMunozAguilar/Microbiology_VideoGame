using System;
using UnityEngine;

public class PlayerHealthLogic : MonoBehaviour, IDamageable
{
    [Header("Player Health Settings")]
    [SerializeField] private int maxLives = 3;

    [SerializeField] private int currentLives;

    [SerializeField] private bool isDead;

    public static Action<int> OnHealthChanged;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        currentLives = maxLives;
        OnHealthChanged?.Invoke(currentLives);
        isDead = false;
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
            GameManager.GetInstance().OnGameOver("Player has died");
        }

    }

    // Esta función es necesaria, ya que garantiza 
    // que el número de vidas no sea negativo, 
    // evitando bugs relacionados con vidas negativas.
    public int ReduceLivesNBound(int damageAmount)
    {
        currentLives -= damageAmount;
        currentLives = Mathf.Max(currentLives, 0);

        Debug.Log("Player: I took damage. Current lives: " + currentLives);
        return currentLives;
    }
}
