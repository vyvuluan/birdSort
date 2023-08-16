using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinItemPrefabBirdOwned : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI useTxt;
    [SerializeField] private Image image;
    [SerializeField] private Button buttonSelected;
    [SerializeField] private Button buttonUnSelected;

    private SkinItem skinItem;
    private System.Action<SkinItem, System.Action<bool>> onClickSelected;
    private System.Action<SkinItem, System.Action<bool>> onClickUnSelected;
    private void Awake()
    {
        useTxt.ThrowIfNull();
        image.ThrowIfNull();
        buttonSelected.ThrowIfNull();
    }
    public void Init(RectTransform parent, SkinItem skinItem, System.Action<SkinItem, System.Action<bool>> onClickSelected, System.Action<SkinItem, System.Action<bool>> onClickUnSelected)
    {
        transform.SetParent(parent, false);
        transform.localScale = Vector3.one;
        this.skinItem = skinItem;
        this.onClickSelected = onClickSelected;
        this.onClickUnSelected = onClickUnSelected;
        image.sprite = skinItem.Image;
        switch (skinItem.Status)
        {
            case StatusState.Own:
                SetOwn();
                break;
            case StatusState.Equip:
                SetEquip();
                break;
        }
    }
    public void OnBtnSelectedClick()
    {
        onClickSelected?.Invoke(skinItem, ProcessUsingSkin);
    }
    public void OnBtnUnSelectedClick()
    {
        onClickUnSelected?.Invoke(skinItem, ProcessUnUsingSkin);
    }
    private void ProcessUsingSkin(bool isUsingSkin)
    {
        if (isUsingSkin)
        {
            SetEquip();
        }
    }
    private void ProcessUnUsingSkin(bool isUsingSkin)
    {
        if (isUsingSkin)
        {
            SetOwn();
        }
    }
    public void SetOwn()
    {
        skinItem.Status = StatusState.Own;
        useTxt.gameObject.SetActive(false);
        buttonSelected.gameObject.SetActive(true);
        buttonUnSelected.gameObject.SetActive(false);
    }
    public void SetEquip()
    {
        skinItem.Status = StatusState.Equip;
        useTxt.gameObject.SetActive(true);
        buttonSelected.gameObject.SetActive(false);
        buttonUnSelected.gameObject.SetActive(true);
    }
}
