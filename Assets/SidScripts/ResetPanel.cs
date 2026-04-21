using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPanel : MonoBehaviour
{
    public GameObject resetPanel;

    private bool buttonWasPressed = false;

    void Update()
    {
        var device = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(
            UnityEngine.XR.XRNode.RightHand);

        bool pressed;
        device.TryGetFeatureValue(
            UnityEngine.XR.CommonUsages.secondaryButton, out pressed);

        // Only toggle on the FIRST frame the button is pressed
        if (pressed && !buttonWasPressed)
        {
            TogglePanel();
        }

        buttonWasPressed = pressed;
    }

    public void TogglePanel()
    {
        if (resetPanel == null) return;
        resetPanel.SetActive(!resetPanel.activeSelf);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}