using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Basketball;

public class scoremanager : MonoBehaviour
{
    public static scoremanager Instance;
    public AudioClip[] swishSounds;
    public AudioSource audioSource;
    public AudioClip rimHitSound;
    public TextMeshProUGUI scoreText;
    public ParticleSystem scoreParticles;
    public RimDetector rimDetector;
    public int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        UpdateScoreText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Basketball"))
        {
            if (rimDetector != null && rimDetector.BallHitRimRecently())
                PlayRimHitSwish();
            else
                PlayRandomSwishSound();

            PlayScoreEffect();
            ShotMaker ballScript = other.GetComponent<ShotMaker>();

            if (ballScript != null)
            {
                ballScript.made = true;
                if (ballScript.shotType == "FreeThrow")
                {
                    score++;
                }
                else if (ballScript.shotType == "MidRange" || ballScript.shotType == "Layup" )
                {
                    score += 2;
                }
                else if (ballScript.shotType == "ThreePointer")
                    score += 3;
            }
            UpdateScoreText();
        }
    }
    private void PlayRandomSwishSound()
    {
        if (audioSource && swishSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, swishSounds.Length);
            audioSource.PlayOneShot(swishSounds[randomIndex]);
        }
    }

    private void PlayRimHitSwish()
    {
        if (audioSource && rimHitSound)
        {
            audioSource.PlayOneShot(rimHitSound);
        }
    }

    private void PlayScoreEffect()
    {
        if (scoreParticles != null)
        {
            scoreParticles.Play();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
