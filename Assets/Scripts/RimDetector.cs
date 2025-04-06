using System.Collections;
using UnityEngine;

public class RimDetector : MonoBehaviour
{
    public AudioSource rimAudioSource;       // Assign in Inspector
    public AudioClip rimHitSound;            // Single hit sound
    public AudioClip[] rimRollSounds;        // Multiple rolling sounds

    private float lastHitTime = 0f;
    private float hitCooldown = 0.2f;        // Prevents spam on fast hits
    private float resetTime = 0.5f;

    private bool isRolling = false;          // Tracks if ball is rolling
    private Coroutine rollCoroutine;
    private bool ballHitRimRecently = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Basketball"))
        {
            float timeSinceLastHit = Time.time - lastHitTime;
            float impactStrength = collision.relativeVelocity.magnitude;

            if (impactStrength > 2f && timeSinceLastHit > hitCooldown) // Strong hit
            {
                PlayRimHitSound();
                lastHitTime = Time.time;
                ballHitRimRecently = true;
                StartCoroutine(ResetRimHitStatus());
            }
            else if (impactStrength <= 2f) // Soft roll
            {
                if (!isRolling)
                {
                    isRolling = true;
                    rollCoroutine = StartCoroutine(PlayRimRollSounds());
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Basketball"))
        {
            if (isRolling)
            {
                isRolling = false;
                if (rollCoroutine != null) StopCoroutine(rollCoroutine);
            }
        }
    }

    private void PlayRimHitSound()
    {
        if (rimHitSound != null)
        {
            rimAudioSource.PlayOneShot(rimHitSound);
        }
    }

    private IEnumerator PlayRimRollSounds()
    {
        while (isRolling)
        {
            if (rimRollSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, rimRollSounds.Length);
                rimAudioSource.PlayOneShot(rimRollSounds[randomIndex], 0.5f); // Lower volume for rolls
            }
            yield return new WaitForSeconds(Random.Range(0.3f, 0.7f)); // Randomize roll sound spacing
        }
    }
    private IEnumerator ResetRimHitStatus()
    {
        yield return new WaitForSeconds(resetTime);
        ballHitRimRecently = false;
    }

    public bool BallHitRimRecently()
    {
        return ballHitRimRecently;
    }
}
