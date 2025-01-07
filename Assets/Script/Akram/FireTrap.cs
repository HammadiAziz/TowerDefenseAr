using System.Collections; // Required for IEnumerator
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public GameObject firePrefab; // Fire prefab to spawn
    public Transform[] fireSpawnPoints; // Empty objects where fire will spawn
    public float trapDuration = 2f; // Duration fire stays active
    public float damage = 15f; // Damage dealt by the fire
    public float activationRange = 0.02f; // Distance within which the trap activates

    private bool trapActivated = false; // Ensure the trap activates only once

    private void Update()
    {
        // Check if the trap is ready to activate
        if (!trapActivated)
        {
            // Find all enemies tagged as "Enemy"
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                if (enemy == null) continue;

                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                // Activate the trap for each enemy within range
                if (distanceToEnemy <= activationRange)
                {
                    ActivateTrap(enemy);
                }
            }
        }
    }

    private void ActivateTrap(GameObject enemy)
    {
        // Instantiate the fire prefab at each specified spawn point
        foreach (Transform spawnPoint in fireSpawnPoints)
        {
            if (spawnPoint == null) continue;

            GameObject fire = Instantiate(firePrefab, spawnPoint.position, Quaternion.identity);

            // Destroy the fire object after the trap duration
            Destroy(fire, trapDuration);
        }

        // Apply damage to the enemy
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        // Start cooldown for reactivation
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        trapActivated = true;
        yield return new WaitForSeconds(trapDuration + 4); // Wait for the fire duration + cooldown
        trapActivated = false; // Reset the trap for reactivation
    }
}
