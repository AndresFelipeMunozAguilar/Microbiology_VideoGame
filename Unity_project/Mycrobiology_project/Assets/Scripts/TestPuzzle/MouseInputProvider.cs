using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputProvider : MonoBehaviour
{
    public event Action Clicked;

    public void Greet()
    {
        Debug.Log("Hello from MouseInputProvider");
    }

    public void OnAction(InputValue _)
    {
        Clicked.Invoke();
        Debug.Log("Mouse Clicked");
    }
}
