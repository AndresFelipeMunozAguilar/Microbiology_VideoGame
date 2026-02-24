using TMPro;
using UnityEngine;

public class PlayerHealthView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    public void OnEnable()
    {
        PlayerHealthLogic.OnHealthChanged += UpdateHealthText;

        TryGetComponent<TextMeshProUGUI>(out healthText);

        if (healthText == null) Debug.LogError($"PlayerHealthView: No se encontró el componente TextMeshProUGUI en el GameObject: {this.gameObject.name}");
    }

    public void UpdateHealthText(int currentLives)
    {
        healthText.SetText(currentLives.ToString());
    }

    public void OnDisable()
    {
        PlayerHealthLogic.OnHealthChanged -= UpdateHealthText;
    }

}