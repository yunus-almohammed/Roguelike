using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    public enum UpgradeType
    {
        Damage,
        AttackSpeed,
        Defense,
        MaxHealth,
        MoveSpeed,
        PickupRange
    }

    public enum UpgradeRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    [Header("Upgrade Information")]
    public UpgradeType upgradeType;
    public UpgradeRarity rarity;
    public float upgradeValue;

    [Header("UI")]
    public Button chooseButton;

    private UpgradeManager upgradeManager;

    public void Initialize(UpgradeManager manager)
    {
        upgradeManager = manager;

        if (chooseButton == null)
        {
            Debug.LogError("Choose Button is missing on " + gameObject.name);
            return;
        }

        chooseButton.onClick.RemoveAllListeners();
        chooseButton.onClick.AddListener(ChooseUpgrade);
    }

    private void ChooseUpgrade()
    {
        if (upgradeManager != null)
        {
            upgradeManager.SelectUpgrade(this);
        }
    }
}