using System.Collections;
using UnityEngine;

public class ZonaDesecho : MonoBehaviour
{
    [Header("Método que representa esta zona")]
    [SerializeField] private MetodoDescontaminacion metodoZona;

    [Header("Punto opcional de feedback")]
    [SerializeField] private Transform feedbackPoint;

    [Header("Animación de zona")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite spriteDisponible;
    [SerializeField] private Sprite spriteOcupado;
    [SerializeField] public float tiempoOcupado = 4f;
    [SerializeField] private bool bloquearMientrasEstaOcupado = true;

    [Header("Feedback cuando está ocupada")]
    [SerializeField] private string feedbackOcupado = "Ocupado";

    private bool ocupado = false;
    private Coroutine procesoActual;
    gameplayDesechos gamePlay;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        gamePlay  = transform.GetComponentInParent<gameplayDesechos>();
    }

    public MetodoDescontaminacion GetMetodoZona()
    {
        return metodoZona;
    }

    public bool EstaOcupado()
    {
        return ocupado;
    }

    public bool PuedeRecibir()
    {
        if (!bloquearMientrasEstaOcupado)
        {
            return true;
        }

        return !ocupado;
    }

    public string GetFeedbackOcupado()
    {
        return feedbackOcupado;
    }

    public Vector2 GetFeedbackPosition()
    {
        if (feedbackPoint != null)
        {
            return feedbackPoint.position;
        }

        if (TryGetComponent(out DropZone dropZone))
        {
            return dropZone.getPosition();
        }

        return transform.position;
    }

    public void ActivarZona()
    {
        if (procesoActual != null)
        {
            StopCoroutine(procesoActual);
        }

        procesoActual = StartCoroutine(ProcesoZona());
    }

    private IEnumerator ProcesoZona()
    {
        ocupado = true;
        SetSpriteOcupado();
        yield return new WaitForSeconds(tiempoOcupado);

        ocupado = false;
        SetSpriteDisponible();

        procesoActual = null;
    }

    private void SetSpriteDisponible()
    {
        if (spriteRenderer != null && spriteDisponible != null)
        {
            spriteRenderer.sprite = spriteDisponible;
            gamePlay.CorrectFeedback(transform.position);
            Invoke("CorrectElement",1f);
        }
    }

    void CorrectElement()
    {
        Debug.Log("[Desechos] completado");
        gamePlay.addFinishElement();
    }
    private void SetSpriteOcupado()
    {
        if (spriteRenderer != null && spriteOcupado != null)
        {
            spriteRenderer.sprite = spriteOcupado;
        }
    }
}