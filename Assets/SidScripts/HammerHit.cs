using UnityEngine;

public class HammerHit : MonoBehaviour
{
    [Header("Settings")]
    public float hitCooldown = 0.3f;
    private float lastHitTime;

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastHitTime < hitCooldown) return;

        Worm worm = other.GetComponent<Worm>();
        if (worm != null)
        {
            worm.OnHit();
            lastHitTime = Time.time;
        }
    }
}