using Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinItemPrefabSell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private Image image;
    [SerializeField] private Image mask;
    [SerializeField] private TextMeshProUGUI textLevelUnlock;
    [SerializeField] private GameObject buttonBuy;
    private SkinItem skinItem;
    private Action<SkinItem> onClickBuy;
    private void Awake()
    {
        priceTxt.ThrowIfNull();
        image.ThrowIfNull();
        mask.ThrowIfNull();
        textLevelUnlock.ThrowIfNull();
        buttonBuy.ThrowIfNull();
    }
    public void Init(RectTransform parent, SkinItem skinItem, Action<SkinItem> onClickBuy, int lvlCurrent)
    {
        transform.SetParent(parent,false);
        transform.localScale = Vector3.one;
        this.skinItem = skinItem;
        this.onClickBuy = onClickBuy;
        priceTxt.text = skinItem.Price.ToString();
        image.sprite = skinItem.Image;
        if (lvlCurrent >= skinItem.LevelUnlock)
        {
            mask.gameObject.SetActive(false);
            buttonBuy.SetActive(true);
        }
        else
        {
            mask.gameObject.SetActive(true);
            buttonBuy.SetActive(false);
            textLevelUnlock.text = "Level " + skinItem.LevelUnlock + " unlock";
        }
      
    }
    public void OnBtnClick()
    {
        onClickBuy?.Invoke(skinItem);
    }

}
