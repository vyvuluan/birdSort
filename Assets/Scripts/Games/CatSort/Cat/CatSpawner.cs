using System;
using System.Collections.Generic;
using UnityEngine;
public class CatSpawner : MonoBehaviour
{
    private int quantityCatIn1CatType;
    private float timeJump;
    private float timeCoinMove;
    private float heightCatJump;
    private float heightFishJump;
    private GameObject fishScore;
    private List<int> skinUsing;
    private List<SkinItem> skinItems;
    private List<Cat> catPool;
    private void Awake()
    {
       // quantityCatIn1CatType = 4;
        catPool = new List<Cat>();
    }
    public void Initialized(float timeJump, float heightCatJump, float heightFishJump, GameObject fishScore, int quantityCatIn1CatType, List<int> skinUsing, List<SkinItem> skinItems, float timeCoinMove)
    {
        this.timeJump = timeJump;
        this.heightCatJump = heightCatJump;
        this.heightFishJump = heightFishJump;
        this.fishScore = fishScore;
        this.quantityCatIn1CatType = quantityCatIn1CatType;
        this.skinUsing = skinUsing;
        this.skinItems = skinItems;
        this.timeCoinMove = timeCoinMove;
    }
    private void Start()
    {
        SetSkinToCat();
    }
    public void SetSkinToCat()
    {
        int catType = 0;
        int temp = 0;
        foreach(var item in skinItems)
        {
            if (skinUsing.Contains(item.Id))
            {
                for (int i = 0; i < quantityCatIn1CatType; i++)
                {
                    Cat cat = Instantiate(item.PrefabBird).GetComponent<Cat>();
                    cat.TypeCat  = (TypeCat) catType;
                    cat.CatSpecial = CatSpecial.None;
                    cat.gameObject.SetActive(false);
                    catPool.Add(cat);
                }
                catType++;
                temp++;
            }    
        }    
        
    }
    //use function for ConfigMap
    public List<CatConfig> GetCat(int catTypeCount)
    {
        List<CatConfig> temp = new();
        for (int i = 0; i < catTypeCount * quantityCatIn1CatType; i++)
        {
            CatConfig catConfig = new CatConfig();
            catConfig.CatSpecial = catPool[i].CatSpecial;
            catConfig.CatType = catPool[i].TypeCat;
            temp.Add(catConfig);
        }
        return temp;

    }
    public Cat GetCat(CatConfig catConfig)
    {
        for (int i = 0; i < catPool.Count; i++)
        {
            if (!catPool[i].gameObject.activeInHierarchy && catPool[i].TypeCat == catConfig.CatType)
            {
                Cat cat = catPool[i];
                cat.Initialized(timeJump, heightCatJump, heightFishJump, fishScore, timeCoinMove);
                //Cat Special
                if(catConfig.CatSpecial == CatSpecial.Boom)
                {
                    cat.CatSpecial = CatSpecial.Boom;
                    cat.Boom.gameObject.SetActive(true);
                }    
                cat.gameObject.SetActive(true);
                return cat;
            }
        }
        return null;

    }

    
}
