using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements;
    [SerializeField] private GameObject continueButton;
    private AudioManager audioManager;
    private UI_FadeEffect fadeEffect;
    public string sceneName;

    [Header("Interactive Camera")]
    [SerializeField] private MenuCharecter menuCharecter;
    [SerializeField] private CinemachineCamera cinemachine;
    [SerializeField] private Transform mainmenuPoint;
    [SerializeField] private Transform skinSelectPoint;
    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }
    private void Start()
    {
        if(HasLevelProgression())
            continueButton.SetActive(true);

        fadeEffect.ScreenFade(0, 1.5f);

        audioManager = AudioManager.instance;
    }
    public void NewGame()
    {
        fadeEffect.ScreenFade(1, 1.5f, LoadScene);

        audioManager.PlaySFX(4);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene( sceneName );
    }

    public void SwitchUI(GameObject uiToEnable)
    {
        foreach(GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        uiToEnable.SetActive(true);

        audioManager.PlaySFX(4);
    }

    private bool HasLevelProgression()
    {
        bool hasLevelProgression = PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;

        return hasLevelProgression;
    }

    public void ContinueGame()
    {
        int difficultyIndex = PlayerPrefs.GetInt("GameDifficulty");
        DifficultyManager.instance.SetDifficulty((DifficultyType)difficultyIndex);
        int ContinueIndex = PlayerPrefs.GetInt("ContinueLevelNumber", 0);
        SceneManager.LoadScene("Level_" +  ContinueIndex);

        audioManager.PlaySFX(4);
    }

    public void MoveCameraToMainMenu()
    {
        menuCharecter.MoveTo(mainmenuPoint);
        cinemachine.Follow = mainmenuPoint;
    }

    public void MoveCameraToSkinSelect()
    {
        menuCharecter.MoveTo(skinSelectPoint);
        cinemachine.Follow = skinSelectPoint;   
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
