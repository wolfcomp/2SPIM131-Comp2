using UnityEngine;

public class Movement : MonoBehaviour
{
    [Tooltip("The acceleration and max moveSpeed of the player character")]
    [SerializeField] 
    private float _moveSpeed = 7f;

    // a multiplier to the moveSpeed in order to keep the numbers relatively low and manageable
    private float _movementMultiplier = 10f; 

    [SerializeField]
    private Rigidbody2D _rb;

    [Tooltip("The amount of slowdown on the player. Contributes to how quickly the player stops moving after you stop pressing the direction")]
    [SerializeField]private float _drag = 15f;

    private void Drag()
    {
        Vector2 velocity = transform.InverseTransformDirection(_rb.velocity);
        float forceX;
        float forceY;
        forceX = -_drag * velocity.x;
        forceY = -_drag * velocity.y;
        // adds "negative force" to the player, as to work as drag
        _rb.AddRelativeForce(new Vector2(forceX, forceY)); 
    }

    // Called from PlayerController, The act of adding force in the direction gotten from the actionMap
    public void Move(Vector2 moveVector) {
        
        _rb.AddForce(moveVector.normalized * (_moveSpeed * _movementMultiplier), ForceMode2D.Force);
    }
}
