using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealthView : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI healthText;
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

}