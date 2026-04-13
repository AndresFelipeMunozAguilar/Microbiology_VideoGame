using UnityEngine;

public class ContainerManager : MonoBehaviour
{
    [SerializeField]
    Containers type;

    public Containers getType()
    {
        return type;
    }
}
