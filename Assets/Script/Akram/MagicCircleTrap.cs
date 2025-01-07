using System.Collections;
using UnityEngine;

public class MagicCircleTrap : MonoBehaviour
{
    public GameObject magicBallPrefab; // Prefab for the magic ball
    public Transform[] spawnPoints; // Points where magic balls will spawn
    public float activationRadius = 2f; // Radius within which the trap detects enemies
    public float fireRate = 0.5f; // Delay between firing magic balls
    public float trapDuration = 2f; // Duration for firing magic balls
    public float cooldownTime = 5f; // Cooldown time before the trap can activate again
    public int damagePerBall = 2; // Damage dealt by each magic ball

    private GameObject enemyPrefab; // Reference to the detected enemy
    private bool trapActivated = false; // To prevent reactivation during cooldown

    private void Start()
    {
        // Automatically find and assign the enemy tagged as "Enemy" in the scene
        if (enemyPrefab == null)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy != null)
            {
                enemyPrefab = enemy;
            }
        }
    }

    private void Update()
    {
        if (!trapActivated && enemyPrefab != null)
        {
            // Check if the enemy is within the activation radius
            float distanceToEnemy = Vector3.Distance(transform.position, enemyPrefab.transform.position);
            if (distanceToEnemy <= activationRadius)
            {
                ActivateTrap();
            }
        }
    }

    private void ActivateTrap()
    {
        trapActivated = true;

        // Start firing magic balls
        StartCoroutine(FireMagicBalls());

        Debug.Log("Magic Circle Trap Activated!");

        // Start the cooldown
        StartCoroutine(StartCooldown());
    }

    private IEnumerator FireMagicBalls()
    {
        float elapsedTime = 0f;
        int spawnIndex = 0; // To track the current spawn point in the loop

        while (elapsedTime < trapDuration)
        {
            // Fire magic balls from each spawn point one by one
            if (magicBallPrefab != null && spawnPoints.Length > 0)
            {
                // Instantiate and launch magic balls at the current spawn point
                GameObject magicBall = Instantiate(magicBallPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
                MagicBall ballScript = magicBall.GetComponent<MagicBall>();
                if (ballScript != null && enemyPrefab != null)
                {
                    ballScript.Initialize(enemyPrefab.transform, damagePerBall);
                }

                // Move to the next spawn point
                spawnIndex = (spawnIndex + 1) % spawnPoints.Length;
            }

            elapsedTime += fireRate;  // Increase elapsed time by the fireRate
            yield return new WaitForSeconds(fireRate); // Wait for fireRate duration before firing the next magic ball
        }
    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldownTime); // Wait for cooldown
        trapActivated = false; // Reset the trap for reactivation
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the activation radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}
