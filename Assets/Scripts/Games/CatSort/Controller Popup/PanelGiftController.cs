using UnityEngine;
using UnityEngine.Events;

public class PanelGiftController : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> onButtonClosePenalGift;
    public void ClosePenalGift()
    {
        onButtonClosePenalGift?.Invoke(false);
    }
}
