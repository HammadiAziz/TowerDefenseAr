using UnityEngine;
using System.Collections;

public class TornadoEffect : MonoBehaviour
{
    public GameObject tornado;          // Reference to the Tornado GameObject
    public float attractionForce = 5f;  // Speed at which the enemy is pulled toward the tornado
    public float duration = 3f;         // How long the attraction lasts

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is an enemy
        if (other.CompareTag("Enemy"))
        {
            // Activate the tornado
            tornado.SetActive(true);

            // Start pulling the enemy toward the tornado
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                StartCoroutine(AttractEnemy(enemy));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the trigger is an enemy
        if (other.CompareTag("Enemy"))
        {
            // Deactivate the tornado
            tornado.SetActive(false);
        }
    }

    private IEnumerator AttractEnemy(Enemy enemy)
    {
        enemy.StartAttraction(); // Pause enemy movement

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Check if the enemy still exists
            if (enemy == null) yield break;

            // Calculate the direction toward the tornado
            Vector3 direction = (tornado.transform.position - enemy.transform.position).normalized;

            // Move the enemy toward the tornado
            enemy.transform.position += direction * attractionForce * Time.deltaTime;

            // Wait for the next frame
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemy.EndAttraction(); // Resume enemy movement
    }

}
