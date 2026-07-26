using UnityEngine;

public class Raycaster : MonoBehaviour
{

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void ProcessTap(Vector2 screenPosition)
    {
        Debug.Log("Procesando tap en Raycaster");
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);

        RaycastHit2D rayHit = Physics2D.GetRayIntersection(ray);

        Debug.Log($"<color=orange>Raycaster:</color> RaycastHit2D detectó colision con {(rayHit.collider != null ? rayHit.collider.gameObject.name : "ningún objeto")}");

        if (!rayHit.collider) return;

        Debug.Log($"Se hizo tap sobre el objeto {rayHit.collider.gameObject.name}");

        rayHit.collider.TryGetComponent<ITappable>(out ITappable tappable);

        if (tappable == null) return;

        Debug.Log("El objeto es ITappable, llamando OnTap()");
        tappable.OnTap();
    }
}