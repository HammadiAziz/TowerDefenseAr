using UnityEngine;
using System.Collections;

public class CrystalDebuff : MonoBehaviour
{
    public float debuffRadius = 10f;      // Radius within which enemies are affected
    public float slowMultiplier = 0.5f;  // Factor to reduce the enemy speed (e.g., 0.5 for 50% speed)
    public float debuffDuration = 2f;    // Duration of the debuff effect in seconds
    public float checkInterval = 1f;     // How often to apply the debuff (seconds)

    private void Start()
    {
        // Repeat the ApplyDebuff method at regular intervals
        InvokeRepeating(nameof(ApplyDebuff), 0f, checkInterval);
    }

    private void ApplyDebuff()
    {
        // Find all colliders within the debuff radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, debuffRadius);

        foreach (var hitCollider in hitColliders)
        {
            // Check if the object has the tag "Enemy"
            if (hitCollider.CompareTag("Enemy"))
            {
                // Check if the object has an EnemyBehavior component
                EnemyBehavior enemy = hitCollider.GetComponent<EnemyBehavior>();
                if (enemy != null)
                {
                    // Apply the slowing effect to the enemy
                    StartCoroutine(ApplySlow(enemy));
                }
            }
        }
    }

    private IEnumerator ApplySlow(EnemyBehavior enemy)
    {
        float originalSpeed = enemy.moveSpeed;
        enemy.moveSpeed *= slowMultiplier; // Reduce speed

        yield return new WaitForSeconds(debuffDuration);

        enemy.moveSpeed = originalSpeed; // Restore original speed
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the debuff radius in the editor
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, debuffRadius);
    }
}
