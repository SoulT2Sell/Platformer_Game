using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelSelectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private TextMeshProUGUI levelBestTimeRecord;
    [SerializeField] private TextMeshProUGUI levelFruitsCollected;
    private string sceneName;
    private int levelIndex;

    private DifficultyType difficulty;
    private int difficultyIndex;

    public void SetLevelName(int newlevelIndex)
    {
        levelIndex = newlevelIndex;
        levelNumberText.text = "Level_" + levelIndex;
        sceneName = "Level_" + levelIndex;

        levelBestTimeRecord.text = SetLevelBestTimeRecord();
        levelFruitsCollected.text = SetLevelFruitCollected();
    }
    public void LoadScene()
    {
        difficulty = DifficultyManager.instance.difficulty;
        difficultyIndex = ((int)difficulty);
        PlayerPrefs.SetInt("GameDifficulty", difficultyIndex);
        SceneManager.LoadScene(sceneName);
    }

    public string SetLevelBestTimeRecord()
    {
        float timeRecord = PlayerPrefs.GetFloat("Level" + levelIndex + "BestTimeRecord", 99f);
        return "Best Time: " + timeRecord.ToString("00") + " sec";
    }

    public string SetLevelFruitCollected()
    {
        int totalFruits = PlayerPrefs.GetInt("Level" + levelIndex + "TotalFriuts", 0);
        string totalFruitsText = totalFruits == 0 ? "?" : totalFruits.ToString();   
        int fruitCollected = PlayerPrefs.GetInt("Level" + levelIndex + "FruitCollected", 0);
        return "Fruits: " + fruitCollected.ToString() + "/" + totalFruitsText;
    }
}
