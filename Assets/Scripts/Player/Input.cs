using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    public Vector2 moveVector { get; private set; }
    void OnMove(InputValue inputValue) => moveVector = inputValue.Get<Vector2>();
}
