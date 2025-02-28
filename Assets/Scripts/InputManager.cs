using System;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Vector2> OnMove = new UnityEvent<Vector2>();
    public UnityEvent OnSpacePressed = new UnityEvent();
    public UnityEvent OnResetPressed = new UnityEvent();

    private void Update()
    {
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            input += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            input += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            input += Vector2.right;
        }
        OnMove?.Invoke(input);

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnResetPressed?.Invoke();
        }
    }
}