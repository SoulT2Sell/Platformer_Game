using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool Activate = false;
    private bool canBeReactivate;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        canBeReactivate = GameManager.instance.CheckPointsCanBeReactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Activate && canBeReactivate == false) 
            return;

        Player1 player1 = collision.GetComponent<Player1>();

        if (player1 != null)
            CheckpointActivation();
    }

    private void CheckpointActivation()
    {
        Activate = true;
        anim.SetTrigger("Activation");
        PlayerManager.instance.UpdateCheckpoin(transform);
    }

}
