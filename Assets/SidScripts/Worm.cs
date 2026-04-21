using UnityEngine;
using System.Collections;

public class Worm : MonoBehaviour
{
    [Header("Movement")]
    public float upPosition = 0.6f;
    public float downPosition = -0.3f;
    public float moveSpeed = 2f;
    public float waitTimeUp = 1.2f;
    public float waitTimeDown = 0.8f;

    private bool isUp = false;
    private bool isHit = false;
    private Vector3 startLocalPos;

    // -------------------------------------------------------

    void Start()
    {
        startLocalPos = transform.localPosition;
        SetToDown(instant: true);
    }

    // -------------------------------------------------------

    public void StartMoving()
    {
        isHit = false;
        StopAllCoroutines();
        StartCoroutine(WormCycle());
    }

    public void StopMoving()
    {
        StopAllCoroutines();
        StartCoroutine(MoveTo(startLocalPos + Vector3.up * downPosition));
        isUp = false;
        isHit = false;
    }

    public void OnHit()
    {
        if (isHit) return;
        if (!isUp) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.Running) return;

        isHit = true;
        GameManager.Instance.AddScore(1);

        StopAllCoroutines();
        StartCoroutine(HitReaction());
    }

    // -------------------------------------------------------

    IEnumerator WormCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(waitTimeDown, waitTimeDown + 1f));

            if (GameManager.Instance.CurrentState != GameManager.GameState.Running)
                yield break;

            yield return StartCoroutine(MoveTo(startLocalPos + Vector3.up * upPosition));
            isUp = true;

            yield return new WaitForSeconds(waitTimeUp);

            yield return StartCoroutine(MoveTo(startLocalPos + Vector3.up * downPosition));
            isUp = false;
            isHit = false;
        }
    }

    IEnumerator HitReaction()
    {
        yield return StartCoroutine(MoveTo(startLocalPos + Vector3.up * downPosition));
        isUp = false;
        isHit = false;

        if (GameManager.Instance.CurrentState == GameManager.GameState.Running)
            StartCoroutine(WormCycle());
    }

    IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.localPosition, target) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        transform.localPosition = target;
    }

    void SetToDown(bool instant)
    {
        Vector3 target = startLocalPos + Vector3.up * downPosition;
        if (instant)
            transform.localPosition = target;
        else
            StartCoroutine(MoveTo(target));
    }
}