using System;
using UnityEngine;
using UnityEngine.UI;

public class CountdownBarManager : MonoBehaviour
{
    [SerializeField] private Image countdownBar;


    [Header("Main Behaviour")]

    [SerializeField] private bool isPaused;

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
        // Guard Clause de Pausa: Si está pausado, no gastamos CPU en el resto.
        if (isPaused) return;

        // ======== LOGICA PRINCIPAL ========
        // Se disminuye el tiempo restante
        timeRemaining -= Time.deltaTime;

        // Guard Clause principal: Si el tiempo se acabó, 
        // ejecutamos la lógica de time up y salimos del update.

        // OJO
        // Esto se coloca aquí para que comprobar la disminución 
        // del tiempo sea lo primero que se haga, así los pasos 
        // siguientes tendrán la certeza de que el tiempo restante 
        // será siempre mayor a 0, evitando que se dibuje la barra
        //  o se actualice el cronometro con un valor negativo
        if (timeRemaining <= 0)
        {
            TimeIsUp();
            return;
        }

        // Se actualiza la barra de progreso
        countdownBar.fillAmount = timeRemaining / maxTime;

        // Segundo Guard Clause: Si el segundo no ha cambiado, salimos.
        if (Mathf.FloorToInt(timeRemaining) == previousTimeRemaining) return;

        previousTimeRemaining = Mathf.FloorToInt(timeRemaining);

        Debug.Log($"BarManager: Disparo el delegate Action OnTimeChanged con valor: {previousTimeRemaining}");
        OnTimeChanged?.Invoke(previousTimeRemaining);

    }

    public void StartCountdown()
    {
        timeRemaining = maxTime;

        // IMPORTANTE: Resetear esta variable evita que el evento OnTimeChanged 
        // no se dispare en el primer segundo del nuevo conteo.
        previousTimeRemaining = Mathf.FloorToInt(maxTime);

        // Forzamos la barra al máximo de inmediato
        if (countdownBar != null) countdownBar.fillAmount = 1f;

        isPaused = false;
        Debug.Log("BarManager: Countdown Reiniciado");
    }

    // Pausar o reanudar el tiempo
    public void SetPaused(bool paused)
    {
        isPaused = paused;
        Debug.Log($"BarManager: Tiempo {(isPaused ? "Pausado" : "Reanudado")}");
    }

    // Reinicia el contador a su estado original
    public void ResetCountdown()
    {
        StartCountdown();
    }

    public void TimeIsUp()
    {
        // Nos aseguramos de que el tiempo no sea negativo 
        // ni en la variable, ni en el texto de la UI, 
        // aunque la barra ya esté vacía
        timeRemaining = 0f;
        OnTimeChanged?.Invoke(Mathf.FloorToInt(timeRemaining));

        Debug.Log("CountdownBarMngr: Time is up! Invoking OnTimeUp Action dlgt...");

        GameManager.GetInstance().GameOver("Time is up!");

        // Dejar de actualizarse al terminar la cuenta regresiva
        this.enabled = false;
    }



}