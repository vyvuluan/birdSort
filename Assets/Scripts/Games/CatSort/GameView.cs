using Extensions;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class ShopTab
{
    [SerializeField] private GameObject shopType;
    [SerializeField] private GameObject buttonTab;
    [SerializeField] private Image selected;

    public GameObject ShopType { get => shopType; set => shopType = value; }
    public GameObject ButtonTab { get => buttonTab; set => buttonTab = value; }
    public Image Selected { get => selected; set => selected = value; }
}
[Serializable]
public class ShopOwnedShop
{
    [SerializeField] private GameObject buttonShopTab;
    [SerializeField] private GameObject buttonOwnedTab;
    [SerializeField] private Image imageButtonShopSelectedTab;
    [SerializeField] private Image imageButtonOwnedSelectedTab;
    [SerializeField] private GameObject panelShopTab;
    [SerializeField] private GameObject panelOwnedTab;

    public GameObject ButtonShopTab { get => buttonShopTab; private set => buttonShopTab = value; }
    public GameObject ButtonOwnedTab { get => buttonOwnedTab; private set => buttonOwnedTab = value; }
    public Image ImageButtonShopSelectedTab { get => imageButtonShopSelectedTab; private set => imageButtonShopSelectedTab = value; }
    public Image ImageButtonOwnedSelectedTab { get => imageButtonOwnedSelectedTab; private set => imageButtonOwnedSelectedTab = value; }
    public GameObject PanelShopTab { get => panelShopTab; private set => panelShopTab = value; }
    public GameObject PanelOwnedTab { get => panelOwnedTab; private set => panelOwnedTab = value; }
}
public class GameView : MonoBehaviour
{
    
    #region PANEL
    [Header("Panel")]
    [Space(8.0f)]
    [SerializeField] private GameObject panelNextMapLevel;
    [SerializeField] private GameObject panelHome;
    [SerializeField] private GameObject panelReset;
    [SerializeField] private GameObject panelSetting;
    [SerializeField] private GameObject panelShop;
    [SerializeField] private GameObject panelBoosterNextMap;
    [SerializeField] private GameObject panelBoosterAddBar;
    [SerializeField] private GameObject panelBoosterRollback;
    [SerializeField] private GameObject panelGetCoin;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject panelLimitPacked;
    #endregion
    #region SHOP
    [Header("Shop")]
    [Space(8.0f)]
    [SerializeField] private List<ShopTab> panelShops;
    [SerializeField] private List<ShopOwnedShop> listShopOwnedShop;
    #endregion
    #region BOOSTER
    [Header("Booster")]
    [Space(8.0f)]
    [SerializeField] private TextMeshProUGUI textQuantityBoosterAddbar;
    [SerializeField] private TextMeshProUGUI textQuantityBoosterRollback;
    [SerializeField] private TextMeshProUGUI textQuantityBoosterNextMap;
    [SerializeField] private Image imagePlusBoosterAddbar;
    [SerializeField] private Image imagePlusBoosterRollback;
    [SerializeField] private Image imagePlusBoosterNextMap;
    [SerializeField] private Image imageButtonRollback;
    [SerializeField] private Image imageButtonAddBar;
    [SerializeField] private Image imageButtonNextMap;
    #endregion
    #region GAMEPLAY
    [Header("Gameplay")]
    [Space(8.0f)]
    [SerializeField] private Slider sliderPercentFinishMap;
    [SerializeField] private TextMeshProUGUI textPercentFinishMap;
    [SerializeField] private Image imageMaskCircle;
    [SerializeField] private Image imageMaskBird;
    [SerializeField] private Image imageBackground;
    [SerializeField] private Sprite imageFish;
    [SerializeField] private GameObject fishScore;
    [SerializeField] private TextMeshProUGUI mapLevelText;
    [SerializeField] private TextMeshProUGUI jumpCount;
    [SerializeField] private TextMeshProUGUI scoreFishText;
    [SerializeField] private TextMeshProUGUI textStatus;
    [SerializeField] private Canvas coin;
    [SerializeField] private Canvas main;
    [SerializeField] private RectTransform adsBannerBottomRight;
    [SerializeField] private RectTransform adsBannerBottomLeft;
    [SerializeField] private GameObject watchAdsBtn;
    [SerializeField] private GameObject prefabCoin;
    [SerializeField] private GameObject imagePanelGetCoin;
    [SerializeField] private GameObject imageCoinInPanelGetCoin;
    #endregion
    public GameObject FishScore { get => fishScore; set => fishScore = value; }
    public TextMeshProUGUI TextStatus { get => textStatus; set => textStatus = value; }
    public Canvas Main { get => main; set => main = value; }
    public RectTransform AdsBannerBottomRight { get => adsBannerBottomRight; set => adsBannerBottomRight = value; }
    public RectTransform AdsBannerBottomLeft { get => adsBannerBottomLeft; set => adsBannerBottomLeft = value; }
    public GameObject PrefabCoin { get => prefabCoin; set => prefabCoin = value; }
    public GameObject ImagePanelGetCoin { get => imagePanelGetCoin; set => imagePanelGetCoin = value; }
    public GameObject ImageCoinInPanelGetCoin { get => imageCoinInPanelGetCoin; set => imageCoinInPanelGetCoin = value; }

