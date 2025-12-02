using Unity.Cinemachine;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float projectileTime = 0.5f;
    private Rigidbody2D projectileRb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        projectileRb = GetComponent<Rigidbody2D>();
        projectileRb.linearVelocity = transform.up * speed;
        // Destroy the projectile after a certain time
        Destroy(gameObject, projectileTime);
    }

    // Destroy the projectile when hits something
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            return;
        }
        Destroy(gameObject);
    }
}