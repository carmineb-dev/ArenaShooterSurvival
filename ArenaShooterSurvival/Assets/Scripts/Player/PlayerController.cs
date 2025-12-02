using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D playerRb;
    private Vector2 moveInput;

    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireCooldown = 0.3f;
    private float nextFireTime = 0f;

    [SerializeField] private int playerHealth = 5;
    private bool isInvincible = false;
    [SerializeField] private float invincibilitySeconds = 1.5f;
    private SpriteRenderer spriteRenderer;

    private bool canMove = true;

    public HealthUI healthUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.Find("Model").GetComponent<SpriteRenderer>();

        // Set starting HealthUI
        healthUI.UpdateHealth(playerHealth);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (canMove)
        {// Player movement
            playerRb.linearVelocity = moveInput * speed;
        }

        // Rotation
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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        // Rotate
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Input Fire
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    // Player takes damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isInvincible && collision.gameObject.CompareTag("Enemy"))
        {
            // Takes damage
            playerHealth--;

            // Refresh HealthUI
            healthUI.UpdateHealth(playerHealth);

            // Apply knockback
            Knockback(collision.transform);

            // Player dies
            if (playerHealth <= 0)
            {
                Destroy(gameObject);
            }

            // Starts invincibility timer
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        // Start invincibility
        isInvincible = true;

        canMove = false;

        // Elapsed time since the start of flashing
        float elapsedTime = 0f;

        // Save original color
        Color originalColor = spriteRenderer.color;

        // Flashing speed
        float flashSpeed = 5;

        // Flashing loop
        while (elapsedTime < invincibilitySeconds)
        {
            // Calculate alpha value with pingpong
            float alpha = Mathf.PingPong(Time.time * flashSpeed, 1);

            // Apply alpha value to the model of the player
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            // Add time
            elapsedTime += Time.deltaTime;

            // Wait next frame
            yield return null;
        }

        // Apply original color to the model of the player
        spriteRenderer.color = originalColor;

        // Ending invincibility
        isInvincible = false;

        canMove = true;
    }

    private void Knockback(Transform enemy)
    {
        float knockbackStrength = 2f;
        Vector2 knockbackDirection = (transform.position - enemy.position).normalized;
        playerRb.AddForce(knockbackDirection * knockbackStrength, ForceMode2D.Impulse);
    }
}