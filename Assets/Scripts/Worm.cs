using UnityEngine;
using System.Collections;

public class Worm : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float upPosition = 1f;
    public float downPosition = 0f;
    public float moveSpeed = 2f;

    [Header("Timing Parameters")]
    public float popIntervalMin = 1f;
    public float popIntervalMax = 3f;
    public float popDuration = 2f;

    private Vector3 startPosition;
    private bool isMoving = false;
    private bool isUp = false;
    private bool isFullyUp = false;
    private bool wasHit = false;
    private float timer = 0f;
    private Coroutine moveCoroutine;

    private GameManager gameManager;

    void Start()
    {
        startPosition = transform.localPosition;
        gameManager = GameManager.Instance;
        ResetWorm();
    }

    void Update()
    {
        if (!isMoving || gameManager.currentState != GameManager.GameState.Running) return;

        timer -= Time.deltaTime;

        if (isUp && timer <= 0)
            StartMoveDown();
        else if (!isUp && timer <= 0)
            StartMoveUp();
    }

    public void StartMoving()
    {
        isMoving = true;
        timer = Random.Range(popIntervalMin, popIntervalMax);
    }

    public void StopMoving()
    {
        isMoving = false;
        StopAllCoroutines();
        moveCoroutine = null;
        ForceDown();
    }

    public void ResetWorm()
    {
        StopAllCoroutines();
        moveCoroutine = null;
        isMoving = false;
        isUp = false;
        isFullyUp = false;
        wasHit = false;
        timer = 0f;
        transform.localPosition = new Vector3(startPosition.x, downPosition, startPosition.z);
    }

    private void StartMoveUp()
    {
        isUp = true;
        isFullyUp = false;
        wasHit = false;
        timer = popDuration;

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(SmoothMove(upPosition, onComplete: () => isFullyUp = true));
    }

    private void StartMoveDown()
    {
        isUp = false;
        isFullyUp = false;
        timer = Random.Range(popIntervalMin, popIntervalMax);

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(SmoothMove(downPosition));
    }

    private void ForceDown()
    {
        isUp = false;
        isFullyUp = false;
        wasHit = false;
        transform.localPosition = new Vector3(startPosition.x, downPosition, startPosition.z);
    }

    private IEnumerator SmoothMove(float targetY, System.Action onComplete = null)
    {
        Vector3 start = transform.localPosition;
        Vector3 end = new Vector3(start.x, targetY, start.z);
        float elapsed = 0f;
        float duration = Mathf.Abs(targetY - start.y) / moveSpeed;

        if (duration <= 0f)
        {
            transform.localPosition = end;
            onComplete?.Invoke();
            yield break;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            t = t * t * (3f - 2f * t); // Smooth ease in-out
            transform.localPosition = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.localPosition = end;
        onComplete?.Invoke();
        moveCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.currentState != GameManager.GameState.Running) return;
        if (!other.CompareTag("Hammer")) return;
        if (!isFullyUp || wasHit) return;

        HammerHit hammer = other.GetComponent<HammerHit>();

        // If hammer script exists, validate swing speed before scoring
        if (hammer != null && !hammer.IsValidSwing())
        {
            Debug.Log("Worm: Hit ignored — swing too slow.");
            return;
        }

        wasHit = true;
        gameManager.Score++;
        gameManager.PlayHitSound();
        hammer?.TriggerHaptics();
        StartCoroutine(HitSquish());
        StartMoveDown();
    }

    private IEnumerator HitSquish()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 squishScale = new Vector3(originalScale.x * 1.3f, originalScale.y * 0.6f, originalScale.z * 1.3f);

        float elapsed = 0f;
        float squishTime = 0.08f;

        while (elapsed < squishTime)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, squishScale, elapsed / squishTime);
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < squishTime)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(squishScale, originalScale, elapsed / squishTime);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}