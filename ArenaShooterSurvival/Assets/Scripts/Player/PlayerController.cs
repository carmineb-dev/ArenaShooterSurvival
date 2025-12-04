using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // === PLAYER SPEED ===
    [SerializeField] private float speed = 5f;

    // === RIGIDBODY ===
    private Rigidbody2D playerRb;

    // === MOVEMENT ===
    private Vector2 moveInput;

    // === SHOOTING ===
    public Transform firePoint;

    public GameObject projectilePrefab;
    public float fireCooldown = 0.3f;
    private float nextFireTime = 0f;

    // === HEALTH ===
    public GameUI healthUI;

    [SerializeField] private int playerHealth = 5;

    public bool isGameOver = false;

    // === INVINCIBILITY ===
    private bool isInvincible = false;

    [SerializeField] private float invincibilitySeconds = 1.5f;
    [SerializeField] private float flashSpeed = 5f;

    // === SPRITE MODEL ===
    [SerializeField] private SpriteRenderer spriteRenderer;

    // === CAMERA ===
    private Camera mainCamera;

    // === KNOCKBACK ===
    private Vector2 knockbackVelocity = Vector2.zero;

    [SerializeField] private float knockbackDecay = 5f;
    [SerializeField] private float knockbackStrength = 10f;

    // === AUDIO ===
    [SerializeField] private AudioClip hitSound;

    [SerializeField] private AudioClip shootSound;

    // === START ===
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Initialize rigidbody
        playerRb = GetComponent<Rigidbody2D>();

        // Set starting HealthUI
        healthUI.UpdateHealth(playerHealth);

        // Set camera cache
        mainCamera = Camera.main;
    }

    // === UPDATE ===
    // Update is called once per frame
    private void FixedUpdate()
    {
        // Combine player movement + knockback
        Vector2 moveVelocity = moveInput * speed;
        playerRb.linearVelocity = moveVelocity + knockbackVelocity;

        // Knockback gradually decrease
        knockbackVelocity = Vector2.Lerp(knockbackVelocity, Vector2.zero, knockbackDecay * Time.fixedDeltaTime);

        // Rotation
        RotatePlayer();
    }

    // === ROTATE PLAYER ===
    public void RotatePlayer()
    {
        // Calculate Mouse Position
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

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

    // === INPUT FIRE ===
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
        AudioManager.instance.playSfx(shootSound, 0.5f);
    }

    // === PLAYER TAKES DAMAGE ===
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

            // Play hit audio
            AudioManager.instance.playSfx(hitSound, 0.5f);

            // Player dies
            if (playerHealth <= 0)
            {
                isGameOver = true;
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

        // Elapsed time since the start of flashing
        float elapsedTime = 0f;

        // Save original color
        Color originalColor = spriteRenderer.color;

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
    }

    private void Knockback(Transform enemy)
    {
        // Calculate knockback direction
        Vector2 knockbackDirection = (transform.position - enemy.position).normalized;

        // Set knockback velocity
        knockbackVelocity = knockbackDirection * knockbackStrength;
    }
}