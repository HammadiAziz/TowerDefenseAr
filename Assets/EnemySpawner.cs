using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;    // Assign your enemy prefab here
    public float spawnInterval = 2.0f; // Time between spawns
    public float spawnRadius = 10.0f; // Distance from the tower to spawn enemies

    private float timer;
    private Transform tower;

    void Start()
    {
        // Find the tower by tag
        GameObject towerObject = GameObject.FindWithTag("Tour");
        if (towerObject != null)
        {
            tower = towerObject.transform;
        }
        else
        {
            Debug.LogError("No GameObject with the 'Tower' tag found!");
        }
    }

    void Update()
    {
        // Spawn enemies at intervals
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (tower != null)
        {
            // Generate a random position within a circle around the tower
            Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(
                tower.position.x + randomPosition.x,
                tower.position.y,
                tower.position.z + randomPosition.y
            );

            // Instantiate the enemy at the random spawn position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
