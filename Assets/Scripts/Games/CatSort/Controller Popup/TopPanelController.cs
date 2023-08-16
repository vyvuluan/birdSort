using UnityEngine;
using UnityEngine.Events;

public class TopPanelController : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> onButtonHomeClick;
    [SerializeField] private UnityEvent<bool> onButtonShowPanelResetMapLevel;
    [SerializeField] private UnityEvent<bool> onButtonGetCoin;
    [SerializeField] private UnityEvent<bool> onButtonGetGift;
    public void ShowPanelResetMapLevel()
    {

        onButtonShowPanelResetMapLevel?.Invoke(true);
    }
    public void ShowPanelHome()
    {
        onButtonHomeClick?.Invoke(true);
    }
    public void ShowPanelGetCoin()
    {
        onButtonGetCoin?.Invoke(true);
    }    
    public void ShowPanelGift()
    {
        onButtonGetGift?.Invoke(true);
    }
}
