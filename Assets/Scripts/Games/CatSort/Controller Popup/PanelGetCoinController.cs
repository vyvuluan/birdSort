using UnityEngine;
using UnityEngine.Events;

public class PanelGetCoinController : MonoBehaviour
{

    [SerializeField] private UnityEvent<bool> onButtonClose;
    [SerializeField] private UnityEvent onGetCoin;
    public void OffPanelGetCoin()
    {
        onButtonClose?.Invoke(false);
    }
    public void GetCoin()
    {
        onGetCoin?.Invoke();
    }
}
