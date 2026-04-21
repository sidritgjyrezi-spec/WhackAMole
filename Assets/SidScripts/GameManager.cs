using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { Idle, Running, Ended }
    public GameState CurrentState { get; private set; } = GameState.Idle;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;

    [Header("Worms")]
    public List<Worm> worms;

    [Header("Audio")]
    public AudioSource bgMusic;
    public AudioSource hitSound;

    [Header("Round Settings")]
    public float roundDuration = 30f;

    private int score = 0;
    private int highScore = 0;
    private float timer;

    // -------------------------------------------------------

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();
        UpdateTimerUI(roundDuration);

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (CurrentState != GameState.Running) return;
        timer -= Time.deltaTime;
        UpdateTimerUI(timer);
        if (timer <= 0f) EndGame();
    }

    // -------------------------------------------------------

    public void StartGame()
    {
        if (CurrentState == GameState.Running) return;

        score = 0;
        timer = roundDuration;
        CurrentState = GameState.Running;

        UpdateScoreUI();
        UpdateTimerUI(timer);

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        foreach (Worm w in worms)
            w.StartMoving();

        if (bgMusic != null)
        {
            bgMusic.loop = true;
            bgMusic.Play();
        }
    }

    public void AddScore(int amount = 1)
    {
        if (CurrentState != GameState.Running) return;
        score += amount;
        UpdateScoreUI();

        if (hitSound != null)
            hitSound.Play();
    }

    public void ResetGame()
    {
        CurrentState = GameState.Idle;
        score = 0;
        UpdateScoreUI();
        UpdateTimerUI(roundDuration);

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        foreach (Worm w in worms)
            w.StopMoving();

        if (bgMusic != null)
            bgMusic.Stop();
    }

    public int GetScore() => score;
    public int GetHighScore() => highScore;

    // -------------------------------------------------------

    void EndGame()
    {
        CurrentState = GameState.Ended;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        foreach (Worm w in worms)
            w.StopMoving();

        if (bgMusic != null)
            bgMusic.Stop();

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER\nScore: " + score + "\nBest: " + highScore;
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score + "  |  Best: " + highScore;
    }

    void UpdateTimerUI(float time)
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(Mathf.Max(time, 0f));
    }
}