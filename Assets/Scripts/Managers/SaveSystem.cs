using System;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public int maxUnlockedLevel = 1;
    public int[] bestStars;
    public bool[] normalCompleted;

    public static GameSaveData CreateNew(int totalLevels)
    {
        GameSaveData data = new GameSaveData
        {
            maxUnlockedLevel = 1,
            bestStars = new int[totalLevels],
            normalCompleted = new bool[totalLevels]
        };

        return data;
    }
}

public static class SaveSystem
{
    private const string SaveKey = "Roguelike_LevelProgress_Save";

    public static GameSaveData Load(int totalLevels)
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            GameSaveData newData = GameSaveData.CreateNew(totalLevels);
            Save(newData);
            return newData;
        }

        string json = PlayerPrefs.GetString(SaveKey);

        if (string.IsNullOrEmpty(json))
        {
            GameSaveData newData = GameSaveData.CreateNew(totalLevels);
            Save(newData);
            return newData;
        }

        GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(json);
        return ValidateData(loadedData, totalLevels);
    }

    public static void Save(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
    }

    private static GameSaveData ValidateData(GameSaveData data, int totalLevels)
    {
        if (data == null)
        {
            return GameSaveData.CreateNew(totalLevels);
        }

        if (data.bestStars == null)
        {
            data.bestStars = Array.Empty<int>();
        }

        if (data.normalCompleted == null)
        {
            data.normalCompleted = Array.Empty<bool>();
        }

        Array.Resize(ref data.bestStars, totalLevels);
        Array.Resize(ref data.normalCompleted, totalLevels);

        data.maxUnlockedLevel = Mathf.Clamp(data.maxUnlockedLevel, 1, totalLevels);

        return data;
    }
}
