using System.Collections;
using UnityEngine;

public class Fruit_DropedByPlayer : Fruit
{

    [SerializeField] private Vector2 velocity;
    [SerializeField] private Color transparentColor;
    [SerializeField] private float[] waitTime;
    private bool canPeakup;


    protected override void Start()
    {
        base.Start();

        StartCoroutine(BlinkCoroutine());
    }
    private void Update()
    {
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;
    }
    private IEnumerator BlinkCoroutine()
    {
        anim.speed = 0;

        foreach (float seconds in waitTime)
        {
            toggleSpeedAndColor(transparentColor);

            yield return new WaitForSeconds(seconds);

            toggleSpeedAndColor(Color.white);

            yield return new WaitForSeconds(seconds);
        }
        velocity.x = 0;
        anim.speed = 1;
        canPeakup = true;
    }

    private void toggleSpeedAndColor(Color color)
    {
        velocity.x = velocity.x * -1;
        sr.color = color;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(canPeakup == false)
            return;

        base.OnTriggerEnter2D(collision);
    }

}
