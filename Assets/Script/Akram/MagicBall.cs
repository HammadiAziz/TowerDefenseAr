using UnityEngine;

public class MagicBall : MonoBehaviour
{
    public float speed = 2f; // Speed of the magic ball
    private Transform target; // Target enemy
    private int damage; // Damage dealt by the ball

    public void Initialize(Transform targetTransform, int damageAmount)
    {
        target = targetTransform;
        damage = damageAmount;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Destroy if it reaches the target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            OnHitTarget();
        }
    }

    private void OnHitTarget()
    {
        // Apply damage to the enemy
        EnemyController enemy = target.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Destroy the magic ball
        Destroy(gameObject);
    }
}
