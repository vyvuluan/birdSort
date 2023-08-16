using UnityEngine;
using UnityEngine.Events;

public class PanelRollbackBooster : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> onButtonClose;
    [SerializeField] private UnityEvent onButtonRollback;
    [SerializeField] private UnityEvent<BoosterType, int> onButtonBuyOneBoosterRollback;
    [SerializeField] private UnityEvent<BoosterType> onButtonBuyOneAds;
    [SerializeField] private UnityEvent<BoosterType, int> onButtonBuyThreeBoosterRollback;
    public void OffPanelRollbackBooster()
    {
        onButtonClose?.Invoke(false);
    }
    public void Rollback()
    {
        onButtonRollback?.Invoke();
    }
    public void BuyOneBoosterRollback()
    {
        onButtonBuyOneBoosterRollback?.Invoke(BoosterType.Rollback,1);
    }
    public void BuyThreeBoosterRollback()
    {
        onButtonBuyOneBoosterRollback?.Invoke(BoosterType.Rollback,3);
    }
    public void BuyOneAds()
    {
        onButtonBuyOneAds?.Invoke(BoosterType.Rollback);

    }
}
