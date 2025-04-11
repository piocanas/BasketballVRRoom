using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using TMPro;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;

    [Header("Player & Spawns")]
    public Transform mainMenuSpawn;
    public Transform freeModeSpawn;
    public Transform timedModeSpawn;
    public Transform challengeSpawn;
    public Transform player;

    [Header("Challenge Mode")]
    public Transform[] challengeSpawns;
    private int challengeSpawnIndex = 0;
    private int shotsTakenAtCurrentSpot = 0;
    private const int shotsPerSpot = 5;

    [Header("UI")]
    public GameObject gameOverUI;
    public GameObject countdownUI;
    public GameObject TimerUI;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;
    public AudioClip countdownBeep;
    public AudioClip gameOverSound;
    public AudioSource audioSource;
    private float gameTimer = 0;

    [Header("Game Settings")]
    public GameObject shootingRack;
    public TextMeshProUGUI fieldGoalText;
    public TextMeshProUGUI debugText;
    public bool timerstop = false;

    public float challengeDuration = 60f;

    private int totalShots = 0;
    private int madeShots = 0;
    private bool isChallengeActive = false;

    public enum GameMode { None, FreeMode, TimedMode, ShootingChallenge }
    public GameMode currentMode = GameMode.None;
    public scoremanager scoremanager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartGameMode(currentMode);
    }

    public void StartGameMode(GameMode mode)
    {
        currentMode = mode;
        MovePlayer(GetSpawnPoint(mode));
        scoremanager.score = 0;

        switch (mode)
        {
            case GameMode.FreeMode:
                shootingRack.SetActive(false);
                RestrictPlayerMovement(false);
                isChallengeActive = false;
                break;

            case GameMode.TimedMode:
                shootingRack.SetActive(true);
                RestrictPlayerMovement(false);
                StartTimedMode();
                break;

            case GameMode.ShootingChallenge:
                shootingRack.SetActive(true);
                totalShots = 0;
                madeShots = 0;
                RestrictPlayerMovement(true);
                isChallengeActive = true;
                StartShootingChallenge();
                break;

            case GameMode.None:
                ReturnToMainMenu();
                break;
        }

        Debug.Log($"Started Game Mode: {mode}");
        UpdateDebugText($"Game Mode: {mode}");
    }

    public void gamemodeClick(string gamemode)
    {
        if (gamemode == "FreeMode")
        {
            StartGameMode(GameMode.FreeMode);
        }
        else if (gamemode == "TimedMode")
        {
            StartGameMode(GameMode.TimedMode);
        }
        else if (gamemode == "ShootingChallenge")
        {
            StartGameMode(GameMode.ShootingChallenge);
        }
    }

    public void ResetMode()
    {
        if (currentMode == GameMode.None) return;

        // Restart the same mode
        RestrictPlayerMovement(false);
        StartGameMode(currentMode);
    }

    public void ReturnToMainMenu()
    {
        gameOverUI.gameObject.SetActive(false);
        currentMode = GameMode.None;
        MovePlayer(mainMenuSpawn);
        shootingRack.SetActive(false);
        isChallengeActive = false;
        bool restrictmenu = true;
        RestrictPlayerMovement(restrictmenu);
        UpdateDebugText("Returned to Main Menu");
    }

    private Transform GetSpawnPoint(GameMode mode)
    {
        return mode switch
        {
            GameMode.FreeMode => freeModeSpawn,
            GameMode.TimedMode => timedModeSpawn,
            GameMode.ShootingChallenge => challengeSpawn,
            _ => mainMenuSpawn
        };
    }

    private void MovePlayer(Transform spawnPoint)
    {
        if (spawnPoint == null) return;

        player.position = spawnPoint.position;
        player.rotation = spawnPoint.rotation;
    }
    public void RegisterShot(bool made)
    {
        if (!isChallengeActive) return;

        totalShots++;
        if (made) madeShots++;
        shotsTakenAtCurrentSpot++;

        fieldGoalText.text = $"FG%: {GetFieldGoalPercentage()}%   {madeShots} / {totalShots}";

        if (shotsTakenAtCurrentSpot >= shotsPerSpot)
        {
            challengeSpawnIndex++;

            if (challengeSpawnIndex < challengeSpawns.Length)
            {
                MovePlayer(challengeSpawns[challengeSpawnIndex]);
                shotsTakenAtCurrentSpot = 0;
            }
            else
            {
                EndGame();
            }
        }
    }


    private int GetFieldGoalPercentage()
    {
        return totalShots > 0 ? Mathf.RoundToInt(madeShots / (float)totalShots * 100f) : 0;
    }

    public void RestrictPlayerMovement(bool restrict)
    {
        ActionBasedContinuousMoveProvider locomotion = player.GetComponent<ActionBasedContinuousMoveProvider>();
        if (locomotion != null)
        {
            locomotion.enabled = !restrict;
        }
    }

    private void UpdateDebugText(string message)
    {
        if (debugText != null)
        {
            debugText.text = message;
        }
    }

    private IEnumerator StartCountdown(System.Action onComplete)
    {
        countdownUI.SetActive(true);
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            audioSource.PlayOneShot(countdownBeep);
            yield return new WaitForSeconds(1f);
        }

        countdownUI.SetActive(false);
        countdownText.gameObject.SetActive(false);
        onComplete.Invoke();
    }

    public void StartShootingChallenge()
    {
        RestrictPlayerMovement(true);
        MovePlayer(challengeSpawns[0]);
        shootingRack.SetActive(true);

        challengeSpawnIndex = 0;
        shotsTakenAtCurrentSpot = 0;

        totalShots = 0;
        madeShots = 0;
        isChallengeActive = true;

        DisableBallSpawning();

        StartCoroutine(StartCountdown(() =>
        {
        }));
    }
    public void StartTimedMode()
    {
        MovePlayer(timedModeSpawn);
        shootingRack.SetActive(true);
        gameTimer = challengeDuration;

        StartCoroutine(StartCountdown(() =>
        {
            StartCoroutine(StartTimer());
        }));
    }
    private IEnumerator StartTimer()
    {
        float timeLeft = challengeDuration;
        timerText.gameObject.SetActive(true);
        TimerUI.SetActive(true);

        while (timeLeft > 0)
        {
            timerText.text = Mathf.Ceil(timeLeft).ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
            if (timerstop == true)
                break;
        }

        timerText.gameObject.SetActive(false);
        EndGame();
    }
    private void EndGame()
    {
        isChallengeActive = false;
        timerstop = false;

        gameOverUI.SetActive(true);
        countdownUI.SetActive(false);
        TimerUI.SetActive(false);

        audioSource.PlayOneShot(gameOverSound);
        EnableHandButtons();

        gameOverUI.SetActive(true);
        if (currentMode == GameMode.ShootingChallenge){
            gameOverText.text = "GAMEOVER                           " + $"FG%: {GetFieldGoalPercentage()}%   {madeShots} / {totalShots}";
        }
        else
            gameOverText.text = "GAMEOVER";
    }

    private void DisableBallSpawning()
    {
        XRController[] controllers = FindObjectsOfType<XRController>();

        foreach (var controller in controllers)
        {
            controller.enableInputActions = false;
        }
    }
    private void EnableHandButtons()
    {
        XRController[] controllers = FindObjectsOfType<XRController>();

        foreach (var controller in controllers)
        {
            controller.enableInputActions = true;
        }
    }

}
