using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public GameObject spikePrefab; // Spike prefab to spawn
    public float trapDuration = 2f; // Time enemy stays stuck
    public float damage = 15f; // Damage dealt when stuck
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
        // Instantiate the spike prefab at the trap location
        GameObject spike = Instantiate(spikePrefab, transform.position, Quaternion.identity);

        // Apply the stuck effect to the enemy and damage
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        // Destroy the spike object after the trap duration
        Destroy(spike, trapDuration);

        // Start cooldown for reactivation
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        trapActivated = true;
        yield return new WaitForSeconds(trapDuration + 4); // Wait for the stuck duration + cooldown
        trapActivated = false; // Reset the trap for reactivation
    }
}
