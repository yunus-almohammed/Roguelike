using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade UI")]
    public GameObject upgradePanel;
    public Transform[] cardSlots;

    [Header("Card Layout")]
    [Tooltip("يُستخدم فقط إذا كان حجم البطاقة داخل الـPrefab يساوي صفرًا.")]
    public Vector2 fallbackCardSize = new Vector2(230f, 340f);

    [Header("Upgrade Card Prefabs")]
    public GameObject[] commonCards;
    public GameObject[] uncommonCards;
    public GameObject[] rareCards;
    public GameObject[] epicCards;
    public GameObject[] legendaryCards;

    private readonly List<GameObject> spawnedCards = new List<GameObject>();

    public bool IsUpgradePanelOpen { get; private set; }

    public event Action OnUpgradeChosen;

    private void Start()
    {
        Time.timeScale = 1f;
        IsUpgradePanelOpen = false;

        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
    }

    public void ShowUpgradeChoices()
    {
        if (IsUpgradePanelOpen)
            return;

        ClearCards();

        if (upgradePanel == null)
        {
            Debug.LogError("Upgrade Panel is not connected.");
            return;
        }

        if (cardSlots == null || cardSlots.Length == 0)
        {
            Debug.LogError("Card Slots are not connected.");
            return;
        }

        upgradePanel.SetActive(true);
        IsUpgradePanelOpen = true;

        Time.timeScale = 0f;

        HashSet<GameObject> usedCards = new HashSet<GameObject>();

        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i] == null)
            {
                Debug.LogError("Card Slot " + i + " is missing.");
                continue;
            }

            GameObject selectedCardPrefab = GetRandomCard(usedCards);

            if (selectedCardPrefab == null)
            {
                Debug.LogError("No Upgrade Card was found.");
                continue;
            }

            usedCards.Add(selectedCardPrefab);

            // قراءة حجم البطاقة الأصلي من الـPrefab قبل إنشائها.
            Vector2 originalCardSize = GetPrefabCardSize(selectedCardPrefab);

            GameObject spawnedCard = Instantiate(
                selectedCardPrefab,
                cardSlots[i],
                false
            );

            ConfigureCardTransform(spawnedCard, originalCardSize);

            UpgradeCard upgradeCard =
                spawnedCard.GetComponent<UpgradeCard>();

            if (upgradeCard != null)
            {
                upgradeCard.Initialize(this);
            }
            else
            {
                Debug.LogError(
                    spawnedCard.name +
                    " does not have UpgradeCard script."
                );
            }

            spawnedCards.Add(spawnedCard);
        }
    }

    private Vector2 GetPrefabCardSize(GameObject cardPrefab)
    {
        RectTransform prefabRect =
            cardPrefab.GetComponent<RectTransform>();

        if (prefabRect == null)
        {
            return fallbackCardSize;
        }

        Vector2 prefabSize = prefabRect.rect.size;

        if (prefabSize.x <= 0f || prefabSize.y <= 0f)
        {
            return fallbackCardSize;
        }

        return prefabSize;
    }

    private void ConfigureCardTransform(
        GameObject cardObject,
        Vector2 originalSize)
    {
        RectTransform cardRect =
            cardObject.GetComponent<RectTransform>();

        if (cardRect == null)
        {
            Debug.LogError(
                cardObject.name +
                " does not have a RectTransform."
            );

            return;
        }

        // وضع البطاقة في منتصف الـContainer.
        cardRect.anchorMin = new Vector2(0.5f, 0.5f);
        cardRect.anchorMax = new Vector2(0.5f, 0.5f);
        cardRect.pivot = new Vector2(0.5f, 0.5f);

        cardRect.anchoredPosition = Vector2.zero;
        cardRect.localRotation = Quaternion.identity;
        cardRect.localScale = Vector3.one;

        // المحافظة على حجم البطاقة الموجود في الـPrefab.
        cardRect.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            originalSize.x
        );

        cardRect.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            originalSize.y
        );
    }

    private GameObject GetRandomCard(HashSet<GameObject> usedCards)
    {
        for (int attempt = 0; attempt < 50; attempt++)
        {
            UpgradeCard.UpgradeRarity rarity = GetRandomRarity();
            GameObject[] cardPool = GetCardPool(rarity);

            if (cardPool == null || cardPool.Length == 0)
                continue;

            GameObject selectedCard =
                cardPool[Random.Range(0, cardPool.Length)];

            if (selectedCard != null &&
                !usedCards.Contains(selectedCard))
            {
                return selectedCard;
            }
        }

        return GetAnyUnusedCard(usedCards);
    }

    private UpgradeCard.UpgradeRarity GetRandomRarity()
    {
        int roll = Random.Range(1, 101);

        if (roll <= 50)
            return UpgradeCard.UpgradeRarity.Common;

        if (roll <= 75)
            return UpgradeCard.UpgradeRarity.Uncommon;

        if (roll <= 90)
            return UpgradeCard.UpgradeRarity.Rare;

        if (roll <= 98)
            return UpgradeCard.UpgradeRarity.Epic;

        return UpgradeCard.UpgradeRarity.Legendary;
    }

    private GameObject[] GetCardPool(
        UpgradeCard.UpgradeRarity rarity)
    {
        switch (rarity)
        {
            case UpgradeCard.UpgradeRarity.Common:
                return commonCards;

            case UpgradeCard.UpgradeRarity.Uncommon:
                return uncommonCards;

            case UpgradeCard.UpgradeRarity.Rare:
                return rareCards;

            case UpgradeCard.UpgradeRarity.Epic:
                return epicCards;

            case UpgradeCard.UpgradeRarity.Legendary:
                return legendaryCards;

            default:
                return commonCards;
        }
    }

    private GameObject GetAnyUnusedCard(
        HashSet<GameObject> usedCards)
    {
        List<GameObject> allCards = new List<GameObject>();

        AddCardsToList(allCards, commonCards);
        AddCardsToList(allCards, uncommonCards);
        AddCardsToList(allCards, rareCards);
        AddCardsToList(allCards, epicCards);
        AddCardsToList(allCards, legendaryCards);

        allCards.RemoveAll(card =>
            card == null || usedCards.Contains(card));

        if (allCards.Count == 0)
            return null;

        return allCards[Random.Range(0, allCards.Count)];
    }

    private void AddCardsToList(
        List<GameObject> list,
        GameObject[] cards)
    {
        if (cards == null)
            return;

        list.AddRange(cards);
    }

    public void SelectUpgrade(UpgradeCard selectedCard)
    {
        if (selectedCard == null)
            return;

        Debug.Log(
            "Selected Upgrade: " +
            selectedCard.upgradeType +
            " | " +
            selectedCard.rarity +
            " | Value: " +
            selectedCard.upgradeValue
        );

        // سنضيف تأثير القدرة الحقيقي لاحقًا.

        CloseUpgradePanel();
    }

    private void CloseUpgradePanel()
    {
        ClearCards();

        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }

        IsUpgradePanelOpen = false;
        Time.timeScale = 1f;

        OnUpgradeChosen?.Invoke();
    }

    private void ClearCards()
    {
        foreach (GameObject card in spawnedCards)
        {
            if (card != null)
            {
                Destroy(card);
            }
        }

        spawnedCards.Clear();
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}