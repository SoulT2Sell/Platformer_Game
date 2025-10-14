using UnityEngine;

public class Ui_DifficultySelect : MonoBehaviour
{
    private DifficultyManager difficultyManager;

    private void Start()
    {
        difficultyManager = DifficultyManager.instance;
    }

    public void SetEasy () => difficultyManager.SetDifficulty(DifficultyType.Easy);
    public void SetMedium () => difficultyManager.SetDifficulty(DifficultyType.Medium);
    public void SetHard() => difficultyManager.SetDifficulty(DifficultyType.Hard);
}
