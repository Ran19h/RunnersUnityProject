using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 90f; // 1 min 30 sec
    private float currentTime;
    private bool timerRunning = true;

    [Header("UI")]
    public TextMeshProUGUI timerText; // Drag TMP text here

    [Header("Alarm Settings")]
    public AudioSource alarmSound; // Drag alarm sound
    public Camera mainCamera;      // Main Camera
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.2f;

    [Header("Flash Settings")]
    public Image redFlashImage; // UI Image covering screen
    public float flashSpeed = 2f; // How fast it flashes

    private Vector3 originalCamPos;
    private bool alarmTriggered = false;

    // <-- This is the event ChallengeManager listens to
    public System.Action OnTimerEnded;

    void Start()
    {
        currentTime = startTime;
        if (mainCamera != null)
            originalCamPos = mainCamera.transform.localPosition;

        if (redFlashImage != null)
            redFlashImage.color = new Color(1, 0, 0, 0); // transparent red
    }

    void Update()
    {
        if (!timerRunning) return;

        // Update time
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            timerRunning = false;
            EndTimer();
        }

        // Display time
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";

        // Alarm and effects
        if (currentTime <= 30 && !alarmTriggered)
        {
            alarmTriggered = true;
            if (alarmSound) alarmSound.Play();
            InvokeRepeating(nameof(ShakeCamera), 0f, 0.1f);
        }

        // Flash red when under 30 seconds
        if (currentTime <= 30 && redFlashImage != null)
        {
            float alpha = Mathf.PingPong(Time.time * flashSpeed, 0.5f); // 0 to 0.5
            redFlashImage.color = new Color(1, 0, 0, alpha);
        }
    }

    void EndTimer()
    {
        CancelInvoke(nameof(ShakeCamera));
        if (mainCamera != null)
            mainCamera.transform.localPosition = originalCamPos;

        if (redFlashImage != null)
            redFlashImage.color = new Color(1, 0, 0, 0); // hide flash

        Debug.Log("Time's up!");

        // 🔔 Notify listeners (e.g., ChallengeManager) that time ended
        OnTimerEnded?.Invoke();

        // Game Over logic stays external (ChallengeManager)
    }

    void ShakeCamera()
    {
        if (mainCamera == null) return;

        Vector3 shakePos = originalCamPos + Random.insideUnitSphere * shakeIntensity;
        mainCamera.transform.localPosition = shakePos;

        CancelInvoke(nameof(ResetCamera));
        Invoke(nameof(ResetCamera), shakeDuration);
    }

    void ResetCamera()
    {
        if (mainCamera == null) return;
        mainCamera.transform.localPosition = originalCamPos;
    }
}
