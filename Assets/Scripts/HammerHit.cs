using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HammerHit : MonoBehaviour
{
    [Header("Haptics")]
    public float hapticAmplitude = 0.5f;
    public float hapticDuration = 0.1f;

    [Header("Swing Validation")]
    public float minSwingSpeed = 0.5f;

    private ActionBasedController xrController;
    private Rigidbody rb;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody>();
        xrController = GetComponentInParent<ActionBasedController>();

        if (rb == null)
        {
            Debug.LogWarning("HammerHit: No Rigidbody found. Adding one automatically.");
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }

    public bool IsValidSwing()
    {
        return rb != null && rb.linearVelocity.magnitude >= minSwingSpeed;
    }

    public void TriggerHaptics()
    {
        if (xrController == null)
        {
            Debug.LogWarning("HammerHit: No XR Controller found for haptics.");
            return;
        }

        xrController.SendHapticImpulse(hapticAmplitude, hapticDuration);
    }
}