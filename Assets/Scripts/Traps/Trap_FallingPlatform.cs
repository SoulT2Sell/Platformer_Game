using UnityEngine;

public class Trap_FallingPlatform : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D[] boxColliders;

    [Header("Flying Options")]
    [SerializeField] private bool canMove = false;
    [SerializeField] private float distance;
    [SerializeField] private float speed = .75f;
    private Vector3[] wayPoints;
    private int wayPointsIndex = 0;
    
    [Header("Fall Details")]
    [SerializeField] private float impactSpeed = 3;
    [SerializeField] private float impactDuration = .1f;
    private float impactTimer;
    private bool impactHappened;
    [Space]
    [SerializeField] private float fallDelay = .5f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxColliders = GetComponents<BoxCollider2D>(); 
    }

    private void Start()
    {
        SetupWayPoints();
        float randomDelay = Random.Range(0, .6f);
        Invoke(nameof(ActivatePlatform), randomDelay);
    }

    private void Update()
    {
        HandleImpact();
        HandleMovment();
    }

    private void ActivatePlatform() =>  canMove = true;
    
    private void SetupWayPoints()
    {
        wayPoints = new Vector3[2];
        float yOffset = distance/2;
        wayPoints[0] = transform.position + new Vector3(0, yOffset, 0);
        wayPoints[1] = transform.position + new Vector3(0, -yOffset, 0);
    }

    private void HandleMovment()
    {
        if(canMove == false) 
            return;
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[wayPointsIndex], speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, wayPoints[wayPointsIndex]) < .1f)
        {
            wayPointsIndex++;
            if(wayPointsIndex >= wayPoints.Length)  
                wayPointsIndex = 0; 
        }
    }

    private void HandleImpact()
    {
        if (impactTimer < 0)
            return;

        impactTimer -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3.down * 10), impactSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(impactHappened == true) 
            return;

        Player1 player1 = collision.GetComponent<Player1>();

        if (player1 != null)
        {
            Invoke(nameof(PlatformDeactivate), fallDelay);
            impactTimer = impactDuration;
            impactHappened = true;  
        }
    }

    private void PlatformDeactivate()
    {
        anim.SetTrigger("Activate");
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 3.5f;
        rb.linearDamping = .5f;

        foreach (BoxCollider2D boxCollider in boxColliders)
        {
            boxCollider.enabled = false;
        }

        canMove = false;
    }
}
