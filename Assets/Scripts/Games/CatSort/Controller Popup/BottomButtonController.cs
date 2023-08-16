using UnityEngine;
using UnityEngine.Events;

public class BottomButtonController : MonoBehaviour
{
    [SerializeField] private UnityEvent<BoosterType> onButtonAddBar;
    [SerializeField] private UnityEvent<BoosterType> onButtonRollback;
    [SerializeField] private UnityEvent<BoosterType> onButtonNextMap;
    [SerializeField] private UnityEvent<bool> onButtonPenalShop;
    public void AddBar()
    {
        onButtonAddBar?.Invoke(BoosterType.Add);
    }
    public void Rollback()
    {
        onButtonRollback?.Invoke(BoosterType.Rollback);
    }
    public void NextMap()
    {
        onButtonNextMap?.Invoke(BoosterType.NextMap);
    }
    public void OnPenalShop()
    {
        onButtonPenalShop?.Invoke(true);
    }
}
