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
    private void Update()
    {
        //Movement
        playerRb.linearVelocity = moveInput * speed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}