using UnityEngine;

public class TubeManager : MonoBehaviour
{
    Animator anim;
    Element element;
    bool isFill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Filling(Element newElement)
    {
        anim.Play("Filling");
        element = newElement;
    }
}
