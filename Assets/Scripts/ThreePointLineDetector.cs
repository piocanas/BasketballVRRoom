using UnityEngine;

public class ThreePointLineDetector : MonoBehaviour
{
    public bool isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    public bool IsMidRanger()
    {
        return isPlayerInside;
    }
}
