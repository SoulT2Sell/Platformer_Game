using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame instance;
    public UI_FadeEffect fadeEffect { get; private set; }

    [SerializeField] private TextMeshProUGUI gameTimer;
    [SerializeField] private TextMeshProUGUI fruitCollected;
    [SerializeField] private GameObject PauseUI;

    private bool isPaused;

    private void Awake()
    {
        instance = this;
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();   
    }
    private void Start()
    {
        fadeEffect.ScreenFade(0, 1f);
    }

    public void SetGameTimer(float time)
    {
        gameTimer.text = time.ToString("00") + "s";
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            PauseButton();
    }

    public void PauseButton()
    {
        if(isPaused == true)
        {
            isPaused = false;
            Time.timeScale = 1;
            PauseUI.SetActive(false);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
            PauseUI.SetActive(true);    
        }
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1; 
        fadeEffect.ScreenFade(1, 1.5f);
        SceneManager.LoadScene(0);
    }

    public void SetCollectedFruits(int collected, int totalFruits)
    {
        fruitCollected.text = collected.ToString() + " / " + totalFruits.ToString();
    }
}
