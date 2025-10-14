using System.Collections;
using UnityEngine;

public class Trap_Fire : MonoBehaviour
{
    [SerializeField] private float offDuration;
    [SerializeField] private Trap_FireButton fireButton;
    private Animator anim;
    private bool isActive;
    private CapsuleCollider2D fireCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        fireCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        if (fireButton == null)
            Debug.LogWarning("You dont have fire button on" + gameObject.name + "!");

        SetFire(true);  
    }

    public void SwitchOffFire()
    {
        if(isActive == false)
            return;

        StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        SetFire(false);

        yield return new WaitForSeconds(offDuration);

        SetFire(true);
    }

    private void SetFire(bool active)
    {
        anim.SetBool("Activate", active);
        fireCollider.enabled = active;
        isActive = active;
    }
}