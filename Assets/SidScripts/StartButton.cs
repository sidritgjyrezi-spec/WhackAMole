using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StartButton : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (interactable != null)
            interactable.selectEntered.AddListener(OnPressed);
    }

    void OnPressed(SelectEnterEventArgs args)
    {
        GameManager.Instance.StartGame();
    }
}