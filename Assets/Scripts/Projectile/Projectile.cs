using Unity.Cinemachine;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // === PROJECTILE  ===
    [SerializeField] private float speed = 20f;

    [SerializeField] private float projectileLifeTime = 0.5f;

    private Rigidbody2D projectileRb;

    private void Start()
    {
        projectileRb = GetComponent<Rigidbody2D>();
        projectileRb.linearVelocity = transform.up * speed;

        // Destroy the projectile after a certain time
        Destroy(gameObject, projectileLifeTime);
    }

    // Destroy the projectile when hits something
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the projectile collide with the player or with arena bounds, ignore the collision and continue
        if (collision.CompareTag("Player") || collision.CompareTag("ArenaBounds"))
        {
            return;
        }
        Destroy(gameObject);
    }
}