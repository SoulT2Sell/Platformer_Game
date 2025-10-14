using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private string groundLayerName = "Ground";
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    [SerializeField] private float speed;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
        anim = GetComponent<Animator>(); 
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
    }

    public void Flip_Bullet() => sr.flipX = !sr.flipX;

    public void HandleMovement(Vector2 velocity) => rb.linearVelocity = velocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            collision.GetComponent<Player1>().Knockback(transform.position.x);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            Destroy(gameObject);
        }
    }

}
