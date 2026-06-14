using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class LevelButtonUI
{
    public Button button;
    public GameObject lockedIcon;
    public GameObject[] starObjects;
    public TextMeshProUGUI levelNumberText;
}

public class LevelMapManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "Game";

    [Header("Level Buttons")]
    [SerializeField] private LevelButtonUI[] levelButtons;

    [Header("Difficulty Panel")]
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private TextMeshProUGUI difficultyTitleText;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button extremeButton;

    private LevelProgressManager progressManager;
    private int selectedLevelNumber = 1;

    private void Awake()
    {
        SetupLevelButtonClicks();
        SetupDifficultyButtonClicks();
    }

    private void Start()
    {
        progressManager = LevelProgressManager.GetInstance();

        CloseDifficultyPanel();
        RefreshMap();
    }

    private void OnEnable()
    {
        if (progressManager != null)
        {
            progressManager.OnProgressChanged += RefreshMap;
        }
    }

    private void OnDisable()
    {
        if (progressManager != null)
        {
            progressManager.OnProgressChanged -= RefreshMap;
        }
    }

    private void SetupLevelButtonClicks()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNumber = i + 1;

            if (levelButtons[i].button == null)
            {
                continue;
            }

            levelButtons[i].button.onClick.RemoveAllListeners();
            levelButtons[i].button.onClick.AddListener(() => OpenDifficultyPanel(levelNumber));
        }
    }

    private void SetupDifficultyButtonClicks()
    {
        if (normalButton != null)
        {
            normalButton.onClick.RemoveAllListeners();
            normalButton.onClick.AddListener(() => StartLevel(Difficulty.Normal));
        }

        if (hardButton != null)
        {
            hardButton.onClick.RemoveAllListeners();
            hardButton.onClick.AddListener(() => StartLevel(Difficulty.Hard));
        }

        if (extremeButton != null)
        {
            extremeButton.onClick.RemoveAllListeners();
            extremeButton.onClick.AddListener(() => StartLevel(Difficulty.Extreme));
        }
    }

    public void RefreshMap()
    {
        if (progressManager == null)
        {
            progressManager = LevelProgressManager.GetInstance();
        }

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNumber = i + 1;
            LevelButtonUI levelUI = levelButtons[i];

            bool unlocked = progressManager.IsLevelUnlocked(levelNumber);
            int bestStars = progressManager.GetBestStars(levelNumber);

            if (levelUI.button != null)
            {
                levelUI.button.interactable = unlocked;
            }

            if (levelUI.lockedIcon != null)
            {
                levelUI.lockedIcon.SetActive(!unlocked);
            }

            if (levelUI.levelNumberText != null)
            {
                levelUI.levelNumberText.text = levelNumber.ToString();
            }

            UpdateStarObjects(levelUI.starObjects, bestStars);
        }
    }

    public void OpenDifficultyPanel(int levelNumber)
    {
        if (progressManager == null)
        {
            progressManager = LevelProgressManager.GetInstance();
        }

        if (!progressManager.IsLevelUnlocked(levelNumber))
        {
            Debug.Log("Level is locked: " + levelNumber);
            return;
        }

        selectedLevelNumber = levelNumber;

        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(true);
        }

        if (difficultyTitleText != null)
        {
            difficultyTitleText.text = "Level " + selectedLevelNumber + " Difficulty";
        }

        RefreshDifficultyButtons();
    }

    public void CloseDifficultyPanel()
    {
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(false);
        }
    }

    private void RefreshDifficultyButtons()
    {
        if (progressManager == null)
        {
            progressManager = LevelProgressManager.GetInstance();
        }

        if (normalButton != null)
        {
            normalButton.interactable = progressManager.CanPlayDifficulty(selectedLevelNumber, Difficulty.Normal);
        }

        if (hardButton != null)
        {
            hardButton.interactable = progressManager.CanPlayDifficulty(selectedLevelNumber, Difficulty.Hard);
        }

        if (extremeButton != null)
        {
            extremeButton.interactable = progressManager.CanPlayDifficulty(selectedLevelNumber, Difficulty.Extreme);
        }
    }

    private void StartLevel(Difficulty difficulty)
    {
        if (progressManager == null)
        {
            progressManager = LevelProgressManager.GetInstance();
        }

        bool canPlay = progressManager.CanPlayDifficulty(selectedLevelNumber, difficulty);

        if (!canPlay)
        {
            Debug.Log("Cannot play this difficulty yet.");
            return;
        }

        DifficultyManager.SelectLevelAndDifficulty(selectedLevelNumber, difficulty);

        SceneManager.LoadScene(gameSceneName);
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
