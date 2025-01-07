using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TowerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public TextMeshPro healthTextMesh;
    public Image healthBarImage;
    public GameObject healthBarBackground;
    private float targetHealthBarFillAmount;
    private float transitionSpeed = 5f;

    public GameObject damageNumberPrefab; // Reference to the damage number prefab
    private List<GameObject> activeDamageNumbers = new List<GameObject>(); // List to track active damage numbers

    public Transform cameraTransform;
    public bool isGlowing = false; // Indicates if the tower is glowing

    public void ActivateGlow(float duration)
    {
        isGlowing = true;
        // Optional: Trigger visual effects for the glow here
        StartCoroutine(DeactivateGlowAfterTime(duration));
    }

    private IEnumerator DeactivateGlowAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        isGlowing = false;
        // Optional: Disable the visual effects here
    }

    private void Start()
    {
        currentHealth = maxHealth;
        targetHealthBarFillAmount = currentHealth / maxHealth;

        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = targetHealthBarFillAmount;
            healthBarImage.gameObject.SetActive(false); // Hide the health bar initially
        }

        if (healthBarBackground != null)
        {
            healthBarBackground.SetActive(false); // Hide the background initially
        }

        if (healthTextMesh != null)
        {
            healthTextMesh.text = "HP: " + Mathf.Round(currentHealth);
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, targetHealthBarFillAmount, Time.deltaTime * transitionSpeed);
        }

        RotateHealthBarToCamera();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        ShowDamageNumber(damage); // Show damage number when taking damage

        if (healthBarImage != null && !healthBarImage.gameObject.activeSelf)
        {
            healthBarImage.gameObject.SetActive(true); // Show the health bar when taking damage
        }

        if (healthBarBackground != null && !healthBarBackground.activeSelf)
        {
            healthBarBackground.SetActive(true); // Show the background when taking damage
        }

        targetHealthBarFillAmount = currentHealth / maxHealth;

        if (healthTextMesh != null)
        {
            healthTextMesh.text = "HP: " + Mathf.Round(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ShowDamageNumber(float damage)
    {
        if (damageNumberPrefab != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 0, 0); // Adjusted for 3D TextMeshPro

            GameObject damageText = Instantiate(damageNumberPrefab, spawnPosition, Quaternion.identity); // Instantiate at the tower's position
            activeDamageNumbers.Add(damageText); // Track the instantiated damage number

            TextMeshPro damageTextMesh = damageText.GetComponent<TextMeshPro>();
            if (damageTextMesh != null)
            {
                damageTextMesh.text = "-" + Mathf.Round(damage).ToString();
                damageTextMesh.fontSize = 24; // Optional: Adjust the size of the text

                Debug.Log($"Damage Number: {damageTextMesh.text}");
            }
            else
            {
                Debug.LogWarning("Damage TextMeshPro component not found!");
            }

            StartCoroutine(AnimateDamageNumber(damageText));
        }
        else
        {
            Debug.LogWarning("Damage Number Prefab is missing!");
        }
    }

    private IEnumerator AnimateDamageNumber(GameObject damageText)
    {
        float timeAlive = 0f;
        Vector3 startPosition = damageText.transform.position;

        // Animate upwards and fade out
        while (timeAlive < 1f)
        {
            if (damageText == null) yield break; // Exit if the object is destroyed

            timeAlive += Time.deltaTime;
            damageText.transform.position = startPosition + new Vector3(0, timeAlive * 0.5f, 0); // Move upwards

            yield return null;
        }

        if (damageText != null)
        {
            activeDamageNumbers.Remove(damageText); // Remove from the list once destroyed
            Destroy(damageText); // Remove the damage number after the animation
        }
    }



    private void Die()
    {
        Debug.Log($"{gameObject.name} has been destroyed.");

        TowerDestroyEffect destroyEffect = GetComponent<TowerDestroyEffect>();
        if (destroyEffect != null)
        {
            destroyEffect.TriggerDestroyEffect();
        }
        // Destroy all remaining damage numbers
        foreach (GameObject damageNumber in activeDamageNumbers)
        {
            Destroy(damageNumber);
        }
        activeDamageNumbers.Clear(); // Clear the list

        // Hide or destroy the health bar and background
        if (healthBarImage != null)
        {
            healthBarImage.gameObject.SetActive(false); // Hide the health bar image
        }

        if (healthBarBackground != null)
        {
            healthBarBackground.SetActive(false); // Hide the health bar background
        }

        
       
    }

    public bool IsDestroyed()
    {
        return currentHealth <= 0;
    }

    private void RotateHealthBarToCamera()
    {
        Vector3 directionToCamera = cameraTransform.position - transform.position;
        directionToCamera.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
