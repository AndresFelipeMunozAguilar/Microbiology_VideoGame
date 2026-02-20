using System;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    [SerializeField] private Image countdownBar;

    // Se define el delegate que se dispara cuando cambia el tiempo restante
    public static Action<int> OnTimeChanged;

    private int previousTimeRemaining;

    [Header("Time Settings (In seconds)")]
    [SerializeField] private float timeRemaining;
    [SerializeField] private float maxTime = 60f;

    // Para activar la lógica de vaciado de la barra, 
    // el script debe estar activo, pues sólo mientras lo esté, 
    // se vaciará la barra sin detenerse.
    public void OnEnable()
    {
        StartCountdown();
    }

    public void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            countdownBar.fillAmount = timeRemaining / maxTime;
        }
        else
        {
            timeRemaining = 0f;
            Debug.Log("CountdownBarMngr: Time is up!");

            OnTimeChanged?.Invoke(Mathf.FloorToInt(timeRemaining));
            // Dejar de actualizarse al terminar la cuenta regresiva
            this.enabled = false;
        }

        if (Mathf.FloorToInt(timeRemaining) != previousTimeRemaining)
        {
            previousTimeRemaining = Mathf.FloorToInt(timeRemaining);

            Debug.Log($"BarManager: Disparo el delegate Action OnTimeChanged con valor: {previousTimeRemaining}");
            OnTimeChanged?.Invoke(previousTimeRemaining);
        }
    }

    public void StartCountdown()
    {
        timeRemaining = maxTime;
        countdownBar.fillAmount = 1f;
    }

}