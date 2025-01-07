using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Assign this in the Inspector
    public GameObject defeatScreen;  // Assign this in the Inspector
    public string mainBaseTag = "MainBase"; // The tag for the main base prefab

    private float survivalTime = 0f;
    private bool isCounting = false;

    private void Start()
    {
        // Ensure the defeat screen is hidden initially
        if (defeatScreen != null)
            defeatScreen.SetActive(false);

        // Start monitoring for the main base's presence immediately
        StartCoroutine(CheckForMainBase());
    }

    private void Update()
    {
        if (isCounting)
        {
            survivalTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(survivalTime / 60);
            int seconds = Mathf.FloorToInt(survivalTime % 60);
            timerText.text = $"Survival Time: {minutes:00}:{seconds:00}";
        }
    }

    public void StartTimer()
    {
        isCounting = true;
    }

    public void StopTimerAndShowDefeatScreen()
    {
        isCounting = false;

        if (defeatScreen != null)
        {
            defeatScreen.SetActive(true);
        }
    }

    private System.Collections.IEnumerator CheckForMainBase()
    {
        while (true)
        {
            // Find the main base by its tag
            GameObject mainBase = GameObject.FindWithTag(mainBaseTag);

            if (mainBase != null && !isCounting) // When main base is detected and timer isn't counting
            {
                StartTimer(); // Start the timer automatically
            }

            // If no main base is detected, stop the timer and show the defeat screen
            if (mainBase == null && isCounting)
            {
                StopTimerAndShowDefeatScreen();
                break;
            }

            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds
        }
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
