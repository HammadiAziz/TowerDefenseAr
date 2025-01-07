using UnityEngine;

public class Enemyy : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Destroy enemy object
        Destroy(gameObject);
    }
}
