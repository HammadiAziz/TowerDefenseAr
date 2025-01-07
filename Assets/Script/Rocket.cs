using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    public float damage = 100f;
    public float explosionRadius = 5f;  // Radius within which enemies are affected
    public float explosionForce = 10f;  // Force applied to enemies (optional)
    public GameObject explosionEffect;  // Optional: assign a particle effect (like an explosion prefab) for visual feedback

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the rocket collided with something
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground"))
        {
            // Handle the explosion damage
            Explode();

            // Destroy the rocket (or handle differently based on your design)
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        // Optional: Create an explosion visual effect at the rocket's position
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Find all enemies within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Check if the object is an enemy and apply damage
            if (nearbyObject.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = nearbyObject.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }

                // Optionally, apply force to the enemy (e.g., knockback effect)
                Rigidbody enemyRigidbody = nearbyObject.GetComponent<Rigidbody>();
                if (enemyRigidbody != null)
                {
                    enemyRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }
    }
}
