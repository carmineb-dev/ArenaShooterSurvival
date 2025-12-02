using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    public Transform Player;

    [SerializeField] private float speed = 5f;
    private Rigidbody2D enemyRb;

    [SerializeField] private int enemyHealth = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Movement toward the player
        Vector2 direction = (Player.position - transform.position).normalized;
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
            if (enemyHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}