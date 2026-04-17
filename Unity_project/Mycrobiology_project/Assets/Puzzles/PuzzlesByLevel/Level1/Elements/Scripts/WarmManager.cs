using UnityEngine;
using UnityEngine.UI;


public class WarmManager : MonoBehaviour
{
    [SerializeField] bool SwitchMecheroBaño; // true-> mechero false-> baño maria
    [SerializeField] private float fillSpeed = 2f;
    [SerializeField] Image barrTermometer;
    private float targetFill = 0f;
    void Start()
    {
        ChangeState(false);
    }
    public bool getSwitch()
    {
        return SwitchMecheroBaño;
    }
    public void newTemperature(float temperature)
    {
        // Convertir de 0–100 a 0–1
        targetFill = Mathf.Clamp01(temperature / 100f);
    }
    public void ChangeState(bool state)
    {
        barrTermometer.transform.parent.gameObject.SetActive(state);
        if(!state)newTemperature(0);
    }
    private void Update()
    {
        // Movimiento suave hacia el objetivo
        barrTermometer.fillAmount = Mathf.MoveTowards(
            barrTermometer.fillAmount,
            targetFill,
            fillSpeed * Time.deltaTime
        );
    }


}
