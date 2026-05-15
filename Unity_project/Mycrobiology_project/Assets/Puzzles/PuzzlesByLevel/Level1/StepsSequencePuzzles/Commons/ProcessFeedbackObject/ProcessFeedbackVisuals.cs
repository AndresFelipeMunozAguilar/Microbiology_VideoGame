using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ProcessFeedbackVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField]
    [Range(0, 1)]
    private float _transparencyWhenDisabled;


    [SerializeField] private SequentialProccessPuzzleGameplay _sequentialProccessGameplay;

    [SerializeField] private ErrorFeedbackVisuals _errorFeedbackVisuals;
    [SerializeField] private Sprite _positiveFeedbackIcon;
    [SerializeField] private Sprite _negativeFeedbackIcon;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {

        if (GetComponentInParent<SequentialProccessPuzzleGameplay>() == null)
        {
            Debug.LogError("ProcessFeedbackVisuals: Parent object with SequentialProccessPuzzleGameplay component not found.");
            return;
        }
        _sequentialProccessGameplay = GetComponentInParent<SequentialProccessPuzzleGameplay>();
        _sequentialProccessGameplay.OnErrorChanged += ShowErrorEffect;
        _sequentialProccessGameplay.OnPuzzleLost += DisableComponents;
    }


    private void OnDestroy()
    {
        _sequentialProccessGameplay.OnErrorChanged -= ShowErrorEffect;
        _sequentialProccessGameplay.OnPuzzleLost -= DisableComponents;
    }


    public void UpdateVisuals(Sprite newSprite)
    {
        if (newSprite == null) return;

        // Aquí podrías añadir una pequeña animación o partículas antes del cambio
        _spriteRenderer.sprite = newSprite;
        Debug.Log("<color=green>Visuals:</color> Sprite de la mancha actualizado.");

        _errorFeedbackVisuals.SetSuccessText("¡Bien!");
        _errorFeedbackVisuals.Play(_positiveFeedbackIcon);
    }

    public void ShowErrorEffect(int currentErrors, int maxErrors)
    {
        // Feedback visual simple para error (ej. parpadeo rojo)
        // Puedes usar una corrutina o un Tweening aquí.
        Debug.Log("<color=red>Visuals:</color> Mostrando feedback de error.");
        _errorFeedbackVisuals.SetErrorText(currentErrors, maxErrors);
        _errorFeedbackVisuals.Play(_negativeFeedbackIcon);
    }

    public void DisableStain()
    {
        // Se llama cuando se pierden todas las vidas o termina el puzzle

        // Variable auxiliar para configurar la transparencia del sprite al perder
        ChangeSpriteRendererAlpha(_transparencyWhenDisabled);
    }

    private void DisableComponents()
    {
        _errorFeedbackVisuals.gameObject.SetActive(false);
    }

    public void ChangeSpriteRendererAlpha(float transparency)
    {
        Color setTransparency = _spriteRenderer.color;
        setTransparency.a = transparency;
        _spriteRenderer.color = setTransparency;
    }
}