    private void Awake()
    {
        panelNextMapLevel.ThrowIfNull();
        panelHome.ThrowIfNull();
        panelReset.ThrowIfNull();
        panelSetting.ThrowIfNull();
        panelShop.ThrowIfNull();
        panelBoosterNextMap.ThrowIfNull();
        panelBoosterAddBar.ThrowIfNull();
        panelBoosterRollback.ThrowIfNull();
        panelGetCoin.ThrowIfNull();
        panelGameOver.ThrowIfNull();

        imageButtonRollback.ThrowIfNull();
        imageButtonAddBar.ThrowIfNull();
        imageButtonNextMap.ThrowIfNull();
        imagePlusBoosterAddbar.ThrowIfNull();
        imagePlusBoosterRollback.ThrowIfNull();
        imagePlusBoosterNextMap.ThrowIfNull();
        textQuantityBoosterAddbar.ThrowIfNull();
        textQuantityBoosterRollback.ThrowIfNull();
        textQuantityBoosterNextMap.ThrowIfNull();

        mapLevelText.ThrowIfNull();
        jumpCount.ThrowIfNull();
        scoreFishText.ThrowIfNull();
        textStatus.ThrowIfNull();
        sliderPercentFinishMap.ThrowIfNull();
        textPercentFinishMap.ThrowIfNull();
        imageMaskCircle.ThrowIfNull();
        imageMaskBird.ThrowIfNull();
        imageBackground.ThrowIfNull();
        imageFish.ThrowIfNull();
        fishScore.ThrowIfNull();
        coin.ThrowIfNull();
    }
    public void HideBtnWatchAds(bool hideAds)
    {
        watchAdsBtn.SetActive(hideAds);
    }    
    public void StatusCoinTop(bool status)
    {
        if(status)
        {
            coin.sortingOrder = 30;
        }    
        else 
        {
            coin.sortingOrder = 31; 
        }
    }      
    public void SetBackground(Sprite sprite)
    {
        imageBackground.sprite = sprite;
    }
    public void SetMapLevel(int level)
    {
        mapLevelText.text = "Level " + level;
    }
    public void SetOpacityBird(SpriteRenderer sr, float opacity)
    {
        Color currentColor = sr.color;
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, opacity);
        sr.color = newColor;
    }
  
    public void SetScore(int score)
    {
        scoreFishText.text = score.ToString();
    }    
    public void ShowPanelNextMapLevel(int mapLevel)
    {
        int mapLevelConvect10 = mapLevel % 10;
        float maskCirle = 1 - mapLevelConvect10 * 0.1f;
        float maskBird = 1;
        switch (mapLevelConvect10)
        {
            case 3:
                maskBird = 0.93f;
                break;
            case 4:
                maskBird = 0.777f;
                break;
            case 5:
                maskBird = 0.625f;
                break;
            case 6:
                maskBird = 0.477f;
                break;
            case 7:
                maskBird = 0.325f;
                break;
            case 8:
                maskBird = 0.172f;
                break;
            case 9:
                maskBird = 0.023f;
                break;
            case 0:
                maskBird = 0f;
                maskCirle = 0f;
                break;
        }
        sliderPercentFinishMap.value = mapLevel;
        imageMaskCircle.fillAmount = maskCirle;
        imageMaskBird.fillAmount = maskBird;
        if(mapLevelConvect10 != 0)
        {
            int percent = mapLevelConvect10 * 10;
            textPercentFinishMap.text = percent.ToString() + "%";
        }
        else textPercentFinishMap.text = "100%";

        panelNextMapLevel.SetActive(true);
    }
    public void PanelHome(bool status)
    {
        panelHome.SetActive(status);
    }
    public void PanelReset(bool status)
    {
        panelReset.SetActive(status);
    }
    public void PanelSetting(bool status)
    {
        panelSetting.SetActive(status);
    }
    public void PanelGetCoin(bool status)
    {
        panelGetCoin.SetActive(status);
    }
    public void PanelShop(bool status)
    {
        panelShop.SetActive(status);
    }
    public void ImageButtonRollback(float value)
    {
        Color color = imageButtonRollback.color;
        color.a = value;
        imageButtonRollback.color = color;

    }
    public void ImageButtonAddBar(float value)
    {
        Color color = imageButtonAddBar.color;
        color.a = value;
        imageButtonAddBar.color = color;
    }
    public void ImageButtonNextMap(float value)
    {
        Color color = imageButtonNextMap.color;
        color.a = value;
        imageButtonNextMap.color = color;
    }

    public void SetTabOn(int index)
    {
        for (int i = 0; i < panelShops.Count; i++)
        {
            if (i == index)
            {
                panelShops[i].ShopType.SetActive(true);
                panelShops[i].ButtonTab.SetActive(false);
                panelShops[i].Selected.gameObject.SetActive(true);
            }
            else
            {
                panelShops[i].ShopType.SetActive(false);
                panelShops[i].ButtonTab.SetActive(true);
                panelShops[i].Selected.gameObject.SetActive(false);
            }
        }    
    } 
    public void SetQuantityBoosterAddbarText(int quantity)
    {
        
        if (quantity > 0)
        {
            textQuantityBoosterAddbar.text = quantity.ToString();
            imagePlusBoosterAddbar.gameObject.SetActive(false);
            ImageButtonAddBar(1f);
        }
        else
        {
            imagePlusBoosterAddbar.gameObject.SetActive(true);
            ImageButtonAddBar(0.5f);
        }

    }
    public void SetQuantityBoosterRollback(int quantity, int rollBackCount)
    {
        if (quantity > 0)
        {
            textQuantityBoosterRollback.text = quantity.ToString();
            imagePlusBoosterRollback.gameObject.SetActive(false);
            if (rollBackCount > 0)
            {
                ImageButtonRollback(1f);              
            }   
            else
            {
                ImageButtonRollback(0.5f);
            }       
        }
        else
        {
            imagePlusBoosterRollback.gameObject.SetActive(true);
            ImageButtonRollback(0.5f);
        }
    }
    public void SetQuantityBoosterNextMap(int quantity)
    {
        if (quantity > 0)
        {
            textQuantityBoosterNextMap.text = quantity.ToString();
            imagePlusBoosterNextMap.gameObject.SetActive(false);
            ImageButtonNextMap(1f);
        }
        else
        {
            
            imagePlusBoosterNextMap.gameObject.SetActive(true);
            ImageButtonNextMap(0.5f);
        }
    }
   
    public void PanelBoosterNextMap(bool status)
    {
        panelBoosterNextMap.SetActive(status);
    }
    public void PanelBoosterRollback(bool status)
    {
        panelBoosterRollback.SetActive(status);
    }
    public void PanelBoosterAddBar(bool status)
    {
        panelBoosterAddBar.SetActive(status);
    }
    public void PanelGameOver(bool status)
    {
        panelGameOver.SetActive(status);
    }
    public void PanelLimitPacked(bool status)
    {
        panelLimitPacked.SetActive(status);
    }
    public void SetJumpCount(int jumpCountInt)
    {
        jumpCount.text = "Step: " + jumpCountInt;
    }
    public void JumpCount(bool status)
    {
        jumpCount.gameObject.SetActive(status);
    }
    //public void SetPositionJumpCount(Transform tf)
    //{
    //    jumpCount.gameObject.transform.position = tf.position;
    //}
    public void SetTextStatus(string text)
    {
        textStatus.text = text;
    }   
    public void SetOnShopOrOwned(int indexShopOwned, int indexTab)
    {
        //indexShopOwned = 1 is select shop, indexShopOwned = 2 is select owned
        if (indexShopOwned == 1)
        {
            listShopOwnedShop[indexTab].ButtonShopTab.SetActive(false);
            listShopOwnedShop[indexTab].ButtonOwnedTab.SetActive(true);
            listShopOwnedShop[indexTab].ImageButtonShopSelectedTab.gameObject.SetActive(true);
            listShopOwnedShop[indexTab].ImageButtonOwnedSelectedTab.gameObject.SetActive(false);
            listShopOwnedShop[indexTab].PanelShopTab.SetActive(true);
            listShopOwnedShop[indexTab].PanelOwnedTab.SetActive(false);
        }
        else
        {
            listShopOwnedShop[indexTab].ButtonShopTab.SetActive(true);
            listShopOwnedShop[indexTab].ButtonOwnedTab.SetActive(false);
            listShopOwnedShop[indexTab].ImageButtonShopSelectedTab.gameObject.SetActive(false);
            listShopOwnedShop[indexTab].ImageButtonOwnedSelectedTab.gameObject.SetActive(true);
            listShopOwnedShop[indexTab].PanelShopTab.SetActive(false);
            listShopOwnedShop[indexTab].PanelOwnedTab.SetActive(true);
        }
    }

}
