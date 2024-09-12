using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


[System.Serializable]
public class IncrementalData
{
    public GameObject image; // Holds the image GameObject
    public string name;      // Holds the name
    public string info;      // Holds the info
    public int price;
}
public class IncrementalManager : MonoBehaviour
{   
    [SerializeField] private GameObject infoSpecialPanel;
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField] private GameData gameData;
    // List of IncrementalData to hold all the images, names, and info
    [SerializeField] private List<IncrementalData> incrementalDataList = new List<IncrementalData>();
    [SerializeField] private Ease ease;

    private int selectedIndex;

    [Header("Special Panel / Types")]
    [SerializeField] private GameObject redoType;
    [SerializeField] private GameObject orderType;
    [SerializeField] private GameObject teleportType;
    [SerializeField] private GameObject bombType;

    [Header("Special Panel / Locks")]
    [SerializeField] private GameObject redoLock;
    [SerializeField] private GameObject orderLock;
    [SerializeField] private GameObject teleportLock;
    [SerializeField] private GameObject bombLock;

    [Header("Special Panel / Pluses")]
    [SerializeField] private GameObject redoPlus;
    [SerializeField] private GameObject orderPlus;
    [SerializeField] private GameObject teleportPlus;
    [SerializeField] private GameObject bombPlus;

    [Header("Special Panel / Infoes")]
    [SerializeField] private GameObject redoInfo;
    [SerializeField] private GameObject orderInfo;
    [SerializeField] private GameObject teleportInfo;
    [SerializeField] private GameObject bombInfo;


    [Header("Special Panel / Amounts")]
    [SerializeField] private int redoAmount;
    [SerializeField] private int orderAmount;
    [SerializeField] private int teleportAmount;
    [SerializeField] private int bombAmount;

    [Header("Special Panel / Prices")]
    [SerializeField] private int redoPrice;
    [SerializeField] private int orderPrice;
    [SerializeField] private int teleportPrice;
    [SerializeField] private int bombPrice;

    [Header("Special Panel / Active Level")]
    [SerializeField] private int redoLevel;
    [SerializeField] private int orderLevel;
    [SerializeField] private int teleportLevel;
    [SerializeField] private int bombLevel;

    [Header("Special Panel / Active Level Texts")]
    [SerializeField] private TextMeshProUGUI redoLevelText;
    [SerializeField] private TextMeshProUGUI orderLevelText;
    [SerializeField] private TextMeshProUGUI teleportLevelText;
    [SerializeField] private TextMeshProUGUI bombLevelText;


    [Header("Special Panel / Amount Texts")]
    [SerializeField] private TextMeshProUGUI redoAmountText;
    [SerializeField] private TextMeshProUGUI orderAmountText;
    [SerializeField] private TextMeshProUGUI teleportAmountText;
    [SerializeField] private TextMeshProUGUI bombAmountText;

    private void Start()
    {
        GetAmountPrefs();
        CheckIfLevelActive();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
    }

    private void OnNextLevel()
    {
        CheckIfLevelActive();
    }

    private void CheckIfLevelActive()
    {
        CheckActivityLevel(redoType,redoLock,redoLevel,redoLevelText);
        CheckActivityLevel(orderType,orderLock,orderLevel,orderLevelText);
        CheckActivityLevel(teleportType,teleportLock,teleportLevel,teleportLevelText);
        CheckActivityLevel(bombType,bombLock,bombLevel,bombLevelText);
    }

    private void CheckActivityLevel(GameObject incType, GameObject lockType, int typesLevel,TextMeshProUGUI levelText)
    {
        if(typesLevel>gameData.levelNumber)
        {
            incType.SetActive(false);
            lockType.SetActive(true);
            levelText.SetText("LVL " + (typesLevel+1).ToString());
            
        }

        else
        {
            incType.SetActive(true);
            lockType.SetActive(false);
        }
    }

    private void GetAmountPrefs()
    {
        GetAllAmounts();
        GetAllAmountTexts();
        CheckAllAmounts();
        
        
    }

    private void GetAllAmounts()
    {
        redoAmount=GetPrefs("redoAmount",0);
        orderAmount=GetPrefs("orderAmount",0);
        teleportAmount=GetPrefs("teleportAmount",0);
        bombAmount=GetPrefs("bombAmount",0);
    }

    private void SetAmount(string key,int val)
    {
        PlayerPrefs.SetInt(key,val);
        
    }

    private void GetAllAmountTexts()
    {
        GetAmountTexts(redoAmountText,redoAmount);
        GetAmountTexts(orderAmountText,orderAmount);
        GetAmountTexts(teleportAmountText,teleportAmount);
        GetAmountTexts(bombAmountText,bombAmount);
    }

    private void CheckAllAmounts()
    {
        CheckGreaterThanZero(redoPlus,redoInfo,redoAmount);;
        CheckGreaterThanZero(orderPlus,orderInfo,orderAmount);
        CheckGreaterThanZero(teleportPlus,teleportInfo,teleportAmount);
        CheckGreaterThanZero(bombPlus,bombInfo,bombAmount);
    }

    private int GetPrefs(string key,int defaultVal)
    {
        return PlayerPrefs.GetInt(key,defaultVal);
    }

    private void GetAmountTexts(TextMeshProUGUI amountText,int val)
    {
        amountText.SetText(val.ToString());
    }

    private void CheckGreaterThanZero(GameObject plus,GameObject Amount,int val)
    {
        if(val>0)
        {
            Amount.SetActive(true);
            plus.SetActive(false);
        }

        else
        {
            Amount.SetActive(false);
            plus.SetActive(true);
        }
    }

    

    #region Buttons
    public void OpenIncrementalInfo(int index)
    {
        if (index < 0 || index >= incrementalDataList.Count) return; // Safety check for index

        // Activate the info panel
        infoSpecialPanel.SetActive(true);
        infoSpecialPanel.transform.DOScale(Vector3.one,0.5f).SetEase(ease);
        EventManager.Broadcast(GameEvent.OnIncrementalPress);
        // Hide all images and only show the selected one
        for (int i = 0; i < incrementalDataList.Count; i++)
        {
            incrementalDataList[i].image.SetActive(i == index);
        }

        // Set header and info text
        headerText.text = incrementalDataList[index].name;
        infoText.text = incrementalDataList[index].info;
        priceText.text= incrementalDataList[index].price.ToString();
        
        selectedIndex=index;
    }

    public void BuySelected()
    {
        switch(selectedIndex)
        {
            case 0:
                BuyReDo();
                break;
            case 1:
                BuyOrder();
                break;
            case 2:
                BuyTeleport();
                break;
            case 3:
                BuyBomb();
                break;
        }
    }

    private void BuyReDo()
    {
        if(gameData.score>=redoPrice)
        {
            redoAmount=2;
            GetAmountTexts(redoAmountText,redoAmount);
            gameData.score-=redoPrice;
            PlayerPrefs.SetInt("ScoreGame",gameData.score);
            EventManager.Broadcast(GameEvent.OnUIUpdate);
            SetAmount("redoAmount",redoAmount);
            redoPlus.SetActive(false);
            redoInfo.SetActive(true);
            BackButton();
        }
        
    }

    private void BuyOrder()
    {
        if(gameData.score>=orderPrice)
        {
            orderAmount=2;
            GetAmountTexts(orderAmountText,orderAmount);
            gameData.score-=orderPrice;
            PlayerPrefs.SetInt("ScoreGame",gameData.score);
            EventManager.Broadcast(GameEvent.OnUIUpdate);
            SetAmount("orderAmount",orderAmount);
            orderPlus.SetActive(false);
            orderInfo.SetActive(true);
            BackButton();
        }
        
    }

    private void BuyTeleport()
    {
        if(gameData.score>=teleportPrice)
        {
            teleportAmount=2;
            GetAmountTexts(teleportAmountText,teleportAmount);
            gameData.score-=teleportPrice;
            PlayerPrefs.SetInt("ScoreGame",gameData.score);
            EventManager.Broadcast(GameEvent.OnUIUpdate);
            SetAmount("teleportAmount",teleportAmount);
            teleportPlus.SetActive(false);
            teleportInfo.SetActive(true);
            BackButton();
        }
        
    }

    private void BuyBomb()
    {
        if(gameData.score>=bombPrice)
        {
            bombAmount=2;
            GetAmountTexts(bombAmountText,bombAmount);
            gameData.score-=bombPrice;
            PlayerPrefs.SetInt("ScoreGame",gameData.score);
            EventManager.Broadcast(GameEvent.OnUIUpdate);
            SetAmount("bombAmount",bombAmount);
            bombPlus.SetActive(false);
            bombInfo.SetActive(true);
            BackButton();
        }
        
    }




    public void BackButton()
    {
        infoSpecialPanel.transform.DOScale(Vector3.zero,0.5f).SetEase(ease).OnComplete(()=>{
            infoSpecialPanel.SetActive(false);;
        });
        
    }

    #endregion
}

