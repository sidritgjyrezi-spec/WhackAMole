using UnityEngine;
using TMPro;

public class HighscoreDisplay : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI highscoreText;
    public GameObject highscorePanel;

    void Start()
    {
        if (highscorePanel != null)
            highscorePanel.SetActive(false);
    }

    public void ToggleHighscorePanel()
    {
        if (highscorePanel == null) return;
        bool isActive = highscorePanel.activeSelf;
        highscorePanel.SetActive(!isActive);

        if (!isActive)
            UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (highscoreText != null)
            highscoreText.text = "Best Score: " + GameManager.Instance.GetHighScore()
                                                + "\nCurrent: "  + GameManager.Instance.GetScore();
    }
}