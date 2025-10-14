using Unity.Cinemachine;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{

    private Transform player;
    private CinemachineCamera cinemachin;

    private void Awake()
    {
        cinemachin = GetComponentInChildren<CinemachineCamera>(true);
    }
    private void Start()
    {
        InvokeRepeating(nameof(PlayerRef), 0, 1);
    }
    private void Update()
    {
        GetPlayerTransform();
    }

    private void GetPlayerTransform()
    {
        cinemachin.Follow = player;
    }

    private void PlayerRef()
    {
        if (PlayerManager.instance.player == null)
            return;

        if (player == null)
            player = PlayerManager.instance.player.transform;
    }

    public void ActiveCamera(bool status)
    {
        cinemachin.gameObject.SetActive(status);
    }
}
