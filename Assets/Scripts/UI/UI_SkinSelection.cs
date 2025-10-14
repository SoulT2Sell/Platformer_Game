using TMPro;
using UnityEngine;



[System.Serializable]
public struct Skin
{
    public string skinName;
    public int price;
    public bool unlocked;
}
public class UI_SkinSelection : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField] private Skin[] skinList;

    [SerializeField] private int skinIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private Animator skinDisplay;

    [SerializeField] private TextMeshProUGUI buySelectText;
    [SerializeField] private TextMeshProUGUI priceTextMesh;
    [SerializeField] private TextMeshProUGUI fruitBank;

    private UI_MainMenu mainMenuUI;
    private UI_LevelSelection levelSelectionUI;
    private void Start()
    {
        LoadSkinUnlocks();
        UpdateSkinDisplay();
        mainMenuUI = GetComponentInParent<UI_MainMenu>();
        levelSelectionUI = mainMenuUI.GetComponentInChildren<UI_LevelSelection>(true);
        audioManager = AudioManager.instance;
    }
    private void LoadSkinUnlocks()
    {
        for (int i = 0; i < skinList.Length; i++)
        {
            string skinName = skinList[i].skinName;
            bool skinUnlocked = PlayerPrefs.GetInt(skinName + "Unlocked", 0) == 1;

            if(skinUnlocked || i == 0)
                skinList[i].unlocked = true;
        }
    }
    public void SelectSkin()
    {
        if (skinList[skinIndex].unlocked == false)
            BuySkin(skinIndex);
        else
        {
            SkinManager.instance.setSkinID(skinIndex);
            mainMenuUI.SwitchUI(levelSelectionUI.gameObject);
        }

        UpdateSkinDisplay();

        audioManager.PlaySFX(4);
    }
    public void NextSkin()
    {
        skinIndex++;

        if(skinIndex > maxIndex)
            skinIndex = 0;  
        
        UpdateSkinDisplay();

        audioManager.PlaySFX(4);
    }

    public void PrevSkin()
    {
        skinIndex--; 

        if(skinIndex < 0)
            skinIndex = maxIndex;

        UpdateSkinDisplay();

        audioManager.PlaySFX(4);
    }

    private void UpdateSkinDisplay()
    {
        fruitBank.text ="Bank:" + FruitInBank();

        for (int i = 0; i < skinDisplay.layerCount; i++)
        {
            skinDisplay.SetLayerWeight(i, 0);
        }

        skinDisplay.SetLayerWeight(skinIndex, 1);

        if (skinList[skinIndex].unlocked)
        {
            priceTextMesh.transform.parent.gameObject.SetActive(false);
            buySelectText.text = "Select";
        }
        else
        {
            priceTextMesh.transform.parent.gameObject.SetActive(true);
            priceTextMesh.text = "Price:" + skinList[skinIndex].price.ToString();
            buySelectText.text = "Buy";
        }
    }

    private void BuySkin(int index)
    {
        if (HaveEnoughFruit(skinList[index].price) == false)
        {
            AudioManager.instance.PlaySFX(6);
            Debug.Log("Not Enough Fruit");
            return;
        }

        AudioManager.instance.PlaySFX(10);
        string skinName = skinList[index].skinName; 
        skinList[index].unlocked = true;
        PlayerPrefs.SetInt(skinName + "Unlocked", 1);
    }
    private int FruitInBank() => PlayerPrefs.GetInt("FruitBank");

    private bool HaveEnoughFruit(int price)
    {
        if(FruitInBank() >= price)
        {
            PlayerPrefs.SetInt("FruitBank", FruitInBank() - price);
            return true;
        }
        else
            return false;
    }
}
