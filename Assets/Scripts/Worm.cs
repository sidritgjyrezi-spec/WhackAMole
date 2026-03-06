using UnityEngine;

public class Worm : MonoBehaviour
{
    // Movement parameters
    public float upPosition = 1f; // Height when popped up
    public float downPosition = 0f; // Height when hidden
    public float moveSpeed = 2f; // Speed of up/down movement
    public float popIntervalMin = 1f; // Min time between pops
    public float popIntervalMax = 3f; // Max time between pops
    public float popDuration = 2f; // How long it stays up

    private Vector3 startPosition;
    private bool isMoving = false;
    private bool isUp = false;
    private float timer = 0f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        startPosition = transform.localPosition;
        ResetWorm();
    }

    void Update()
    {
        if (!isMoving || gameManager.currentState != GameManager.GameState.Running) return;

        timer -= Time.deltaTime;

        if (isUp && timer <= 0)
        {
            MoveDown();
        }
        else if (!isUp && timer <= 0)
        {
            MoveUp();
        }
    }

    public void StartMoving()
    {
        isMoving = true;
        timer = Random.Range(popIntervalMin, popIntervalMax);
    }

    public void StopMoving()
    {
        isMoving = false;
        MoveDown();
    }

    public void ResetWorm()
    {
        isMoving = false;
        isUp = false;
        transform.localPosition = new Vector3(startPosition.x, downPosition, startPosition.z);
    }

    private void MoveUp()
    {
        isUp = true;
        timer = popDuration;
        // Use lerp in a coroutine for smooth movement if needed, but for simplicity, instant pop
        transform.localPosition = new Vector3(startPosition.x, upPosition, startPosition.z);
    }

    private void MoveDown()
    {
        isUp = false;
        timer = Random.Range(popIntervalMin, popIntervalMax);
        transform.localPosition = new Vector3(startPosition.x, downPosition, startPosition.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameManager.currentState != GameManager.GameState.Running) return;

        if (collision.gameObject.CompareTag("Hammer") && isUp)
        {
            gameManager.Score++;
            gameManager.PlayHitSound();
            MoveDown();
        }
    }
}