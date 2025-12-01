using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D playerRb;
    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        // Player movement
        playerRb.linearVelocity = moveInput * speed;

        RotatePlayer();
    }

    // Rotate Player
    public void RotatePlayer()
    {
        // Calculate Mouse Position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate distance between player and mouse
        Vector2 direction = mousePosition - (Vector2)transform.position;

        // Calculate angle between mouse and player (90 degree offset cause of the sprite)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;

        // Rotate
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}