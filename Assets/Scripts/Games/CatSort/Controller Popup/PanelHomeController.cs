using UnityEngine;
using UnityEngine.Events;

public class PanelHomeController : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> onButtonPenalSetting;
    [SerializeField] private UnityEvent<bool> onButtonClose;
    public void OnPenalSetting()
    {
        onButtonPenalSetting?.Invoke(true);
    }
    public void OffPanelHome()
    {
        onButtonClose?.Invoke(false);
    }
}
