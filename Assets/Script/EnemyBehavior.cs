using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float attackRange = 1.5f;
    public float attackDamage = 10f;
    public float attackInterval = 1f;
    public float attackDelay = 0.5f; // Delay before damage is applied

    private Transform currentTarget;
    private float nextAttackTime = 0f;

    private Animator animator;
    private static bool isGameOver = false; // Shared flag for all enemies

    private void Start()
    {
        animator = GetComponent<Animator>();
        UpdateTarget("SmallTower");
    }


    private void Update()
    {
        if (isGameOver)
        {
            // Trigger the Celebrate animation once
            if (animator != null && !animator.GetBool("isCelebrating"))
            {
                animator.SetBool("isCelebrating", true);
            }
            return; // Stop enemy behavior if the game is over
        }

        if (currentTarget == null)
        {
            UpdateTarget("SmallTower");
            if (currentTarget == null)
            {
                UpdateTarget("MainBase");
            }
        }

        if (currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            TowerHealth targetTower = currentTarget.GetComponent<TowerHealth>();
            if (targetTower != null && !targetTower.isGlowing)
            {

                if (distanceToTarget <= attackRange)
                {
                    AttackTarget();
                }
                else
                {
                    if (animator != null)
                    {
                        animator.SetBool("isAttacking", false);
                    }
                    MoveTowardsTarget();
                    RotateTowardsTarget();
                }
            }
        }
    }

    void UpdateTarget(string tag)
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(tag);

        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        bool foundValidTarget = false;

        foreach (GameObject target in potentialTargets)
        {
            TowerHealth towerHealth = target.GetComponent<TowerHealth>();

            // Skip glowing towers
            if (towerHealth != null && towerHealth.isGlowing)
                continue;

            // If it's a valid target, mark it
            foundValidTarget = true;

            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        // If no valid target is found, consider all targets as valid again (i.e., glowing towers)
        if (!foundValidTarget)
        {
            foreach (GameObject target in potentialTargets)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }

        if (closestTarget != null)
        {
            currentTarget = closestTarget.transform;
        }
    }

    void MoveTowardsTarget()
    {
        // Only move towards target if not in attack range
        if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) > attackRange)
        {
            Vector3 targetPosition = new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void RotateTowardsTarget()
    {
        // Always rotate towards the target, even if it's in range, to align for attack
        Vector3 directionToTarget = currentTarget.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void AttackTarget()
    {
        if (currentTarget != null)
        {
            TowerHealth towerHealth = currentTarget.GetComponent<TowerHealth>();

            // Skip attack if the tower is glowing
            if (towerHealth != null && towerHealth.isGlowing)
            {
                UpdateTarget("SmallTower");
                if (currentTarget == null)
                {
                    UpdateTarget("MainBase");
                }
                return;  // Skip the attack
            }

            if (Time.time >= nextAttackTime)
            {
                if (animator != null)
                {
                    animator.SetBool("isAttacking", true); // Start attack animation
                }

                nextAttackTime = Time.time + attackInterval; // Set the next attack time
            }
        }
    }

    // This method is triggered by the animation event
    public void DealDamage()
    {
        if (currentTarget != null)
        {
            // Check if the current target is a tower or base and apply damage accordingly
            TowerHealth towerHealth = currentTarget.GetComponent<TowerHealth>();
            if (towerHealth != null)
            {
                towerHealth.TakeDamage(attackDamage);

                // If the tower is destroyed, update the target
                if (towerHealth.IsDestroyed())
                {
                    UpdateTarget("SmallTower");
                    if (currentTarget == null)
                    {
                        UpdateTarget("MainBase");
                    }
                }
            }
            else
            {
                BaseHealth baseHealth = currentTarget.GetComponent<BaseHealth>();
                if (baseHealth != null)
                {
                    baseHealth.TakeDamage(attackDamage);

                    // Check if the base is destroyed and end the game
                    if (baseHealth.IsDestroyed())
                    {
                        isGameOver = true; // Set the global flag to freeze all enemies
                        FreezeAllEnemies(); // Call method to freeze all enemies
                    }
                }
            }
        }
    }

    // Method to freeze all enemies
    private void FreezeAllEnemies()
    {
        EnemyBehavior[] allEnemies = FindObjectsOfType<EnemyBehavior>();
        foreach (var enemy in allEnemies)
        {
            if (enemy.animator != null)
            {
                enemy.animator.enabled = false; // Disable the Animator for all enemies
            }
            enemy.enabled = false; // Disable the script to stop updates
        }
    }
}