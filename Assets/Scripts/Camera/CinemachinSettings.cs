using UnityEngine;

public class CinemachinSettings : MonoBehaviour
{
    private LevelCamera LevelCamera;

    private void Awake()
    {
        LevelCamera = GetComponentInParent<LevelCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player1 player = collision.GetComponent<Player1>();

        if(player != null )
        {
            LevelCamera.ActiveCamera(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player1 Player1 = collision.GetComponent<Player1>();

        if (Player1 != null)
        {
            LevelCamera.ActiveCamera(false);
        }
    }
}
