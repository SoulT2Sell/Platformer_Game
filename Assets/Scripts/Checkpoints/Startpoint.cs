using UnityEngine;

public class Startpoint : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player1 player1 = collision.GetComponent<Player1>();

        if (player1 != null)
            anim.SetTrigger("Activate");
    }
}
