using UnityEngine;


public class WarmManager : MonoBehaviour
{
    [SerializeField] bool SwitchMecheroBaño; // true-> mechero false-> baño maria

    public bool getSwitch()
    {
        return SwitchMecheroBaño;
    }


}
