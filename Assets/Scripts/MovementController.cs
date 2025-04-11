using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementController : MonoBehaviour
{
    public XRController leftController;
    public XRController rightController;
    public bool canMove = false; 

    void Update()
    {
        if (!canMove)
        {
            leftController.enableInputActions = false;
            rightController.enableInputActions = false;
        }
        else
        {
            leftController.enableInputActions = true;
            rightController.enableInputActions = true;
        }
    }
}
