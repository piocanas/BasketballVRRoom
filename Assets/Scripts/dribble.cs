using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRDribble : MonoBehaviour
{
    public Transform controllerTransform; // Assign XR Controller's transform here
    public float dribbleForce = 5f;
    public float handSpeedThreshold = 0.3f;
    public float maxDistance = 0.5f;
    public float grabCooldownTime = 0.5f;

    private Rigidbody rb;
    private Vector3 lastHandPosition;
    private bool isGrabbed = false;
    private float lastGrabTime = -1f;

    public AudioSource bounceAudioSource;
    public AudioClip[] bounceSounds;
    private float minImpactThreshold = 0.5f;
    private float maxImpactThreshold = 5.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (controllerTransform != null)
        {
            lastHandPosition = controllerTransform.position;
        }
    }

    void FixedUpdate()
    {
        if (controllerTransform == null || isGrabbed) return;

        if (Time.time - lastGrabTime < grabCooldownTime) return;

        Vector3 handPosition = controllerTransform.position;

        Vector3 handVelocity = (handPosition - lastHandPosition) / Time.fixedDeltaTime;
        lastHandPosition = handPosition;

        bool isHandMovingDown = handVelocity.y < -handSpeedThreshold;
        bool isHandNearBall = Vector3.Distance(handPosition, transform.position) <= maxDistance;

        if (isHandMovingDown && isHandNearBall)
        {
            rb.AddForce(Vector3.down * dribbleForce, ForceMode.Impulse);
        }
    }

    void IgnoreHandCollision(bool ignore)
    {
        Collider ballCollider = GetComponent<Collider>();
        Collider[] handColliders = GameObject.FindGameObjectsWithTag("Hand")
            .SelectMany(hand => hand.GetComponentsInChildren<Collider>())
            .ToArray();

        foreach (Collider handCollider in handColliders)
        {
            Physics.IgnoreCollision(ballCollider, handCollider, ignore);
        }
    }


    public void OnGrab()
    {
        isGrabbed = true;
        IgnoreHandCollision(true);
    }

    public void OnRelease()
    {
        isGrabbed = false;
        lastGrabTime = Time.time;
        IgnoreHandCollision(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore rim collisions (since they already play sounds)
        if (collision.gameObject.CompareTag("Rim")) return;
        //if (collision.gameObject.CompareTag("Backboard")) return;

        // Get the strongest impact point
        float impactStrength = 0f;
        foreach (ContactPoint contact in collision.contacts)
        {
            impactStrength = Mathf.Max(impactStrength, collision.relativeVelocity.magnitude);
        }

        if (impactStrength > minImpactThreshold && bounceSounds.Length > 0)
        {
            float volume = Mathf.Clamp01(impactStrength / maxImpactThreshold); 
            int randomIndex = Random.Range(0, bounceSounds.Length);
            
            if (!bounceAudioSource.isPlaying)
            {
                bounceAudioSource.PlayOneShot(bounceSounds[randomIndex], volume);
            }
        }
    }
}

