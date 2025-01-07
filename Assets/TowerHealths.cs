using UnityEngine;

public class TowerHealths : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DestroyTower();
        }
    }

    public bool IsDestroyed()
    {
        return currentHealth <= 0;
    }

    private void DestroyTower()
    {
        // Play destruction animation if available
        if (animator != null)
        {
            animator.SetTrigger("Destroy");
        }

        // Optionally, disable components before destruction
        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        // Destroy the object after a delay to allow animation to play
        Destroy(gameObject, 2f);
    }
}
