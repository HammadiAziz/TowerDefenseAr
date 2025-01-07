using UnityEngine;

public class TowerDestroyEffect : MonoBehaviour
{
    public float glowDuration = 1.5f; // Duration of the glowing effect
    public float fadeDuration = 1.5f; // Duration of the fade-out effect
    public Color glowColor = Color.magenta; // Glowing pink color

    private Material towerMaterial; // The material of the tower
    private Color originalColor;
    private bool isDestroying = false;

    private void Start()
    {
        // Get the material of the tower
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            towerMaterial = renderer.material;
            originalColor = towerMaterial.color;
        }
        else
        {
            Debug.LogWarning("MeshRenderer not found on the object!");
        }
    }

    public void TriggerDestroyEffect()
    {
        if (isDestroying || towerMaterial == null)
            return;

        isDestroying = true;
        StartCoroutine(DestroyAnimation());
    }

    private System.Collections.IEnumerator DestroyAnimation()
    {
        // Step 1: Glow Effect
        float timer = 0f;
        while (timer < glowDuration)
        {
            timer += Time.deltaTime;
            float glowFactor = Mathf.PingPong(timer * 3, 1); // Makes the glow pulse
            towerMaterial.color = Color.Lerp(originalColor, glowColor, glowFactor);
            towerMaterial.SetColor("_EmissionColor", glowColor * glowFactor * 10); // Glow multiplier
            yield return null;
        }

        // Step 2: Fade-Out Effect
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float fadeFactor = 1 - (timer / fadeDuration); // Gradually decreases
            towerMaterial.color = Color.Lerp(glowColor, Color.clear, 1 - fadeFactor); // Fade color
            towerMaterial.SetColor("_EmissionColor", glowColor * fadeFactor); // Reduce glow
            yield return null;
        }

        // Step 3: Destroy the object
        Destroy(gameObject);
    }
}
