using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameModeMenu : MonoBehaviour
{
    [Header("Game Mode Buttons")]
    public Button freePlayButton;
    public Button timeAttackButton;
    public Button challengeButton;

    [Header("Teleportation Targets")]
    public Transform freePlaySpawn;
    public Transform timeAttackSpawn;
    public Transform challengeSpawn;

    [Header("Player Reference")]
    public Transform player;

    [Header("Debug")]
    public TextMeshProUGUI debugText;

    private void Start()
    {
        freePlayButton.onClick.AddListener(() => TeleportPlayer(freePlaySpawn));
        timeAttackButton.onClick.AddListener(() => TeleportPlayer(timeAttackSpawn));
        challengeButton.onClick.AddListener(() => TeleportPlayer(challengeSpawn));
    }

    public void TeleportPlayer(Transform spawnPoint)
    {
        if (player != null && spawnPoint != null)
        {
            player.position = spawnPoint.position;
            player.rotation = spawnPoint.rotation;
            Debug.Log($"Teleported to {spawnPoint.name}");
            debugText.text = $"Teleported to {spawnPoint.name}";
        }
    }
}
