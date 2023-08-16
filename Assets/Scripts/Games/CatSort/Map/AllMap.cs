using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllMapConfig
{
    [SerializeField] private int quantityCatType;
    [SerializeField] private int countJump;
    [SerializeField] private int quantityBar;

    public int QuantityCatType { get => quantityCatType; set => quantityCatType = value; }
    public int CountJump { get => countJump; set => countJump = value; }
    public int QuantityBar { get => quantityBar; set => quantityBar = value; }
}

[CreateAssetMenu(fileName = "MapAllLevel", menuName = "ScriptableObjects/AllMap")]
public class AllMap : ScriptableObject
{
    [SerializeField] private List<AllMapConfig> allMaps;

    public List<AllMapConfig> AllMaps { get => allMaps; set => allMaps = value; }
}
