using UnityEngine;

public class Finishpoint : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player1 player1 = collision.GetComponent<Player1>();

        if (player1 != null)
        {
            AudioManager.instance.PlaySFX(2);
            anim.SetTrigger("Activate");
            GameManager.instance.LevelFinished();
        }
    }
}
