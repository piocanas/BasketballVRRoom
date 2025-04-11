using UnityEngine;
using UnityEngine.XR;
using TMPro;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;

public class VRMenuController : MonoBehaviour
{
    private bool menuButtonPressed = false;
    public TextMeshProUGUI debugText;
    public GameObject background;

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        device.TryGetFeatureValue(CommonUsages.menuButton, out bool menuPressed);

        if (menuPressed && !menuButtonPressed)
        {
            menuButtonPressed = true;
            HandleMenuPress();
        }
        else if (!menuPressed)
        {
            menuButtonPressed = false;
        }
    }

    void HandleMenuPress()
    {
        if (GameModeManager.Instance == null) return;

        background.SetActive(true);

        if (GameModeManager.Instance.currentMode == GameModeManager.GameMode.None)
        {
            debugText.text = "Already in the main menu.";
        }
        else
        {
            GameModeManager.Instance.StartGameMode(GameModeManager.GameMode.None);
            GameModeManager.Instance.timerstop = true;
        }
    }
}
