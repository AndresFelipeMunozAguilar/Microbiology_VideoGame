using UnityEngine;

public class Pc_Mobile_Changes : MonoBehaviour
{
    [Header("Objeto UI 1")]
    public RectTransform objeto1;
    public Vector2 posicionPcObjeto1;

    [Header("Objeto UI 2")]
    public RectTransform objeto2;
    public Vector2 posicionPcObjeto2;

    [Header("Objeto UI 3")]
    public RectTransform objeto3;
    public Vector2 posicionPcObjeto3;

    void Start()
    {
        if (!Application.isMobilePlatform)
        {
            Debug.Log("PC WEB");

            if (objeto1 != null)
                objeto1.anchoredPosition = posicionPcObjeto1;

            if (objeto2 != null)
                objeto2.anchoredPosition = posicionPcObjeto2;

            if (objeto3 != null)
                objeto3.anchoredPosition = posicionPcObjeto3;
        }

        if (Application.isMobilePlatform)
        {
            Debug.Log("CELULAR WEB");

            // No hacer nada en celular
        }
    }
}