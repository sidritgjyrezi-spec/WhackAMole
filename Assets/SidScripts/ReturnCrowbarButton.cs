using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ReturnCrowbarButton : MonoBehaviour
{
    [Header("Assign These")]
    public XRGrabInteractable crowbar;
    public XRSocketInteractor socket;

    private float cooldown = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - cooldown < 1f) return;
        cooldown = Time.time;

        ReturnCrowbarToSocket();
    }

    public void ReturnCrowbarToSocket()
    {
        if (crowbar == null || socket == null) return;

        // Drop the crowbar if someone is holding it
        if (crowbar.isSelected)
        {
            crowbar.interactionManager.SelectExit(
                crowbar.firstInteractorSelecting,
                crowbar
            );
        }

        // Move crowbar to socket position and rotation
        crowbar.transform.position = socket.transform.position;
        crowbar.transform.rotation = socket.transform.rotation;

        // Zero out physics so it doesnt fly away
        Rigidbody rb = crowbar.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Crowbar returned to socket!");
    }
}