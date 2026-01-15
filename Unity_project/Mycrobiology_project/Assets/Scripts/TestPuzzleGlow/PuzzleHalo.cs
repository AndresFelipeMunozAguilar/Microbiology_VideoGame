using UnityEngine;

public class PuzzleHalo : MonoBehaviour
{

    // Que tan rápido pulsa el glow
    public float pulseSpeed = 2f;
    // Que tan grande se puede hacer el glow
    public float maxScale = 1.4f;

    SpriteRenderer haloSr;
    Vector3 haloBaseScale;

    // Punto más bajo de la transparencia del glow
    public float minimumTransparency = 0.25f;

    void Awake()
    {
        haloSr = GetComponent<SpriteRenderer>();
        haloBaseScale = Vector3.one;
        transform.localScale = haloBaseScale;
    }

    void Update()
    {
        // Generar valores entre 0 y 1,
        // basado en el tiempo de inicio del frame actual
        float drawTime = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;

        Pulse(drawTime);
        Glow(drawTime);

    }

    // Función que transforma la escala del halo pulsantemente
    void Pulse(float referenceTime)
    {

        float stretchFactor = Mathf.Lerp(1f, maxScale, referenceTime);
        haloSr.transform.localScale = haloBaseScale * stretchFactor;

    }

    // Función que determina la transparencia del halo
    // como si brillara
    void Glow(float referenceTime)
    {

        Color haloColor = haloSr.color;
        haloColor.a = Mathf.Lerp(minimumTransparency, haloSr.color.a, referenceTime);
        haloSr.color = haloColor;

    }

}
