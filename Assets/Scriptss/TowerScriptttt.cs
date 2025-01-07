using System.Collections;
using UnityEngine;

public class TowerScriptttt : MonoBehaviour
{
    Transform targetEnemy;
    [SerializeField] Transform head; // The tower's head, where bullets will spawn
    [SerializeField] GameObject bulletPrefab; // Prefab for bullets
    [SerializeField] float bulletVelocity = 100f;
    [SerializeField] float bulletLifeTime = 3f;
    [SerializeField] float shootingCooldown = 1f;

    private float waitTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"Enemy detected: {other.name}");
            if (targetEnemy == null || Vector3.Distance(transform.position, targetEnemy.position) > Vector3.Distance(transform.position, other.transform.position))
            {
                targetEnemy = other.transform;
                Debug.Log($"Target set to: {targetEnemy.name}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.transform == targetEnemy)
        {
            Debug.Log($"Enemy exited: {other.name}");
            targetEnemy = null;
        }
    }

    private void Update()
    {
        if (targetEnemy != null)
        {
            head.LookAt(targetEnemy.position, Vector3.up);

            // Only shoot if cooldown has elapsed
            if (waitTime <= 0)
            {
                Shoot(targetEnemy.position);
                waitTime = shootingCooldown; // Reset cooldown
            }
        }

        // Reduce cooldown timer
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
    }

    void Shoot(Vector3 targetPosition)
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned!");
            return;
        }

        // Instantiate a bullet at the tower head's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, head.position, head.rotation);
        Debug.Log($"Bullet spawned at {head.position}");

        // Apply force to the bullet to move it towards the target
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            Vector3 direction = (targetPosition - head.position).normalized;
            rb.AddForce(direction * bulletVelocity, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Bullet is missing a Rigidbody!");
        }

        // Destroy the bullet after its lifetime
        StartCoroutine(DestroyBullet(bullet));
    }

    IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifeTime);
        if (bullet != null)
        {
            Destroy(bullet);
        }
    }
}
