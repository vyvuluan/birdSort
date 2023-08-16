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
    [SerializeField]  private float spacingBarY = 1.2f;
    #endregion

    #region CAT
    [Header("Bar")]
    [Space(8.0f)]
    [Tooltip("Time jump")]
    [SerializeField] private float timeJump = 0.6f;
    [Tooltip("Time coin move")]
    [SerializeField] private float timeCoinMove = 0.5f;
    [Tooltip("height cat jump")]
    [SerializeField] private float heightCatJump = 1f;
    [Tooltip("height fish jump")]
    [SerializeField] private float heightFishJump = 0.1f;
    [Tooltip("quantity cat type in 1 bar")]
    [SerializeField] private int quantityCatIn1CatType = 4;
    #endregion


    #region BOOSTER
    [Header("BOOSTER")]
    [Space(8.0f)]
    [Tooltip("price to buy 1 booster add bar")]
    [SerializeField] private int priceOneAddBarBooster = 100;
    [Tooltip("price to buy 1 booster next map")]
    [SerializeField] private int priceOneNextMapBooster = 100;
    [Tooltip("price to buy 1 booster rollback")]
    [SerializeField] private int priceOneRollbackBooster = 50;
    [Tooltip("price to buy 3 booster add bar")]
    [SerializeField] private int priceThreeAddBarBooster = 250;
    [Tooltip("price to buy 3 booster next map")]
    [SerializeField] private int priceThreeNextMapBooster = 250;
    [Tooltip("price to buy 3 booster rollback")]
    [SerializeField] private int priceThreeRollbackBooster = 120;
    #endregion
    #region SKIN
    [Header("SKIN")]
    [Space(8.0f)]
    [Tooltip("list skin")]
    [SerializeField] private List<SkinItem> skinItems;
    #endregion
    #region MAP
    [Header("MAP")]
    [Space(8.0f)]
    [Tooltip("Parameters of map level")]
    [SerializeField] private AllMap allMap;
    [Tooltip("list map")]
    [SerializeField] private List<Map> map;
    #endregion



    public int BarQuantity { get => barQuantity; private set => barQuantity = value; }
    public float SpacingBarY { get => spacingBarY; private set => spacingBarY = value; }

    public float TimeJump { get => timeJump; private set => timeJump = value; }
    public float HeightCatJump { get => heightCatJump; private set => heightCatJump = value; }
    public float HeightFishJump { get => heightFishJump; private set => heightFishJump = value; }
    public int QuantityCatIn1CatType { get => quantityCatIn1CatType; private set => quantityCatIn1CatType = value; }
    public int PriceOneAddBarBooster { get => priceOneAddBarBooster; private set => priceOneAddBarBooster = value; }
    public int PriceOneNextMapBooster { get => priceOneNextMapBooster; private set => priceOneNextMapBooster = value; }
    public int PriceOneRollbackBooster { get => priceOneRollbackBooster; private set => priceOneRollbackBooster = value; }
    public int PriceThreeAddBarBooster { get => priceThreeAddBarBooster; private set => priceThreeAddBarBooster = value; }
    public int PriceThreeNextMapBooster { get => priceThreeNextMapBooster; private set => priceThreeNextMapBooster = value; }
    public int PriceThreeRollbackBooster { get => priceThreeRollbackBooster; private set => priceThreeRollbackBooster = value; }
    public List<SkinItem> SkinItems { get => skinItems; private set => skinItems = value; }
    public List<Map> Map { get => map; private set => map = value; }
    public AllMap AllMap { get => allMap; private set => allMap = value; }
    public float TimeCoinMove { get => timeCoinMove; set => timeCoinMove = value; }
}
