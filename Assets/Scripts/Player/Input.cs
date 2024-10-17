using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    public Vector2 MoveVector { get; private set; }

    void OnMove(InputValue inputValue) => MoveVector = inputValue.Get<Vector2>();
}
