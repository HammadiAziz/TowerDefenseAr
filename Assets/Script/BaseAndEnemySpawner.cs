using System.Collections;
using UnityEngine;

public class BaseAndEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public float spawnDistance = 10f; // Distance from the base where the enemy will spawn
    public Transform baseTransform; // Reference to the main base Transform
    public float spawnInterval = 3f; // Time in seconds between enemy spawns

    private void Start()
    {
        if (gameObject.CompareTag("Base"))
        {
            baseTransform = transform; // Assign the base transform
            StartCoroutine(SpawnEnemiesOverTime());
        }
        else
        {
            Debug.LogError("Base object does not have the 'Base' tag.");
        }
    }

    IEnumerator SpawnEnemiesOverTime()
    {
        while (true)
        {
            SpawnEnemy(); // Spawn an enemy
            yield return new WaitForSeconds(spawnInterval); // Wait before spawning the next
        }
    }

    void SpawnEnemy()
    {
        // Randomize spawn position around the base, but within a reasonable distance
        Vector3 spawnPosition = baseTransform.position + new Vector3(
            Random.Range(-spawnDistance, spawnDistance),
            0,
            Random.Range(-spawnDistance, spawnDistance)
        );

        Debug.Log("Spawning enemy at position: " + spawnPosition);

        if (enemyPrefab != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            // No need to assign the baseTransform here; the enemy will find its own targets.
        }
        else
        {
            Debug.LogError("Enemy prefab is not assigned.");
        }
    }
}
