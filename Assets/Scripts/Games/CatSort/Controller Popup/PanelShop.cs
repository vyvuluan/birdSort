using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Extensions;
[Serializable]
public class ShopAndOnwed
{
    [SerializeField] private RectTransform shop;
    [SerializeField] private RectTransform owned;

    public RectTransform Shop { get => shop; set => shop = value; }
    public RectTransform Owned { get => owned; set => owned = value; }
}
public class PanelShop : MonoBehaviour
{
    [SerializeField] private UnityEvent<int> onButtonTabSelected;
    [SerializeField] private UnityEvent<bool> onButtonCloseShop;
    [SerializeField] private UnityEvent<int,int> onButtonShopOrOwnedTabOne;
    [SerializeField] private UnityEvent<int,int> onButtonShopOrOwnedTabTwo;
    [SerializeField] private UnityEvent<int,int> onButtonShopOrOwnedTabThree;
    [SerializeField] private List<ShopAndOnwed> shops;
    [SerializeField] private GameObject skinItemPrefabOnwed;
    [SerializeField] private GameObject skinItemPrefabSell;

    private Dictionary<SkinItem, SkinItemPrefabSell> skinItemDicSell;
    private Dictionary<SkinItem, SkinItemPrefabOwned> skinItemDicOwned;
    private Dictionary<SkinItem, int> dicTemp;
    private SkinItem barEquip;
    private SkinItem bgEquip;
    private int count;
    private void Awake()
    {
        skinItemPrefabOnwed.ThrowIfNull();
        skinItemPrefabSell.ThrowIfNull();
    }
    public void Init(List<SkinItem> skinItems, Action<SkinItem> onClickBuy, Action<SkinItem> onClickOwned, int lvlCurrent)
    {
        count = 1;
        skinItemDicSell = new Dictionary<SkinItem, SkinItemPrefabSell>();
        skinItemDicOwned = new Dictionary<SkinItem, SkinItemPrefabOwned>();
        dicTemp = new Dictionary<SkinItem, int>();
        foreach (var skinItem in skinItems)
        {
            //Get skin item using of bar and background
            if (skinItem.Status == StatusState.Equip)
            {
                if(skinItem.SkinType == SkinType.Bar)
                {
                    barEquip = skinItem;
                }
                else if (skinItem.SkinType == SkinType.Background)
                {
                    bgEquip = skinItem;
                }    
                    
            }
            //init skin item in tab shop 
            if (skinItem.Status == StatusState.Buy)
            {
                SkinItemPrefabSell prefab = SimplePool.Spawn(skinItemPrefabSell, Vector3.zero, Quaternion.identity).GetComponent<SkinItemPrefabSell>();
                switch (skinItem.SkinType)
                {
                    case SkinType.Bird:
                        prefab.Init(shops[0].Shop, skinItem, onClickBuy, lvlCurrent);
                        break;
                    case SkinType.Bar:
                        prefab.Init(shops[1].Shop, skinItem, onClickBuy, lvlCurrent);
                        break;
                    case SkinType.Background:
                        prefab.Init(shops[2].Shop, skinItem, onClickBuy, lvlCurrent);
                        break;
                }
                skinItemDicSell.Add(skinItem, prefab);
            }
            //init skin item in tab owned 
            else if (skinItem.Status == StatusState.Own || skinItem.Status == StatusState.Equip)
            {
                SkinItemPrefabOwned prefab = SimplePool.Spawn(skinItemPrefabOnwed, Vector3.zero, Quaternion.identity).GetComponent<SkinItemPrefabOwned>();
                switch (skinItem.SkinType)
                {
                    case SkinType.Bird:
                        if(skinItem.Status == StatusState.Equip)
                        {
                            prefab.Init(count, shops[0].Owned, skinItem, onClickOwned);
                            dicTemp.Add(skinItem, count);
                            count++;
                        }
                        else prefab.Init(0, shops[0].Owned, skinItem, onClickOwned);
                        break;
                    case SkinType.Bar:
                        prefab.Init(0, shops[1].Owned, skinItem, onClickOwned);
                        break;
                    case SkinType.Background:
                        prefab.Init(0, shops[2].Owned, skinItem, onClickOwned);
                        break;
                }
                skinItemDicOwned.Add(skinItem, prefab);
            }

        }
    }
    public void UpdateEquip(SkinItem skinItem, Action<int, int> onSetSkin)
    {
        switch(skinItem.SkinType)
        { 
            case SkinType.Bird: 
                //Equip
                if(skinItem.Status == StatusState.Own)
                {
                    //find position null from 1 to 8
                    int posTemp = 0;
                    for(int i = 1; i <= 8; i++)
                    {
                        if (!dicTemp.ContainsValue(i))
                        {
                            posTemp = i;
                            break;
                        }
                    }
                    onSetSkin.Invoke(posTemp, skinItem.Id);
                    //add into dicTemp (dictionary contain bird equip)
                    dicTemp.Add(skinItem, posTemp); 
                    int number = dicTemp[skinItem];
                    skinItemDicOwned[skinItem].SetEquipBird(number);
                }
                //Owned
                else if (skinItem.Status == StatusState.Equip)
                {
                    skinItemDicOwned[skinItem].SetOwn();
                    dicTemp.Remove(skinItem);
                }    
                break;
            case SkinType.Bar:
                if (barEquip != null) skinItemDicOwned[barEquip].SetOwn();
                barEquip = skinItem;
                skinItemDicOwned[barEquip].SetEquip();
                break;
            case SkinType.Background:
                if (bgEquip != null) skinItemDicOwned[bgEquip].SetOwn();
                bgEquip = skinItem;
                skinItemDicOwned[bgEquip].SetEquip();
                break;

        }
    }
    public void UpdateBuy(SkinItem skinItem, Action<SkinItem> onClickOwned)
    {
        SkinItemPrefabOwned prefab = SimplePool.Spawn(skinItemPrefabOnwed, Vector3.zero, Quaternion.identity).GetComponent<SkinItemPrefabOwned>();
        skinItem.Status = StatusState.Own;
        switch (skinItem.SkinType)
        {
            case SkinType.Bird:
                prefab.Init(0, shops[0].Owned, skinItem, onClickOwned);
                break;
            case SkinType.Bar:
                prefab.Init(0, shops[1].Owned, skinItem, onClickOwned);
                break;
            case SkinType.Background:
                prefab.Init(0, shops[2].Owned, skinItem, onClickOwned);
                break;

        }
        Destroy(skinItemDicSell[skinItem].gameObject);
        skinItemDicSell.Remove(skinItem);
        skinItemDicOwned.Add(skinItem, prefab);

    }       
    public void OnTabSelected(int number)
    {
        onButtonTabSelected?.Invoke(number);
    }    
    public void OnCloseShop()
    {
        onButtonCloseShop?.Invoke(false);
    }  
    public void OnShopTabOneSelected()
    {
        onButtonShopOrOwnedTabOne?.Invoke(1, 0);
    }
    public void OnOwnedTabOneSelected()
    {
        onButtonShopOrOwnedTabTwo?.Invoke(2, 0);
    }
    public void OnShopTabTwoSelected()
    {
        onButtonShopOrOwnedTabTwo?.Invoke(1, 1);
    }
    public void OnOwnedTabTwoSelected()
    {
        onButtonShopOrOwnedTabTwo?.Invoke(2, 1);
    }
    public void OnShopTabThreeSelected()
    {
        onButtonShopOrOwnedTabThree?.Invoke(1, 2);
    }
    public void OnOwnedTabThreeSelected()
    {
        onButtonShopOrOwnedTabThree?.Invoke(2, 2);
    }
}
