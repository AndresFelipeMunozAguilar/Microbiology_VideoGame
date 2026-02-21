using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI showText;

    private int showMinutes;
    private int showSeconds;

    void Start()
    {
        BarManager.OnTimeChanged += UpdateTextInMinutes;
    }

    public void UpdateTextInMinutes(int seconds)
    {
        showMinutes = seconds / 60;
        showSeconds = seconds % 60;

        showText.SetText($"{showMinutes:00}:{showSeconds:00}");
    }


    public void OnEnable()
    {
        // Suscripción: "Cuando se cambie el tiempo restante, actualiza mi texto"
        BarManager.OnTimeChanged += UpdateTextInMinutes;
    }


    public void OnDisable()
    {
        // Esto evita que Unity intente llamar a un objeto 
        // destruido o desactivado, causando un Crash o Memory Leak.
        BarManager.OnTimeChanged -= UpdateTextInMinutes;
    }

}
