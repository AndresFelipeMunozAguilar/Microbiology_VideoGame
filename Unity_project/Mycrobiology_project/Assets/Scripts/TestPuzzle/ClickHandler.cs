using UnityEngine;
using UnityEngine.Events;

public class ClickHandler : MonoBehaviour
{
    [SerializeField]
    private UnityEvent clicked;

    private MouseInputProvider mouse;

    public void Start()
    {
        mouse = FindAnyObjectByType<MouseInputProvider>();
        mouse.Greet();
        mouse.Clicked += MouseOnClicked;
    }

    public void MouseOnClicked()
    {
        clicked.Invoke();
    }
}
