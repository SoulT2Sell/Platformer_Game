using UnityEngine;

public class Trap_arrow : Trap_Trampoline
{
    [Header("Additional Info")]
    [SerializeField] private float coolDown;
    [SerializeField] private float rotationSpeed = 120;
    [SerializeField] private bool rotateRight;
    private int rotateDir = -1;
    [Space]
    [SerializeField] private float scaleUpSpeed = 10;
    [SerializeField] private Vector3 targetScale;

    private void Start()
    {
        transform.localScale = new Vector3(.3f, .3f, .3f);
    }
    private void Update()
    {
        HandleScaleUp();
        HandleRotation();
    }

    private void HandleScaleUp()
    {
        if (transform.localScale.x < targetScale.x)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleUpSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        rotateDir = rotateRight ? -1 : 1;
        transform.Rotate(0, 0, (rotationSpeed * rotateDir) * Time.deltaTime);
    }

    private void DestroyMe()
    {
        GameObject arrowPrefab = ObjectCreator.instance.arrowPrefab;
        ObjectCreator.instance.CreatObject(arrowPrefab, transform, coolDown);
        Destroy(gameObject);
    }
}
