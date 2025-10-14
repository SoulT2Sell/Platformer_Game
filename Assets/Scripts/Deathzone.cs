using UnityEngine;

public class Deathzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player1 player1 = collision.GetComponent<Player1>();

        if(player1 != null )
        {
            player1.Damage();
            player1.Die();
            PlayerManager.instance.RespawnPlayer();
        }

        Enemy enemy = collision.GetComponent<Enemy>();

        if(enemy != null )
            enemy.Die();
    }
}
