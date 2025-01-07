using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleTrap : MonoBehaviour
{
    public float pullRadius = 2f; // Radius within which enemies get pulled
    public float pullForce = 8f; // Force of gravity pulling enemies to the center
    public float trapDuration = 2f; // Time enemies stay stuck at the center
    public float cooldownTime = 20f; // Time before the trap can reactivate (must be greater than trapDuration)
    public float damage = 10f; // Damage dealt while stuck

    private bool trapActivated = false; // Prevent reactivation during the effect

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

                // Activate the trap if the enemy is within range
                if (distanceToEnemy <= pullRadius)
                {
                    ActivateTrap(enemy);
                }
            }
        }
    }

    private void ActivateTrap(GameObject enemy)
    {
        trapActivated = true;

        // Start pulling the enemy to the trap's center
        StartCoroutine(PullEnemyToBlackHole(enemy));

        // Trigger any visual or audio effects (optional)
        Debug.Log("Black Hole Trap Activated!");

        // Start cooldown for reactivation (longer than trap duration)
        StartCoroutine(StartCooldown());
    }

    private IEnumerator PullEnemyToBlackHole(GameObject enemy)
    {
        float elapsedTime = 0f;
        Animator enemyAnimator = enemy.GetComponent<Animator>();

        if (enemyAnimator != null)
        {
            enemyAnimator.enabled = false; // Stop animation
        }

        while (elapsedTime < trapDuration)
        {
            if (enemy == null) yield break;

            // Pull enemy toward the center of the trap along X and Z axes only
            Vector3 trapPosition = transform.position;
            Vector3 enemyPosition = enemy.transform.position;

            Vector3 directionToCenter = new Vector3(
                trapPosition.x - enemyPosition.x,
                0, // Ignore Y-axis movement
                trapPosition.z - enemyPosition.z
            ).normalized;

            enemy.transform.position += directionToCenter * pullForce * Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Apply damage after trap effect ends
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        // Resume animation
        if (enemyAnimator != null)
        {
            enemyAnimator.enabled = true; // Resume animation
        }
    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldownTime); // Wait for cooldown (longer than trap duration)
        trapActivated = false; // Reset the trap for reactivation
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the pull radius in the Scene view for visualization
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
