using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyyy : MonoBehaviour
{
    [SerializeField] int maxHealth = 100; // Enemy's max health
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a bullet
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(25); // Damage value (e.g., 25)
            Destroy(collision.gameObject); // Destroy the bullet after collision
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage; // Subtract health
        Debug.Log("Enemy Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died!");
        Destroy(gameObject); // Destroy the enemy GameObject
    }
}
