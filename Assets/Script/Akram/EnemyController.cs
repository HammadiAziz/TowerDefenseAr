using System.Collections;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform[] targets; // Array of target objects assigned via Inspector
    public float speed = 0.5f; // Movement speed
    public float stopDistance = 0.01f; // Distance to stop from a target
    public float rotationSpeed = 2f; // Speed of rotation toward the target

    [Header("Health Settings")]
    public float health = 100f; // Enemy health
    public TextMeshPro healthText; // Assign TextMeshPro component in Inspector

    private int currentTargetIndex = 0; // Index of the current target
    private bool isStuck = false; // Indicates if the enemy is stuck in a trap

    private Animator animator; // Reference to the Animator component

    private void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the enemy!");
        }
    }

    private void Update()
    {
        // Update health display
        if (healthText != null)
        {
            healthText.text = health.ToString("F0");
        }

        if (targets.Length == 0 || isStuck) return;

        Transform target = targets[currentTargetIndex];
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > stopDistance)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            currentTargetIndex = (currentTargetIndex + 1) % targets.Length;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log($"Enemy {gameObject.name} is destroyed.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"Enemy {gameObject.name} took {damage} damage. Remaining health: {health}");
        }

        // Update the health display
        if (healthText != null)
        {
            healthText.text = health.ToString("F0");
        }
    }

    public void ApplyStuck(float duration, float damage)
    {
        if (!isStuck)
        {
            Debug.Log($"Enemy {gameObject.name} is stuck for {duration} seconds.");
            isStuck = true;

            // Stop animation
            if (animator != null)
            {
                animator.enabled = false; // Disable Animator
            }

            StartCoroutine(StuckEffect(duration, damage));
        }
    }

    private IEnumerator StuckEffect(float duration, float damage)
    {
        TakeDamage(damage);

        yield return new WaitForSeconds(duration);

        isStuck = false;

        // Resume animation
        if (animator != null)
        {
            animator.enabled = true; // Re-enable Animator
        }

        currentTargetIndex = (currentTargetIndex + 1) % targets.Length;
    }
}
