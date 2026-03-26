using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BloodStainVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField]
    [Range(0, 1)]
    private float _transparencyWhenDisabled;


    [SerializeField] private BloodStainPuzzleGameplay _bloodStainGameplay;

    [SerializeField] private BloodStainFeedbackVisuals _feedbackVisuals;
    [SerializeField] private Sprite _positiveFeedbackIcon;
    [SerializeField] private Sprite _negativeFeedbackIcon;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {

        if (GetComponentInParent<BloodStainPuzzleGameplay>() == null)
        {
            Debug.LogError("BloodStainFeedbackVisuals: Parent object with BloodStainPuzzleGameplay component not found.");
            return;
        }
        _bloodStainGameplay = GetComponentInParent<BloodStainPuzzleGameplay>();
        _bloodStainGameplay.OnErrorChanged += ShowErrorEffect;
        _bloodStainGameplay.OnPuzzleLost += DisableComponents;
    }


    private void OnDestroy()
    {
        _bloodStainGameplay.OnErrorChanged -= ShowErrorEffect;
        _bloodStainGameplay.OnPuzzleLost -= DisableComponents;
    }


    public void UpdateVisuals(Sprite newSprite)
    {
        if (newSprite == null) return;

        // Aquí podrías añadir una pequeña animación o partículas antes del cambio
        _spriteRenderer.sprite = newSprite;
        Debug.Log("<color=green>Visuals:</color> Sprite de la mancha actualizado.");

        _feedbackVisuals.SetSuccessText("¡Bien!");
        _feedbackVisuals.Play(_positiveFeedbackIcon);
    }

    public void ShowErrorEffect(int currentErrors, int maxErrors)
    {
        // Feedback visual simple para error (ej. parpadeo rojo)
        // Puedes usar una corrutina o un Tweening aquí.
        Debug.Log("<color=red>Visuals:</color> Mostrando feedback de error.");
        _feedbackVisuals.SetErrorText(currentErrors, maxErrors);
        _feedbackVisuals.Play(_negativeFeedbackIcon);
    }

    public void DisableStain()
    {
        // Se llama cuando se pierden todas las vidas o termina el puzzle

        // Variable auxiliar para configurar la transparencia del sprite al perder
        ChangeSpriteRendererAlpha(_transparencyWhenDisabled);
    }

    private void DisableComponents()
    {
        _feedbackVisuals.gameObject.SetActive(false);
    }

    public void ChangeSpriteRendererAlpha(float transparency)
    {
        Color setTransparency = _spriteRenderer.color;
        setTransparency.a = transparency;
        _spriteRenderer.color = setTransparency;
    }
}