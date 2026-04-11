using UnityEngine;

public class PuzzleHalo : MonoBehaviour, IPuzzlePausable
{
    [Header("Pulse")]
    [SerializeField] private float pulseSpeed = 2f;

    // Multiplicador mínimo y máximo respecto a la escala inicial
    [SerializeField] private float minScaleMultiplier = 1f;
    [SerializeField] private float maxScaleMultiplier = 1.4f;

    [Header("Glow")]
    [SerializeField] private float minimumTransparency = 0.25f;
    [SerializeField] private float maximumTransparency = 1f;

    [Header("References")]
    [SerializeField] private SpriteRenderer haloSr;

    private Vector3 haloBaseScale;
    public bool isPlayerClose = false;
    private GameManager _gameManager;

    private void Start()
    {
        if (haloSr == null)
            haloSr = GetComponent<SpriteRenderer>();

        // Tomar la escala real con la que inicia el objeto
        haloBaseScale = transform.localScale;

        _gameManager = GameManager.GetInstance();
        _gameManager.SubscribePuzzlePausable(this);

        // Estado inicial visual
        transform.localScale = haloBaseScale;

        Color haloColor = haloSr.color;
        haloColor.a = minimumTransparency;
        haloSr.color = haloColor;
    }

    private void Update()
    {
        if (!isPlayerClose) return;

        float drawTime = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;

        Pulse(drawTime);
        Glow(drawTime);
    }

    private void Pulse(float referenceTime)
    {
        float scaleMultiplier = Mathf.Lerp(minScaleMultiplier, maxScaleMultiplier, referenceTime);
        transform.localScale = haloBaseScale * scaleMultiplier;
    }

    private void Glow(float referenceTime)
    {
        Color haloColor = haloSr.color;
        haloColor.a = Mathf.Lerp(minimumTransparency, maximumTransparency, referenceTime);
        haloSr.color = haloColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = false;

            transform.localScale = haloBaseScale;

            Color haloColor = haloSr.color;
            haloColor.a = minimumTransparency;
            haloSr.color = haloColor;
        }
    }

    public void PuzzlePauseMe()
    {
        Debug.Log("I am PUZZLE HALO and i have been PAUSED.");
        haloSr.enabled = false;
        enabled = false;
    }

    public void PuzzleResumeMe()
    {
        Debug.Log("I am PUZZLE HALO and i have been RESUMED.");
        haloSr.enabled = true;
        enabled = true;
    }

    private void OnDestroy()
    {
        if (_gameManager != null)
            _gameManager.UnsubscribePuzzlePausable(this);
    }
}