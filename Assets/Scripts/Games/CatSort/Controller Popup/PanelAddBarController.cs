using UnityEngine;
using UnityEngine.Events;

public class PanelAddBarController : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> onButtonClose;
    [SerializeField] private UnityEvent onButtonAddBar;
    [SerializeField] private UnityEvent<BoosterType, int> onButtonBuyOneBoosterAddBar;
    [SerializeField] private UnityEvent<BoosterType> onButtonBuyOneAds;
    [SerializeField] private UnityEvent<BoosterType, int> onButtonBuyThreeBoosterAddBar;
    public void OffPanelAddBarBooster()
    {
        onButtonClose?.Invoke(false);
    }
    public void AddBar()
    {
        onButtonAddBar?.Invoke();
    }
    public void BuyOneBoosterAddBar()
    {
        onButtonBuyOneBoosterAddBar?.Invoke(BoosterType.Add, 1);
    }
    public void BuyThreeBoosterAddBar()
    {
        onButtonBuyOneBoosterAddBar?.Invoke(BoosterType.Add, 3);
    }
    public void BuyOneAds()
    {
        onButtonBuyOneAds?.Invoke(BoosterType.Add);

    }
}
