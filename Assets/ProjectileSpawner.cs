using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public Transform firePoint; 
    public float detectionRadius = 10f; 
    public float fireRate = 1f; 
    public string enemyTag = "Enemy"; 

    private float nextFireTime;

    void Update()
    {
        DetectAndAttack();
    }

    void DetectAndAttack()
    {
        
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider obj in objectsInRange)
        {
            
            if (obj.CompareTag(enemyTag) && Time.time >= nextFireTime)
            {
                FireProjectile(obj.transform);
                nextFireTime = Time.time + fireRate;
                break; 
            }
        }
    }

    void FireProjectile(Transform target)
    {
        
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        
        ProjectileBehavior behavior = projectile.GetComponent<ProjectileBehavior>();
        if (behavior != null)
        {
            behavior.SetTarget(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
