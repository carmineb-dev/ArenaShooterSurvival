using UnityEngine;

public class EnemyTank : Enemy
{
    protected override void Start()
    {
        base.Start();

        // Override stats
        speed = 3f;
        maxHealth = 5;
        currentHealth = maxHealth;
    }

    protected override void FixedUpdate()
    {
        // If enemy is dead disable movement
        if (isDead)
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