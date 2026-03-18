using System.Collections;
using TMPro;
using UnityEngine;

public class PuzzleCompletionText : MonoBehaviour
{
    [Header("Objetos Asociados")]
    // Se usa TextMeshPro porque el texto no aparece en la UI
    [SerializeField] private TextMeshPro _textMeshPro;
    [SerializeField] private PuzzleCompletionCheckZone _puzzleCompletionCheckZone;


    [Header("Configuración de Animación")]
    [SerializeField] private float _duration = 1.5f;
    [SerializeField] private float _yOffset = 20f;
    private Coroutine _activeAnimation;
    private Vector3 _initialPosition;

    // =======================[Métodos Iniciales]=======================
    public void Start()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
        _initialPosition = transform.localPosition;

        if (_puzzleCompletionCheckZone == null)
        {
            Debug.LogError("PuzzleCompletionText: PuzzleCompletionCheckZone reference is null. Please assign it in the inspector.");
            return;
        }
        _puzzleCompletionCheckZone.OnPlayerGetsClose += ShowPuzzleCompletionText;
        _puzzleCompletionCheckZone.OnPlayerLeaves += HidePuzzleCompletionText;

        // El objeto inicia desactivado por seguridad
        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        _puzzleCompletionCheckZone.OnPlayerGetsClose -= ShowPuzzleCompletionText;
        _puzzleCompletionCheckZone.OnPlayerLeaves -= HidePuzzleCompletionText;
    }

    // =======================[Lógica de la corrutina]=======================

    private IEnumerator AnimateText(float startAlpha, float endAlpha, Vector3 startPos, Vector3 endPos, bool disableAtEnd = false)
    {
        float elapsedTime = 0;
        Color color = _textMeshPro.color;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / _duration;

            // Interpolar posición y transparencia
            transform.localPosition = Vector3.Lerp(startPos, endPos, percent);
            color.a = Mathf.Lerp(startAlpha, endAlpha, percent);
            _textMeshPro.color = color;

            yield return null;
        }

        if (disableAtEnd) gameObject.SetActive(false);
    }

    private void StopCurrentAnimation()
    {
        if (_activeAnimation != null) StopCoroutine(_activeAnimation);
    }

    public void Show()
    {
        StopCurrentAnimation();
        gameObject.SetActive(true);
        _activeAnimation = StartCoroutine(AnimateText(0, 1, _initialPosition, _initialPosition + Vector3.up * _yOffset));
    }

    public void Hide()
    {
        StopCurrentAnimation();
        _activeAnimation = StartCoroutine(AnimateText(1, 0, transform.localPosition, transform.localPosition - Vector3.up * _yOffset, true));
    }

    // =======================[Triggers de activar o desactivar animacion]=======================
    public string GetCompletionMessage(int completedPuzzles, int totalPuzzles)
    {
        if (totalPuzzles <= 0) return "Espera... ¿Donde están los puzzles?";

        // Si hay puzzles, empecemos a contarlos!!
        if (completedPuzzles >= 0 && completedPuzzles < totalPuzzles) return $"Puzzles completados: {completedPuzzles}/{totalPuzzles}";
        if (completedPuzzles >= totalPuzzles) return "¡Has completado todos los puzzles!";

        return "Caso no especificado. Numero invalido";
    }

    public void ShowPuzzleCompletionText(int completedPuzzles, int totalPuzzles)
    {
        string completionMessage = GetCompletionMessage(completedPuzzles, totalPuzzles);
        _textMeshPro.SetText(completionMessage);
        Show();
    }

    public void HidePuzzleCompletionText()
    {
        Hide();
    }



}