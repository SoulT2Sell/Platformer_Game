using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static event Action OnPlayerRespawn;
    public static PlayerManager instance;

    [Header("player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay;
    public Player1 player;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        if (respawnPoint == null)
            respawnPoint = FindFirstObjectByType<Startpoint>().transform.GetChild(0).transform;

        if (playerPrefab == null)
            playerPrefab = FindFirstObjectByType<Player1>().gameObject;

        if (player == null)
            player = FindFirstObjectByType<Player1>();
    }

    public void UpdateCheckpoin(Transform checkpoint) => respawnPoint = checkpoint;

    public void RespawnPlayer()
    {
        DifficultyManager difficultyManager = DifficultyManager.instance;
        if (difficultyManager != null && difficultyManager.difficulty == DifficultyType.Hard)
            return;
        StartCoroutine(Respawncoroutine());
    }
    private IEnumerator Respawncoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player1>();
        OnPlayerRespawn?.Invoke();
    }

}
