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

    // --- GESTIÓN DE EVENTOS ---

    public void OnEnable()
    {
        // Suscripción: "Cuando pase esto, ejecuta mi función"
        BarManager.OnTimeChanged += UpdateTextInMinutes;
    }

    // --- GESTIÓN DE EVENTOS ---

    public void OnDestroy()
    {
        // Suscripción: "Cuando pase esto, ejecuta mi función"
        BarManager.OnTimeChanged -= UpdateTextInMinutes;
    }


    public void OnDisable()
    {
        // PUNTO CIEGO CRÍTICO: Si no te desvives, Unity intentará 
        // llamar a un objeto destruido, causando un Crash o Leak.
        BarManager.OnTimeChanged -= UpdateTextInMinutes;
    }

}
