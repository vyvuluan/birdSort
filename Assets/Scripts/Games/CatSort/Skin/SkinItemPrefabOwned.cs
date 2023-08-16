using Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinItemPrefabOwned : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI useTxt;
    [SerializeField] private Image image;
    [SerializeField] private Image mask;
    [SerializeField] private Image quantity;
    [SerializeField] private TextMeshProUGUI quantityText;
    private int pos;
    private SkinItem skinItem;
    private Action<SkinItem> onClick;
    private void Awake()
    {
        useTxt.ThrowIfNull();
        image.ThrowIfNull();
        mask.ThrowIfNull();
        quantity.ThrowIfNull();
        quantityText.ThrowIfNull();
    }
    public void Init(int pos, RectTransform parent, SkinItem skinItem, Action<SkinItem> onClick)
    {
        transform.SetParent(parent, false);
        transform.localScale = Vector3.one;
        this.skinItem = skinItem;
        this.onClick = onClick;
        
        image.sprite = skinItem.Image;
        this.pos = pos;
        switch (skinItem.Status)
        {
            case StatusState.Own:
                SetOwn();
                break;
            case StatusState.Equip:
                if(skinItem.SkinType == SkinType.Bird)
                {
                    SetEquipBird(this.pos);
                }    
                else SetEquip();
                break;
        }
    }
    public void OnBtnClick()
    {
        onClick?.Invoke(skinItem);
    }
    public void SetOwn()
    {
        if (skinItem.SkinType == SkinType.Bird)
        {
            skinItem.Status = StatusState.Own;
            mask.gameObject.SetActive(true);
            quantity.gameObject.SetActive(false);
        }
        else
        {
            skinItem.Status = StatusState.Own;
            mask.gameObject.SetActive(true);
        }    
    }
    public void SetEquip()
    {
        skinItem.Status = StatusState.Equip;
        mask.gameObject.SetActive(false);
    }
    public void SetEquipBird(int index)
    {
        skinItem.Status = StatusState.Equip;
        quantityText.text = index.ToString() + "/8";

        mask.gameObject.SetActive(false);
        quantity.gameObject.SetActive(true);
    }    
}
