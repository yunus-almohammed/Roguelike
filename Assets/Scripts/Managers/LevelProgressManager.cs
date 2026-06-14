using System;
using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private int totalLevels = 8;

    private GameSaveData saveData;

    public event Action OnProgressChanged;

    public int TotalLevels => totalLevels;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadProgress();
    }

    public static LevelProgressManager GetInstance()
    {
        if (Instance != null)
        {
            return Instance;
        }

        LevelProgressManager foundManager = FindFirstObjectByType<LevelProgressManager>();

        if (foundManager != null)
        {
            return foundManager;
        }

        GameObject managerObject = new GameObject("LevelProgressManager");
        LevelProgressManager createdManager = managerObject.AddComponent<LevelProgressManager>();

        return createdManager;
    }

    private void LoadProgress()
    {
        saveData = SaveSystem.Load(totalLevels);
    }

    public bool IsLevelUnlocked(int levelNumber)
    {
        return levelNumber >= 1 && levelNumber <= saveData.maxUnlockedLevel;
    }

    public bool IsNormalCompleted(int levelNumber)
    {
        int index = GetIndex(levelNumber);

        if (!IsValidIndex(index))
        {
            return false;
        }

        return saveData.normalCompleted[index];
    }

    public int GetBestStars(int levelNumber)
    {
        int index = GetIndex(levelNumber);

        if (!IsValidIndex(index))
        {
            return 0;
        }

        return saveData.bestStars[index];
    }

    public bool CanPlayDifficulty(int levelNumber, Difficulty difficulty)
    {
        if (!IsLevelUnlocked(levelNumber))
        {
            return false;
        }

        bool normalCompleted = IsNormalCompleted(levelNumber);

        if (!normalCompleted)
        {
            return difficulty == Difficulty.Normal;
        }

        return true;
    }

    public void CompleteLevel(int levelNumber, Difficulty difficulty)
    {
        if (!IsLevelUnlocked(levelNumber))
        {
            Debug.LogWarning("Cannot complete locked level: " + levelNumber);
            return;
        }

        if (!CanPlayDifficulty(levelNumber, difficulty))
        {
            Debug.LogWarning("Difficulty is not available yet: " + difficulty);
            return;
        }

        int index = GetIndex(levelNumber);
        int earnedStars = DifficultyManager.GetStarsForDifficulty(difficulty);

        if (earnedStars > saveData.bestStars[index])
        {
            saveData.bestStars[index] = earnedStars;
        }

        if (difficulty == Difficulty.Normal)
        {
            saveData.normalCompleted[index] = true;

            if (levelNumber == saveData.maxUnlockedLevel && saveData.maxUnlockedLevel < totalLevels)
            {
                saveData.maxUnlockedLevel++;
            }
        }

        SaveSystem.Save(saveData);
        OnProgressChanged?.Invoke();

        Debug.Log("Saved Level Progress. Level: " + levelNumber + " Difficulty: " + difficulty + " Stars: " + earnedStars);
    }

    public void ResetProgress()
    {
        SaveSystem.DeleteSave();
        saveData = GameSaveData.CreateNew(totalLevels);
        SaveSystem.Save(saveData);

        OnProgressChanged?.Invoke();

        Debug.Log("Progress reset.");
    }

    private int GetIndex(int levelNumber)
    {
        return levelNumber - 1;
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < totalLevels;
    }
}
