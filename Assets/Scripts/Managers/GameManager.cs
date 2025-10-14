using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private UI_InGame inGameUi;

    [Header("LevelManagement")]
    [SerializeField] private float gameTimer;
    [SerializeField] private int currentLevelIndex;
    private int nextLevelIndex;

   
    [Header("FruitManagment")]
    public bool randomFruit;
    public int fruitCollected;
    public int totallFruits;
    public Transform fruitParent;

    [Header("CheckpointsManager")]
    public bool checkPointCanReActivate;

    [Header("Managers")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private DifficultyManager difficultyManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private SkinManager skinManager;
    [SerializeField] private ObjectCreator objectCreator;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inGameUi = UI_InGame.instance;
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        nextLevelIndex = currentLevelIndex + 1;
        CollectFruitsInfo();
        CreateManagerIfNeeded();
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;
        inGameUi.SetGameTimer(gameTimer);
    }

    private void CreateManagerIfNeeded()
    {
        if (AudioManager.instance == null)
            Instantiate(audioManager);

        if (DifficultyManager.instance == null)
            Instantiate(difficultyManager);

        if (PlayerManager.instance == null)
            Instantiate(playerManager);

        if(SkinManager.instance == null)    
            Instantiate(skinManager);

        if (ObjectCreator.instance == null)
            Instantiate(objectCreator);
    }

    private void CollectFruitsInfo()
    {
        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totallFruits = allFruits.Length;
        inGameUi.SetCollectedFruits(fruitCollected, totallFruits);
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalFriuts", totallFruits);
    }

    [ContextMenu("Parent all the fruits")]
    private void ParentAllTheFruits()
    {
        if (fruitParent == null)
            return;

        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        foreach (Fruit fruit in allFruits)
        {
            fruit.transform.parent = fruitParent;
        }
    }

    public bool FruitsHaveRandomLook() => randomFruit;

    public void AddFruit()
    {
        fruitCollected++;
        inGameUi.SetCollectedFruits(fruitCollected, totallFruits);    
    }

    public void RediusFruit()
    {
        fruitCollected--;
        inGameUi.SetCollectedFruits(fruitCollected, totallFruits);
    }

    public void RestartLevel() => UI_InGame.instance.fadeEffect.ScreenFade(1, 1, LoadCurrentScene);

    private void LoadCurrentScene() => SceneManager.LoadScene("Level_" + currentLevelIndex);

    public int GetCollectedFruits() => fruitCollected;  

    public bool CheckPointsCanBeReactivate() => checkPointCanReActivate;

    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Level_" + nextLevelIndex);
    }

    private void LoadCreditScene() => SceneManager.LoadScene("TheCredit");

    public void LevelFinished()
    {
        SaveLevelProgression();
        SaveLevelBestTime();
        SaveLevelFruitCollected();
        LoadNextScene();
    }

    public void SaveLevelFruitCollected()
    {
        int prevFruitCollected = PlayerPrefs.GetInt("Level" + currentLevelIndex + "FruitCollected");

        if(prevFruitCollected < fruitCollected)
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "FruitCollected", fruitCollected);

        int FruitBank = PlayerPrefs.GetInt("FruitBank");
        PlayerPrefs.SetInt("FruitBank", FruitBank + fruitCollected);
    }

    public void SaveLevelBestTime()
    {
        float prevTimeRecord = PlayerPrefs.GetFloat("Level" + currentLevelIndex + "BestTimeRecord", 99);

        if(gameTimer < prevTimeRecord)
            PlayerPrefs.SetFloat("Level" + currentLevelIndex + "BestTimeRecord", gameTimer);
    }

    private void SaveLevelProgression()
    {
        PlayerPrefs.SetInt("Level" + nextLevelIndex + "Unlocked", 1);
        if (NoMoreLevel() == false)
            PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);
    }

    private void LoadNextScene()
    {
        UI_FadeEffect fadeEffect = UI_InGame.instance.fadeEffect;

        if (NoMoreLevel())
            fadeEffect.ScreenFade(1, 1.5f, LoadCreditScene);
        else
            fadeEffect.ScreenFade(1, 1.5f, LoadNextLevel);
    }

    private bool NoMoreLevel()
    {
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 2;
        bool noMoreLevel = currentLevelIndex == lastLevelIndex;
        return noMoreLevel;
    }
}
