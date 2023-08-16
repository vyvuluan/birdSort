using UnityEngine;
using UnityEngine.Events;

public class PanelSettingController : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> onButtonClose;
    public void OffPanelReset()
    {
        onButtonClose?.Invoke(false);
    }
}
