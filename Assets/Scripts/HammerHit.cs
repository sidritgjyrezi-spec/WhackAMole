using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Assuming PICO uses XR Interaction Toolkit

public class HammerHit : MonoBehaviour
{
    // This script is optional if hit detection is in Worm.cs, but provided as per request.
    // It can handle additional hammer-specific logic, like vibration on hit.

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Worm") && gameManager.currentState == GameManager.GameState.Running)
        {
            // Hit logic is already in Worm.cs, but this can add effects like controller haptics.
            // For PICO, use PXR_Input.SendHapticImpulse() if setup.
            Debug.Log("Hammer hit a worm!");
        }
    }
}