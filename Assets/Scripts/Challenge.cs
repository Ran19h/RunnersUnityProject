using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    [Header("Goal")]
    public int targetCount = 10;

    [Header("Refs")]
    public PlayerInventory playerInventory;
    public GameTimer gameTimer;

    [Header("UI")]
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Behaviour")]
    public bool freezeOnEnd = true;

    bool ended;

    void Start()
    {
        playerInventory.OnDiamondCollected.AddListener(OnDiamondCollected);
        gameTimer.OnTimerEnded += OnTimerEnded;
    }

    void OnDestroy()
    {
        playerInventory.OnDiamondCollected.RemoveListener(OnDiamondCollected);
        gameTimer.OnTimerEnded -= OnTimerEnded;
    }

    void OnDiamondCollected(PlayerInventory inv)
    {
        if (ended) return;

        if (inv.NumberOfDiamonds >= targetCount)
            EndGame(true);
    }

    void OnTimerEnded()
    {
        if (ended) return;

        bool win = playerInventory.NumberOfDiamonds >= targetCount;
        EndGame(win);
    }

    void EndGame(bool win)
    {
        ended = true;
        if (winPanel) winPanel.SetActive(win);
        if (losePanel) losePanel.SetActive(!win);
        if (freezeOnEnd) Time.timeScale = 0f;
    }
}