using UnityEngine;
using UnityEngine.Events;

public class PanelResetController : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> onButtonClose;
    [SerializeField] private UnityEvent onButtonResetMap;
    public void OffPanelReset()
    {
        onButtonClose?.Invoke(false);
    }
    public void ResetMapLevel()
    {
        onButtonResetMap?.Invoke();
    }
}
