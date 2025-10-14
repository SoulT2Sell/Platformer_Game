using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelSelection : MonoBehaviour
{
    [SerializeField] private UI_LevelSelectButton levelSelectButttonPrefab;
    [SerializeField] private Transform buttonParent;

    [SerializeField] private bool[] levelsUnlocked;   

    private void Start()
    {
        LoadLevelsInfo();
        MakeButton();
    }
    private void MakeButton()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings - 1;    

        for (int i = 1; i < sceneCount; i++)
        {
            if (IsLevelUnlocked(i) == false)
                return;
            UI_LevelSelectButton newButton = Instantiate(levelSelectButttonPrefab, buttonParent);
            newButton.SetLevelName(i);
        }
    }

    private bool IsLevelUnlocked(int levelIndex) => levelsUnlocked[levelIndex]; 

    private void LoadLevelsInfo()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings - 1;
        levelsUnlocked = new bool[sceneCount];

        for (int i = 1; i < sceneCount; i++)
        {
            bool levelUnlocked = PlayerPrefs.GetInt("Level" + i + "Unlocked", 0) == 1;

            if(levelUnlocked)   
                levelsUnlocked[i] = true;
        }

        levelsUnlocked[1] = true;
    }
}
