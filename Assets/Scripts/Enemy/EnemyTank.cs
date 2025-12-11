using UnityEngine;

public class EnemyTank : Enemy
{
    protected override void Start()
    {
        base.Start();

        // Override stats
        speed = 4f;
        maxHealth = 5;
        damage = 2;
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