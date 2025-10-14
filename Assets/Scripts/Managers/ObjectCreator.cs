using System.Collections;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    public static ObjectCreator instance;

    [Header("Arrow Trap")]
    public GameObject arrowPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void CreatObject(GameObject obj, Transform transform, float delay = 0, bool shouldBeDestroyed = false) => StartCoroutine(HandleCreatObject(obj, transform, delay, shouldBeDestroyed));

    private IEnumerator HandleCreatObject(GameObject obj, Transform transform, float delay, bool shouldBeDestroyed)
    {
        Vector3 position = transform.position;
        yield return new WaitForSeconds(delay);
        GameObject newObj = Instantiate(obj, position, Quaternion.identity);

        if(shouldBeDestroyed) 
            Destroy(newObj, 15);
    }
}
