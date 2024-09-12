using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


[System.Serializable]
public class IncrementalData
{
    public GameObject image;  // Holds the image GameObject
    public string name;       // Holds the name
    public string info;       // Holds the info
    public int price;         // Holds the price
}

public class IncrementalManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject infoSpecialPanel;
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("Game Data")]
    [SerializeField] private GameData gameData;
    [SerializeField] private List<IncrementalData> incrementalDataList = new List<IncrementalData>();
    [SerializeField] private Ease ease;

    private int selectedIndex;

    [Header("Special Panel / Types and Locks")]
    [SerializeField] private List<GameObject> specialTypes;
    [SerializeField] private List<GameObject> specialLocks;
    [SerializeField] private List<GameObject> specialPluses;
    [SerializeField] private List<GameObject> specialInfos;

    [Header("Special Panel / Amounts and Prices")]
    [SerializeField] private List<int> specialAmounts;
    [SerializeField] private List<int> specialPrices;
    [SerializeField] private List<TextMeshProUGUI> specialAmountTexts;
    [SerializeField] private List<TextMeshProUGUI> specialLevelTexts;

    // New List to hold the buttons for each special property
    [Header("Special Panel / Action Buttons")]
    [SerializeField] private List<Button> specialActionButtons;

    private void Start()
    {
        GetAmountPrefs();
        CheckIfLevelActive();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnNextLevel()
    {
        CheckIfLevelActive();
    }

    private void CheckIfLevelActive()
    {
        for (int i = 0; i < specialTypes.Count; i++)
        {
            CheckActivityLevel(specialTypes[i], specialLocks[i], i, specialLevelTexts[i]);
            EnableSpecialButtons(i);
        }
    }

    private void CheckActivityLevel(GameObject incType, GameObject lockType, int index, TextMeshProUGUI levelText)
    {
        int levelRequired = specialPrices[index];
        if (levelRequired > gameData.levelNumber)
        {
            incType.SetActive(false);
            lockType.SetActive(true);
            levelText.SetText($"LVL {levelRequired + 1}");
        }
        else
        {
            incType.SetActive(true);
            lockType.SetActive(false);
        }
    }

    private void GetAmountPrefs()
    {
        for (int i = 0; i < specialAmounts.Count; i++)
        {
            specialAmounts[i] = PlayerPrefs.GetInt($"specialAmount_{i}", 0);
            UpdateAmountText(specialAmountTexts[i], specialAmounts[i]);
            CheckGreaterThanZero(specialPluses[i], specialInfos[i], specialAmounts[i]);
            EnableSpecialButtons(i);
        }
    }

    private void UpdateAmountText(TextMeshProUGUI amountText, int amount)
    {
        amountText.SetText(amount.ToString());
    }

    private void CheckGreaterThanZero(GameObject plus, GameObject info, int amount)
    {
        info.SetActive(amount > 0);
        plus.SetActive(amount <= 0);
    }

    // Method to enable or disable the button based on the special amount
    private void EnableSpecialButtons(int index)
    {
        if (specialAmounts[index] > 0)
        {
            specialActionButtons[index].enabled = true;
        }
        else
        {
            specialActionButtons[index].enabled = false;
        }
    }

    // Method called when a button is pressed, with different skills for each button
    public void UseSpecial(int index)
    {
        if (specialAmounts[index] > 0)
        {
            switch (index)
            {
                case 0:
                    Debug.Log("Special Skill 1 activated!");
                    break;
                case 1:
                    Debug.Log("Special Skill 2 activated!");
                    break;
                case 2:
                    Debug.Log("Special Skill 3 activated!");
                    EventManager.Broadcast(GameEvent.OnTeleportRandomPlayer);
                    break;
                case 3:
                    Debug.Log("Special Skill 4 activated!");
                    break;
                default:
                    Debug.Log("Invalid skill index");
                    break;
            }

            // After using the special power, decrement the special amount
            specialAmounts[index]--;
            UpdateAmountText(specialAmountTexts[index], specialAmounts[index]);

            // Check if we need to disable the button after use
            EnableSpecialButtons(index);

            // Save the new amount
            PlayerPrefs.SetInt($"specialAmount_{index}", specialAmounts[index]);
            CheckGreaterThanZero(specialPluses[index], specialInfos[index], specialAmounts[index]);
        }
        else
        {
            Debug.Log("No special amount left to use this skill.");
        }
    }

    #region Incremental Info Panel
    public void OpenIncrementalInfo(int index)
    {
        if (index < 0 || index >= incrementalDataList.Count) return;

        infoSpecialPanel.SetActive(true);
        infoSpecialPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(ease);
        EventManager.Broadcast(GameEvent.OnIncrementalPress);

        for (int i = 0; i < incrementalDataList.Count; i++)
        {
            incrementalDataList[i].image.SetActive(i == index);
        }

        headerText.text = incrementalDataList[index].name;
        infoText.text = incrementalDataList[index].info;
        priceText.text = incrementalDataList[index].price.ToString();
        selectedIndex = index;
    }

    public void BuySelected()
    {
        if (selectedIndex < 0 || selectedIndex >= specialPrices.Count) return;

        if (gameData.score >= specialPrices[selectedIndex])
        {
            BuyItem(selectedIndex);
            EnableSpecialButtons(selectedIndex);
            
        }
    }

    private void BuyItem(int index)
    {
        specialAmounts[index] = 2;
        UpdateAmountText(specialAmountTexts[index], specialAmounts[index]);

        gameData.score -= specialPrices[index];
        PlayerPrefs.SetInt("ScoreGame", gameData.score);
        EventManager.Broadcast(GameEvent.OnUIUpdate);

        PlayerPrefs.SetInt($"specialAmount_{index}", specialAmounts[index]);
        specialPluses[index].SetActive(false);
        specialInfos[index].SetActive(true);
        BackButton();
    }

    public void BackButton()
    {
        infoSpecialPanel.transform.DOScale(Vector3.zero, 0.5f)
            .SetEase(ease)
            .OnComplete(() => infoSpecialPanel.SetActive(false));
    }
    #endregion
}
