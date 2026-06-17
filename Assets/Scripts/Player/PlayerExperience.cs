using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [Header("XP Settings")]
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 10;

    public void AddXP(int amount)
    {
        currentXP += amount;

        Debug.Log("XP: " + currentXP + " / " + xpToNextLevel);

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentXP = 0;
        xpToNextLevel += 10;

        Debug.Log("Level Up! Current Level: " + currentLevel);
    }
}