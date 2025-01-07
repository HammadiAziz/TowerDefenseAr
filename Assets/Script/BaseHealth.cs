using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public float health = 100f; // Total health of the object

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject); // Destroy object when health reaches 0
        }
    }

    public bool IsDestroyed()
    {
        return health <= 0;
    }
}
