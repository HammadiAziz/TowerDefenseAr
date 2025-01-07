using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f; // Starting health of the enemy

    public void TakeDamage(float damage)
    {
        health -= damage; // Reduce health
        Debug.Log("Enemy took " + damage + " damage. Remaining health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died.");
        Destroy(gameObject); // Destroy the enemy object
    }
}
