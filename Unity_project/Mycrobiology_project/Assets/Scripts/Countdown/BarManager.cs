using System;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    [SerializeField] private Image countdownBar;

    // Se define el delegate que se dispara cuando cambia
    //  el tiempo restante
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
            // Se disminuye el tiempo restante
            timeRemaining -= Time.deltaTime;

            // Se actualiza la barra de progreso
            countdownBar.fillAmount = timeRemaining / maxTime;
        }
        else
        {
            TimeIsUp();
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

    public void TimeIsUp()
    {
        // Nos aseguramos de que el tiempo no sea negativo 
        // ni en la variable, ni en el texto de la UI, 
        // aunque la barra ya esté vacía
        timeRemaining = 0f;
        OnTimeChanged?.Invoke(Mathf.FloorToInt(timeRemaining));

        Debug.Log("CountdownBarMngr: Time is up! Invoking OnTimeUp Action dlgt...");

        GameManager.GetInstance().OnGameOver("Time is up!");

        // Dejar de actualizarse al terminar la cuenta regresiva
        this.enabled = false;
    }



}