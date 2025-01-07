using UnityEngine;

public class Bullettt : MonoBehaviour
{
    public float damage = 50f; // Damage dealt by the bullet

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Bullet collided with: {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"Enemy hit by bullet: {collision.gameObject.name}");
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // Inflict damage on the enemy
            }

            Destroy(gameObject); // Destroy the bullet
        }
        else
        {
            Debug.Log($"Ignored collision with: {collision.gameObject.name}");
        }
    }
}