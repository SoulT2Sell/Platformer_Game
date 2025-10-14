using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;
    public int choosenSkinID;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if(instance == null)    
            instance = this;
        else 
            Destroy(gameObject);

    }

    public void setSkinID(int id) => choosenSkinID = id;
    public int getSkinID() => choosenSkinID;    
}
