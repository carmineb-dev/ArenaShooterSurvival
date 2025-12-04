using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    // === REFERENCE TO PLAYER ===
    private GameObject player;

    // === REFERENCE TO SPAWN MANAGER ===
    private SpawnManager spawnManager;

    // === ENEMY ===
    [SerializeField] private float speed = 5f;

    private Rigidbody2D enemyRb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D enemyCollider;

    [SerializeField] private int enemyHealth = 2;

    private bool isDead = false;

    // === EFFECTS ===
    [SerializeField] private GameObject deathEffectPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // If enemy is dead disable movement
        if (isDead)
        {
            return;
        }

        // Movement toward the player
        Vector2 direction = (player.transform.position - transform.position).normalized;
        enemyRb.MovePosition(enemyRb.position + direction * speed * Time.fixedDeltaTime);

        // Rotate following player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemyRb.rotation = angle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            enemyHealth--;

            StartCoroutine(HitFlash(0.1f));

            if (enemyHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;

        // Disable collider
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Decrease enemy count in Wave manager
        spawnManager?.EnemyDestroyed();

        // Start fadeout
        StartCoroutine(FadeOut(0.5f));
    }

    private IEnumerator FadeOut(float duration)
    {
        isDead = true;
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

    private IEnumerator HitFlash(float duration)
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }

    public void SetSpawnManager(SpawnManager manager)
    {
        spawnManager = manager;
    }
}