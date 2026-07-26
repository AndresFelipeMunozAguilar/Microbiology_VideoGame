using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthView : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;
    [SerializeField] private int maxLives;
    [SerializeField] private float fillSpeed = 2f;

    private Coroutine fillCoroutine;

    private void OnEnable()
    {
        PlayerHealthLogic.OnHealthChanged += UpdateHealthBar;

        if (healthFillImage == null)
            TryGetComponent(out healthFillImage);

        if (healthFillImage == null)
            Debug.LogError($"PlayerHealthView: No se encontró el componente Image en {gameObject.name}");
    }

    private void OnDisable()
    {
        PlayerHealthLogic.OnHealthChanged -= UpdateHealthBar;
    }

    public void UpdateHealthBar(int currentLives)
    {
        if (healthFillImage == null) return;

        float targetFill = Mathf.Clamp01((float)currentLives / maxLives);

        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);

        fillCoroutine = StartCoroutine(AnimateFill(targetFill));
    }

    private IEnumerator AnimateFill(float targetFill)
    {
        while (!Mathf.Approximately(healthFillImage.fillAmount, targetFill))
        {
            healthFillImage.fillAmount = Mathf.MoveTowards(
                healthFillImage.fillAmount,
                targetFill,
                fillSpeed * Time.deltaTime
            );

            yield return null;
        }

        healthFillImage.fillAmount = targetFill;
        fillCoroutine = null;
    }
}