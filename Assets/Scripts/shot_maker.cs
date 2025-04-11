using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

namespace Basketball
{
    public class ShotMaker : MonoBehaviour
    {
        public BallTrajectoryDrawer trajectoryDrawer;
        
        [Header("Hoop Settings")]
        public Transform hoopTransform;  // Assign the hoop's GameObject (backboard, rim, etc.)
        public Vector3 hoopOffset = new Vector3(0, 0, 0);  // Adjust to set the exact rim center
        public float lastRimHitTime = -10f;

        [Header("Player Settings")]
        public Transform playerTransform;
        public ThreePointLineDetector triple;
        public Transform handTransform;
        public GameModeManager manager;
        public bool made = false;

        [Header("Assist Distances")]
        public float layupDistance = 2.5f;
        public float layupAssistRadius = 1.0f;
        public float midRangeAssistRadius = 1.5f;
        public float threePointAssistRadius = 2.0f;

        [Header("Gravity Settings")]
        public float layupGravity = 2.0f;
        public float midRangeGravity = 1.5f;
        public float threePointGravity = 1.0f;

        [Header("Assist Strength")]
        public float layupAssistStrength = 0.3f;
        public float midRangeAssistStrength = 0.5f;
        public float threePointAssistStrength = 0.7f;

        [Header("Shot Conditions")]
        public float activationSpeed = 2.0f; // Minimum speed to activate assistance
        public float upwardThreshold = 0.6f;

        [Header("Debugging & Visualization")]
        public TextMeshProUGUI debugText;
        public bool showHoopCenter = false;


        public AudioSource audioSource;
        public AudioClip rimHitSound;
        private Rigidbody rb;
        public bool isThrown = false;
        public string shotType = "None";
        public float assistStartFactor = 0.6f;
        private Vector3 startingPosition;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (hoopTransform == null || playerTransform == null) return;

            // Calculate the actual rim position using the offset
            Vector3 hoopCenter = hoopTransform.position + hoopOffset;
            float ballHeight = transform.position.y;
            float hoopHeight = hoopCenter.y;

            // If ball is thrown, apply assist if near the hoop
            if (isThrown)
            {
                float distanceToHoop = Vector3.Distance(transform.position, hoopCenter);
                float assistRadius = GetAssistRadius(shotType);
                // Reset thrown state if ball is below the hoop
                if (ballHeight < hoopHeight - 1f && rb.velocity.y < 0)
                {
                    isThrown = false;
                    UpdateDebugText("Ball Reset");
                    manager.RegisterShot(made);
                    made = false;
                    shotType = "none";
                }

                // Apply assist only if the ball is within assist radius and above the rim
                if (distanceToHoop <= assistRadius && ballHeight >= hoopHeight)
                {
                    ApplyShotAssist(shotType, hoopCenter);
                }
            }

        }

        private float GetAssistRadius(string shotType)
        {
            return shotType switch
            {
                "Layup" => layupAssistRadius,
                "MidRange" => midRangeAssistRadius,
                "ThreePointer" => threePointAssistRadius,
                _ => 1.5f
            };
        }

        private string DetermineShotType()
        {
            Vector3 horizontalPosition = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 hoopCenterHorizontal = new Vector3(hoopTransform.position.x, 0, hoopTransform.position.z);
            float horizontalDistance = Vector3.Distance(horizontalPosition, hoopCenterHorizontal);

            if (horizontalDistance <= layupDistance) return "Layup";
            if (triple.IsMidRanger()) return "MidRange";
            return "ThreePointer";
        }

        private void ApplyShotAssist(string shotType, Vector3 hoopCenter)
        {
            float assistStrength = 0f;

            switch (shotType)
            {
                case "Layup":
                    assistStrength = layupAssistStrength;
                    break;
                case "MidRange":
                    assistStrength = midRangeAssistStrength;
                    break;
                case "ThreePointer":
                    assistStrength = threePointAssistStrength;
                    break;
            }

            Vector3 directionToHoop = (hoopCenter - transform.position).normalized;
            float ballTravelDistance = Vector3.Distance(startingPosition, transform.position);
            float totalDistance = Vector3.Distance(startingPosition, hoopCenter);

            float assistFactor = Mathf.Clamp01(ballTravelDistance / (totalDistance * assistStartFactor));
            float scaledAssistStrength = assistStrength * assistFactor;

            Vector3 newVelocity = rb.velocity;
            newVelocity.x = Mathf.Lerp(newVelocity.x, directionToHoop.x * newVelocity.magnitude, scaledAssistStrength * Time.fixedDeltaTime);
            newVelocity.z = Mathf.Lerp(newVelocity.z, directionToHoop.z * newVelocity.magnitude, scaledAssistStrength * Time.fixedDeltaTime);

            rb.velocity = newVelocity;
        }

        public void OnRelease()
        {
            if (isThrown) return; // Prevent duplicate recording

            isThrown = true;
            shotType = DetermineShotType();
            startingPosition = transform.position;

            switch (shotType)
            {
                case "Layup":
                    rb.mass = layupGravity;
                    break;
                case "MidRange":
                    rb.mass = midRangeGravity;
                    break;
                case "ThreePointer":
                    rb.mass = threePointGravity;
                    break;
            }

            UpdateDebugText($"Shot Type: {shotType}");

            if (trajectoryDrawer != null)
            {
                trajectoryDrawer.ShowTrajectory(transform.position, rb.velocity);
            }
        }

        private void UpdateDebugText(string message)
        {
            if (debugText != null)
            {
                debugText.text = message;
            }
        }

        void OnDrawGizmos()
        {
            Vector3 hoopCenter = hoopTransform.position + hoopOffset;
            if (showHoopCenter && hoopTransform != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(hoopCenter, 0.2f);
            }
        }
    }
}
