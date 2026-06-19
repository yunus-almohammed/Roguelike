using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerExperience : MonoBehaviour
{
    [Header("XP Settings")]
    public int currentLevel = 0;
    public int currentXP = 0;
    public int xpToNextLevel = 10;

    [Header("XP UI")]
    public Image xpBarFill;
    public TextMeshProUGUI levelText;

    [Header("Upgrade System")]
    public UpgradeManager upgradeManager;

    [Header("RGB Bar")]
    public bool useRGBColor = true;
    public float rgbSpeed = 0.5f;

    private int pendingUpgradeChoices;

    private void Start()
    {
        Time.timeScale = 1f;

        if (upgradeManager != null)
        {
            upgradeManager.OnUpgradeChosen += HandleUpgradeChosen;
        }

        UpdateXPUI();
    }

    private void Update()
    {
        if (useRGBColor && xpBarFill != null)
        {
            float hue = Mathf.PingPong(
                Time.unscaledTime * rgbSpeed,
                1f
            );

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
        TryShowNextUpgrade();
    }

    private void LevelUp()
    {
        currentLevel++;
        xpToNextLevel += 10;
        pendingUpgradeChoices++;

        Debug.Log("Level Up! Current Level: " + currentLevel);
    }

    private void TryShowNextUpgrade()
    {
        if (pendingUpgradeChoices <= 0)
            return;

        if (upgradeManager == null)
        {
            Debug.LogError("Upgrade Manager is not connected.");
            return;
        }

        if (upgradeManager.IsUpgradePanelOpen)
            return;

        pendingUpgradeChoices--;
        upgradeManager.ShowUpgradeChoices();
    }

    private void HandleUpgradeChosen()
    {
        TryShowNextUpgrade();
    }

    private void UpdateXPUI()
    {
        if (xpBarFill != null)
        {
            xpBarFill.fillAmount =
                (float)currentXP / xpToNextLevel;
        }

        if (levelText != null)
        {
            levelText.text = "Lvl " + currentLevel;
        }
    }

    private void OnDestroy()
    {
        if (upgradeManager != null)
        {
            upgradeManager.OnUpgradeChosen -= HandleUpgradeChosen;
        }

        Time.timeScale = 1f;
    }
}