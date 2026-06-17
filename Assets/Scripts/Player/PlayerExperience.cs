using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerExperience : MonoBehaviour
{
    [Header("XP Settings")]
    public int currentLevel = 0;
    public int currentXP = 0;
    public int xpToNextLevel = 10;

    [Header("UI")]
    public Image xpBarFill;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;

    [Header("RGB Bar")]
    public bool useRGBColor = true;
    public float rgbSpeed = 0.5f;

    private void Start()
    {
        UpdateXPUI();
    }

    private void Update()
    {
        if (useRGBColor && xpBarFill != null)
        {
            float hue = Mathf.PingPong(Time.time * rgbSpeed, 1f);
            xpBarFill.color = Color.HSVToRGB(hue, 1f, 1f);
        }
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }

        UpdateXPUI();

        Debug.Log("XP: " + currentXP + " / " + xpToNextLevel);
    }

    private void LevelUp()
    {
        currentLevel++;
        xpToNextLevel += 10;

        Debug.Log("Level Up! Current Level: " + currentLevel);
    }

    private void UpdateXPUI()
    {
        if (xpBarFill != null)
        {
            xpBarFill.fillAmount = (float)currentXP / xpToNextLevel;
        }

        if (levelText != null)
        {
            levelText.text = "Lvl " + currentLevel;
        }


    }

}