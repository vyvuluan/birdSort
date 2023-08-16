using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    #region BAR
    [Header("Bar")]
    [Space(8.0f)]
    [Tooltip("Bar Quantity")]
    [SerializeField] private int barQuantity = 5;
    [Tooltip("Spacing bar y ")]
    [SerializeField]  private float spacingBarY = 1.5f;
    #endregion

    public int BarQuantity { get => barQuantity; set => barQuantity = value; }
    public float SpacingBarY { get => spacingBarY; set => spacingBarY = value; }
}
