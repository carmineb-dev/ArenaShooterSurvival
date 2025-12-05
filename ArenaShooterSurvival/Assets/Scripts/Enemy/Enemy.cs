using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // === SHARED STATS ===
    [SerializeField] protected int maxHealth = 2;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected float speed = 5f;

    // === SHARED REFERENCES ===
    protected Rigidbody2D enemyRb;
    protected Transform player;
    protected SpawnManager spawnManager;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Collider2D enemyCollider;

    // === SHARED STATE ===
    protected bool isDead = false;

    // === SHARED EFFECTS ===
    [SerializeField] protected GameObject deathEffectPrefab;

    // === INITIALIZATION ===
    protected virtual void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        spawnManager = FindFirstObjectByType<SpawnManager>();
    }

    // Initialize the enemy with the player reference passed by spawnmanager
    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
    }

    //=== MOVEMENT ===

    protected virtual void FixedUpdate()
    {
        // If enemy is dead disable movement
        if (isDead)
        {
            return;
        }
    }

    // === DAMAGE ===

    // Collision with the projectile
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            TakeDamage(1);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        // Decrement health
        currentHealth -= damage;

        // Hit visual effect
        StartCoroutine(HitFlash(0.1f));

        // Death of enemy
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // === DEATH ===
    protected virtual void Die()
    {
        isDead = true;

        //Disable collider
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        // Shows particle effect
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Decrease enemy count in Spawn Manager
        spawnManager?.EnemyDestroyed();

        // Start fadeout
        StartCoroutine(FadeOut(0.5f));
    }

    // === VISUAL EFFECTS ===

    // Fade out
    protected IEnumerator FadeOut(float duration)
    {
        float startAlpha = spriteRenderer.color.a;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0, elapsed / duration);
            Color newColor = spriteRenderer.color;
            newColor.a = alpha;
            spriteRenderer.color = newColor;
            yield return null;
        }
        Destroy(gameObject);
    }

    // Hit
    protected IEnumerator HitFlash(float duration)
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }
}