using UnityEngine;

public class ElementManager : MonoBehaviour
{
    Element element;

    public void CreateElement(Element assigned)
    {
        element= assigned;
        GetComponent<SpriteRenderer>().sprite = element.image;        
    }
}
