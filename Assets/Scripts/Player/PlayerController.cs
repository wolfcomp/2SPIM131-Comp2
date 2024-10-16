using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Input))]
[RequireComponent(typeof(Movement))]
public class PlayerController : MonoBehaviour
{
    private Input _input;
    private Movement _movement;
    private void Awake() {
        _movement = GetComponent<Movement>();
        _input = GetComponent<Input>();
    }

    private void FixedUpdate() {
        //foreach (var animator in animators) {
        //    if (animator.gameObject != _split.mainClones[_split.selectedMain].gameObject)
        //        continue;
        //    if (_input.moveMainVector.x == 0 && _input.moveMainVector.y == 0) 
        //        return;
        //}
        _movement.Move(_input.moveVector);
    }
}
