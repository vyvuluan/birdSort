using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BarConfig
{
    [SerializeField] private List<CatConfig> cats;

    public List<CatConfig> Cats { get => cats; set => cats = value; }
    public BarConfig()
    {
        cats = new List<CatConfig>();
    }
    public BarConfig(List<CatConfig> cats)
    {
        this.cats = cats;
    }
}
[System.Serializable]
public class CatConfig
{
    [SerializeField] private TypeCat catType;
    [SerializeField] private CatSpecial catSpecial;

    public TypeCat CatType { get => catType; set => catType = value; }
    public CatSpecial CatSpecial { get => catSpecial; set => catSpecial = value; }
}

[CreateAssetMenu(fileName = "MapLevel", menuName = "ScriptableObjects/Map")]
public class Map : ScriptableObject
{
    [SerializeField] private List<BarConfig> bars;
    [SerializeField] private int maxCountJump = -1;
    public List<BarConfig> Bars { get => bars; set => bars = value; }
    public int MaxCountJump { get => maxCountJump; set => maxCountJump = value; }
    public Map()
    {
        this.bars = new List<BarConfig>();
       
    }    
}
