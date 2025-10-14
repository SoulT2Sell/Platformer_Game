using UnityEngine;

public class DamageTerigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player1 player1 = collision.GetComponent<Player1>();

        if (player1 != null )
        {
            player1.Knockback(transform.position.x);
            player1.Damage();
        }     
    }
}
