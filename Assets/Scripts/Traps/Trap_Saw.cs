using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Saw : MonoBehaviour
{
    private Animator anim;

    [Header("Options")]
    [SerializeField] private float coolDown = 1;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private Vector3[] waypointPosition;

    private int moveDirection = 1;
    private int waypointIndex = 1;
    private bool canMove = true;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();    
    }

    private void Start()
    {
        UpdateWayPoint();
        transform.position = wayPoints[0].position;
    }

    private void UpdateWayPoint()
    {
        List<Trap_SawWayPoint> wayPointsList = new List<Trap_SawWayPoint>(GetComponentsInChildren<Trap_SawWayPoint>());
        if(wayPointsList.Count > wayPoints.Length)
        {
            wayPoints = new Transform[wayPointsList.Count];
            for(int i = 0; i < wayPointsList.Count; i++)
            {
                wayPoints[i] = wayPointsList[i].transform;
            }
        }
        waypointPosition = new Vector3[wayPoints.Length];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            waypointPosition[i] = wayPoints[i].position;
        }
    }

    private void Update()
    {
        Moving();
    }

    private void Moving()
    {
        anim.SetBool("Active", canMove);

        if (canMove == false)
            return;

        transform.position = Vector2.MoveTowards(transform.position, waypointPosition[waypointIndex], moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypointPosition[waypointIndex]) < .1f)
        {
            if (waypointIndex == waypointPosition.Length - 1 || waypointIndex == 0)
            {
                moveDirection = moveDirection * -1;
                StartCoroutine(IdleSaw(coolDown));
            }
            waypointIndex = waypointIndex + moveDirection;
        }
    }
    private IEnumerator IdleSaw(float delay)
    {
        canMove = false;

        yield return new WaitForSeconds(delay);

        canMove = true;
        sr.flipX = !sr.flipX;   
    }
}
