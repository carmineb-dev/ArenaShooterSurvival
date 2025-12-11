using UnityEngine;

public class EnemyFast : Enemy
{
    protected override void Start()
    {
        base.Start();

        // Override stats
        maxHealth = 1;
        speed = 6f;
        currentHealth = maxHealth;
    }

    protected void FixedUpdate()
    {
        // Disable movement if is spawning or dead
        if (!canMove)
        {
            return;
        }

        // Chase movement
        Vector2 direction = (player.position - transform.position).normalized;
        enemyRb.MovePosition(enemyRb.position + direction * speed * Time.fixedDeltaTime);

        // Rotate following player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemyRb.rotation = angle;
    }
}