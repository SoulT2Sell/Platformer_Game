using UnityEngine;

public enum FruitType {Apple,Banana,Cherry,Kiwi,Melon,Orange,Pineapple,Strawberry};

public class Fruit : MonoBehaviour
{
    [SerializeField] private FruitType fruitType;
    [SerializeField] private GameObject PickupVFX;

    private GameManager gameManager;
    protected Animator anim;
    protected SpriteRenderer sr;
    

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        gameManager = GameManager.instance;
        SetRandomLookIfNeeded();
    }

    private void SetRandomLookIfNeeded()
    {
        if (gameManager.FruitsHaveRandomLook() == false)
        {
            UpdateFruitVisuals();
            return;
        }

        int randomIndex = Random.Range(0, 8);
        anim.SetFloat("fruitIndex", randomIndex);
    }

    private void UpdateFruitVisuals() => anim.SetFloat("fruitIndex", (int)fruitType);

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Player1 player1 = collision.GetComponent<Player1>();

        if (player1 != null)
        {
            AudioManager.instance.PlaySFX(8);
            gameManager.AddFruit();
            Destroy(gameObject);

            GameObject newFX = Instantiate(PickupVFX, transform.position, Quaternion.identity);  
        } 
    }
}
