using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class BallGrabHaptic : MonoBehaviour
{
    public XRBaseInteractor interactor;

    private void OnEnable()
    {
        interactor.selectEntered.AddListener(OnGrab);
    }

    private void OnDisable()
    {
        interactor.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRController controller)
        {
            controller.SendHapticImpulse(0.5f, 0.1f);
        }
    }
}
