using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackboardPhysics : MonoBehaviour
{
    public AudioSource audioSource;  
    public AudioClip[] backboardHitSounds;  

    [Header("Physics Settings")]
    public float englishSpinStrength = 0.5f;  
    public float bounceDampening = 0.8f;  
    public float minImpactThreshold = 0.5f; 

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Basketball")) return;

        Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
        if (ballRb == null) return;

        float impactStrength = collision.relativeVelocity.magnitude;

        if (impactStrength > minImpactThreshold && backboardHitSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, backboardHitSounds.Length);
            float volume = Mathf.Clamp01(impactStrength / 5.0f);
            audioSource.PlayOneShot(backboardHitSounds[randomIndex], volume);
        }

        ApplyEnglishSpin(ballRb, collision);
    }

    private void ApplyEnglishSpin(Rigidbody ballRb, Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        Vector3 reflectedVelocity = Vector3.Reflect(ballRb.velocity, normal) * bounceDampening;

        bool hitFront = Vector3.Dot(normal, Vector3.forward) > 0;

        if (hitFront)
        {
            Vector3 spinDirection = Vector3.Cross(Vector3.up, normal) * englishSpinStrength;
            ballRb.angularVelocity += spinDirection;
        }

        ballRb.velocity = reflectedVelocity;
    }
}
