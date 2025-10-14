using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Player1 Player1;

    private void Awake()
    {
        Player1 = GetComponentInParent<Player1>();  
    }

    private void FinishRespawn() => Player1.RespawnFinished(true);
}
