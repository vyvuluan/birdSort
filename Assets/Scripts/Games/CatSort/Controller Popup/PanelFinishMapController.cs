using UnityEngine;
using UnityEngine.Events;

public class PanelFinishMapController : MonoBehaviour
{
    [SerializeField] private UnityEvent onButtonLoadNextMapLevel;
    [SerializeField] private UnityEvent<bool> onButtonGetCoin;
    public void LoadNextMapLevel()
    {
        onButtonLoadNextMapLevel?.Invoke();
    }
    public void GetCoin()
    {
        onButtonGetCoin?.Invoke(true);
    }



}
