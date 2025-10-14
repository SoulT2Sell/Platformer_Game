using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TheCredit : MonoBehaviour
{
    private UI_FadeEffect fadeEffect;

    [SerializeField] private RectTransform rectT;
    [SerializeField] private float scrollSpeed = 200;
    [SerializeField] private float offScreenPosition = 1800;    

    [SerializeField] private string mainMenuName = "MainMenu";

    private bool creditSkipped;

    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }
    private void Start()
    {
        fadeEffect.ScreenFade(0, 2);
    }
    private void Update()
    {
        rectT.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        if(rectT.anchoredPosition.y > offScreenPosition)
            GoToMainMenu();
    }

    public void SkippCredit()
    {
        if(creditSkipped == false)
        {
            scrollSpeed *= 10;
            creditSkipped = true;
        }
        else
        {
            GoToMainMenu();
        }
    }

    private void GoToMainMenu() => fadeEffect.ScreenFade(1, 2, GoToMainMenuScene);
    private void GoToMainMenuScene()
    {
        SceneManager.LoadScene(mainMenuName);
    }
}
