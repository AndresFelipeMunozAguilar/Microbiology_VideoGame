using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PuzzleVisuals : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    [Range(0, 255)]
    private int _transparencyWhenDisabled;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateVisuals(Sprite newSprite)
    {
        if (newSprite == null) return;

        // Aquí podrías añadir una pequeña animación o partículas antes del cambio
        _spriteRenderer.sprite = newSprite;
        Debug.Log("<color=green>Visuals:</color> Sprite de la mancha actualizado.");
    }

    public void ShowErrorEffect()
    {
        // Feedback visual simple para error (ej. parpadeo rojo)
        // Puedes usar una corrutina o un Tweening aquí.
        Debug.Log("<color=red>Visuals:</color> Mostrando feedback de error.");
    }

    public void DisableStain()
    {
        // Se llama cuando se pierden todas las vidas o termina el puzzle

        // Variable auxiliar para configurar la transparencia del sprite al perder
        Color setTransparency = _spriteRenderer.color;

        setTransparency.a = _transparencyWhenDisabled;
        _spriteRenderer.color = setTransparency;
    }
}