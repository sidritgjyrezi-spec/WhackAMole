using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    public enum GameState { Idle, Running, Ended }

    // Game state management
    public GameState currentState = GameState.Idle;

    // Score and highscore
    private int score = 0;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreText.text = "Score: " + score;
            if (score > highscore)
            {
                highscore = score;
                PlayerPrefs.SetInt("Highscore", highscore);
            }
        }
    }
    private int highscore = 0;

    // UI elements
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;

    // Timer
    private float roundTime = 30f;
    private float remainingTime;

    // Worms
    public Worm[] worms; // Assign all Worm objects in Inspector

    // Sounds
    public AudioSource bgMusic;
    public AudioSource hitSound;

    void Start()
    {
        // Load highscore
        highscore = PlayerPrefs.GetInt("Highscore", 0);

        // Initialize game
        ResetGame();
        bgMusic.Play();
    }

    void Update()
    {
        if (currentState == GameState.Running)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(remainingTime).ToString();

            if (remainingTime <= 0)
            {
                EndGame();
            }
        }
    }

    public void StartGame()
    {
        if (currentState != GameState.Idle) return;

        currentState = GameState.Running;
        remainingTime = roundTime;
        Score = 0;

        foreach (Worm worm in worms)
        {
            worm.StartMoving();
        }
    }

    public void EndGame()
    {
        currentState = GameState.Ended;
        gameOverText.gameObject.SetActive(true);

        foreach (Worm worm in worms)
        {
            worm.StopMoving();
        }
    }

    public void ResetGame()
    {
        currentState = GameState.Idle;
        remainingTime = roundTime;
        Score = 0;
        gameOverText.gameObject.SetActive(false);

        foreach (Worm worm in worms)
        {
            worm.ResetWorm();
        }
    }

    public void PlayHitSound()
    {
        hitSound.Play();
    }
}