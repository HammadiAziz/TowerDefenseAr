using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;        // Enemy health
    public float speed = 2f;          // Movement speed
    private Transform tower;

    private bool isBeingAttracted = false; // Flag to pause movement during attraction

    private float originalSpeed;      // Store the original speed
    private bool isSlowed = false;    // Prevent stacking slow effects

    void Start()
    {
        originalSpeed = speed; // Save the initial speed

        // Find the tower tagged as "Tour"
        GameObject towerObject = GameObject.FindWithTag("Tour");
        if (towerObject != null)
        {
            tower = towerObject.transform;
        }
        else
        {
            Debug.LogError("No GameObject with the 'Tour' tag found!");
        }
    }

    void Update()
    {
        if (!isBeingAttracted) // Only move toward the tower if not being attracted
        {
            MoveTowardsTower();
        }
    }

    void MoveTowardsTower()
    {
        if (tower != null)
        {
            Vector3 direction = (tower.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log($"Enemy took {damageAmount} damage. Remaining health: {health}");

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }

    // Method to pause movement when being attracted
    public void StartAttraction()
    {
        isBeingAttracted = true;
    }

    // Method to resume movement after attraction
    public void EndAttraction()
    {
        isBeingAttracted = false;
    }

    // Apply slowing effect
    public void ApplySlow(float slowMultiplier, float duration)
    {
        if (isSlowed) return; // Prevent stacking slow effects

        isSlowed = true;
        speed *= slowMultiplier; // Reduce speed

        Invoke(nameof(RemoveSlow), duration); // Schedule to remove the slow effect after duration
    }

    // Remove slowing effect
    private void RemoveSlow()
    {
        speed = originalSpeed; // Restore original speed
        isSlowed = false;
    }
}
