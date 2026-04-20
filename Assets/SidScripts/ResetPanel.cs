using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class ResetPanel : MonoBehaviour
{
    public GameObject resetPanel;

    void Update()
    {
        // Secondary button on right controller toggles the panel
        bool pressed;
        var device = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(
            UnityEngine.XR.XRNode.RightHand);

        if (device.TryGetFeatureValue(
                UnityEngine.XR.CommonUsages.secondaryButton, out pressed) && pressed)
        {
            TogglePanel();
        }
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