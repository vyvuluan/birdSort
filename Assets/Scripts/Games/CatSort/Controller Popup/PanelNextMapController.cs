using UnityEngine;
using UnityEngine.Events;

public class PanelNextMapController : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> onButtonClose;
    [SerializeField] private UnityEvent onButtonNextMap;
    [SerializeField] private UnityEvent<BoosterType, int> onButtonBuyOneBoosterNextMap;
    [SerializeField] private UnityEvent<BoosterType> onButtonBuyOneAds;
    [SerializeField] private UnityEvent<BoosterType, int> onButtonBuyThreeBoosterNextMap;
    public void OffPanelNextMapBooster()
    {
        onButtonClose?.Invoke(false);
    }
    public void NextLevelMap()
    {
        onButtonNextMap?.Invoke();
    }
    public void BuyOneBoosterNextMap()
    {
        onButtonBuyOneBoosterNextMap?.Invoke(BoosterType.NextMap, 1);
    }
    public void BuyThreeBoosterNextMap()
    {
        onButtonBuyThreeBoosterNextMap?.Invoke(BoosterType.NextMap, 3);
    }
    public void BuyOneAds()
    {
        onButtonBuyOneAds?.Invoke(BoosterType.NextMap);

    }
}
