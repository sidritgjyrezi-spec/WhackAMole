using System.Collections;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public enum State { Idle, MovingUp, FullyUp, MovingDown }

    public State currentState = State.Idle;

    public Vector3 downPosition = new Vector3(0, -0.5f, 0);
    public Vector3 upPosition = Vector3.zero;
    public float minDelay = 1f;
    public float maxDelay = 5f;
    public float upTime = 2f;
    public float speed = 2f;

    private bool isActive = false;
    private bool isHit = false;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        transform.localPosition = downPosition;
    }

    public void StartMoving()
    {
        if (isActive) return;
        isActive = true;
        StartCoroutine(PopUpRoutine());
    }

    public void StopMoving()
    {
        isActive = false;
        StopAllCoroutines();
        transform.localPosition = downPosition;
        currentState = State.Idle;
    }

    public void ResetWorm()
    {
        StopMoving();
        isHit = false;
    }

    private IEnumerator PopUpRoutine()
    {
        while (isActive)
        {
            currentState = State.Idle;
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            // Move up
            currentState = State.MovingUp;
            float t = 0;
            Vector3 start = transform.localPosition;
            while (t < 1 && isActive)
            {
                t += Time.deltaTime * speed;
                transform.localPosition = Vector3.Lerp(start, upPosition, t);
                yield return null;
            }
            if (!isActive) yield break;
            transform.localPosition = upPosition;
            currentState = State.FullyUp;

            yield return new WaitForSeconds(upTime);

            // Move down
            currentState = State.MovingDown;
            t = 0;
            start = transform.localPosition;
            while (t < 1 && isActive)
            {
                t += Time.deltaTime * speed;
                transform.localPosition = Vector3.Lerp(start, downPosition, t);
                yield return null;
            }
            if (!isActive) yield break;
            transform.localPosition = downPosition;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hammer") && currentState == State.FullyUp && !isHit)
        {
            isHit = true;
            gameManager.Score++;
            gameManager.PlayHitSound();
            // Immediately go down
            StopAllCoroutines();
            StartCoroutine(MoveDown());
        }
    }

    private IEnumerator MoveDown()
    {
        currentState = State.MovingDown;
        float t = 0;
        Vector3 start = transform.localPosition;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(start, downPosition, t);
            yield return null;
        }
        transform.localPosition = downPosition;
        isHit = false;
        if (isActive)
        {
            StartCoroutine(PopUpRoutine());
        }
    }
}
