using UnityEngine;
using UnityEngine.UI;

public class WarmManager : MonoBehaviour
{
    [SerializeField] Transform warmPoint;
    [SerializeField] Image WarmBar;
    [SerializeField] float speedWarm;

    bool IsWarming;

    void Update()
    {
        if (IsWarming)
        {
            WarmBar.fillAmount+=0.01f*speedWarm*Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<ElementManager>(out ElementManager element))
        {
            IsWarming=true;
            element.PlaceWarm(warmPoint.position);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<ElementManager>(out ElementManager element))
        {
            IsWarming=false;
            WarmBar.fillAmount=0;
        }
    }


}
