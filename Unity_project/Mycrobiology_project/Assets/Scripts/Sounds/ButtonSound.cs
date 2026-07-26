using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    public string soundName;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(soundName);
        }
        else
        {
            Debug.LogWarning("No se encontró AudioManager en la escena.");
        }
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(PlaySound);
    }
}