using UnityEngine;

public class Trap_Trampoline : MonoBehaviour
{
    protected Animator anim;
    [SerializeField] private float pushPower;
    [SerializeField] private float duration = .5f;
    private void Awake()
    {
        anim = GetComponent<Animator>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player1 player1 = collision.GetComponent<Player1>();

        if (player1 != null)
        {
            player1.Push(transform.up * pushPower, duration);
            anim.SetTrigger("Activate");
        }
    }
}
