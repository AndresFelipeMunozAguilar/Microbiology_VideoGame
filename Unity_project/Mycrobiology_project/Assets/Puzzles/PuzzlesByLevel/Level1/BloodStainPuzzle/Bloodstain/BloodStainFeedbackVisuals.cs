using System.Collections;
using UnityEngine;
using TMPro; // Asumiendo que usas TextMeshPro

public class BloodStainFeedbackVisuals : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private SpriteRenderer _iconSpriteRenderer;
    [SerializeField] private TextMeshPro _textMeshPro;

    [Header("Configuración de Capas")]
    [SerializeField] private string _sortingLayerName = "Puzzles";
    [SerializeField] private int _orderInLayer = 70;

    [Header("Parámetros de Animación")]
    [Tooltip("Distancia máxima hacia arriba desde el punto inicial.")]
    [SerializeField] private float _jumpHeight = 2f;

    [Tooltip("Tiempo total que dura el salto (ida y vuelta).")]
    [SerializeField] private float _duration = 1.5f;

    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.localPosition;
        SetVisibility(false);
    }


    public void SetErrorText(int currentErrors, int maxErrors)
    {
        _textMeshPro.SetText($"Error {currentErrors}/{maxErrors}");
    }

    public void SetSuccessText(string succesText)
    {
        _textMeshPro.SetText(succesText);
    }

    /// Punto de entrada público para disparar la animación.
    public void Play(Sprite contextualSprite)
    {
        _iconSpriteRenderer.sprite = contextualSprite;
        StopAllCoroutines();
        StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        SetVisibility(true);
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;

            // Calculamos el progreso de 0 a 1
            float normalizedTime = elapsed / _duration;

            // Usamos Seno para crear la curva de subida y bajada
            // Sin(0) = 0, Sin(PI/2) = 1 (punta), Sin(PI) = 0 (fondo)
            float yOffset = Mathf.Sin(normalizedTime * Mathf.PI) * _jumpHeight;

            // Aplicamos la posición sincronizada al objeto padre (que mueve a ambos)
            transform.localPosition = _startPosition + new Vector3(0, yOffset, 0);

            yield return null;
        }

        // Aseguramos posición final y ocultamos
        transform.localPosition = _startPosition;
        SetVisibility(false);
    }

    private void SetVisibility(bool isVisible)
    {
        if (_iconSpriteRenderer != null) _iconSpriteRenderer.enabled = isVisible;
        if (_textMeshPro != null) _textMeshPro.enabled = isVisible;
    }
}