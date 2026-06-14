using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarSystemManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string levelMapSceneName = "LevelMap";

    [Header("Win UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private GameObject[] earnedStarObjects;

    private void Start()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        UpdateStarObjects(earnedStarObjects, 0);
    }

    public void CompleteCurrentLevel()
    {
        int levelNumber = DifficultyManager.SelectedLevelNumber;
        Difficulty difficulty = DifficultyManager.SelectedDifficulty;

        LevelProgressManager progressManager = LevelProgressManager.GetInstance();
        progressManager.CompleteLevel(levelNumber, difficulty);

        int earnedStars = DifficultyManager.GetStarsForDifficulty(difficulty);

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (resultText != null)
        {
            resultText.text = "Level " + levelNumber + " Complete\n" +
                              "Difficulty: " + DifficultyManager.GetDifficultyName(difficulty) + "\n" +
                              "Stars: " + earnedStars;
        }

        UpdateStarObjects(earnedStarObjects, earnedStars);
    }

    public void ReturnToLevelMap()
    {
        SceneManager.LoadScene(levelMapSceneName);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateStarObjects(GameObject[] stars, int activeStars)
    {
        if (stars == null)
        {
            return;
        }

        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] != null)
            {
                stars[i].SetActive(i < activeStars);
            }
        }
    }
}
