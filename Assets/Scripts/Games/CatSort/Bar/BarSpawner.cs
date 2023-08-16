using DG.Tweening;
using Extensions;
using System.Collections.Generic;
using UnityEngine;

public class BarSpawner : MonoBehaviour
{
    private int barQuantity;
    private int maxCountJump;
    private int mapLevel;
    private int levelMax;
    private float spacingBarY;
    private bool isMovementBarWhenBooster;
    private List<Bar> bars;
    private List<Map> map;
    private Sprite skinBar;
    private AllMap allMap;
    private float worldHeight;
    private float worldWidth;
    [SerializeField] private Bar barPrefabSingle;
    [SerializeField] private CatSpawner catSpawner;
    [SerializeField] private Transform environment;

    public bool IsMovementBarWhenBooster { get => isMovementBarWhenBooster; set => isMovementBarWhenBooster = value; }
    public int MaxCountJump { get => maxCountJump; set => maxCountJump = value; }
    public int BarQuantity { get => barQuantity; private set => barQuantity = value; }

    private void Awake()
    {
        worldHeight = Camera.main.orthographicSize * 2f;
        worldWidth = worldHeight * Screen.width / Screen.height;
        isMovementBarWhenBooster = false;
        bars = new();
        if(mapLevel > 0 && mapLevel <= levelMax)
        {
            barQuantity = map[mapLevel - 1].Bars.Count;
            maxCountJump = map[mapLevel - 1].MaxCountJump;
        }
        barPrefabSingle.ThrowIfNull();
        catSpawner.ThrowIfNull();   
        environment.ThrowIfNull();
    }
    
    public void Initialized(float spacingBarY, int mapLevel, Sprite skinBar, List<Map> map, AllMap allMap, int levelMax)
    {
        this.spacingBarY = spacingBarY;
        this.mapLevel = mapLevel;
        this.skinBar = skinBar;
        this.map = map; 
        this.allMap = allMap;
        this.levelMax = levelMax;
    }

    void Start()
    {
        if( mapLevel <= levelMax)
        {
            BarSpawn();
            MoveBarStartGame();
        }    
        

    }
    public void BarSpawn()
    {
        float positionX = -worldWidth / 4f;
        float positionY = barPrefabSingle.transform.position.y;
        //Set layout 
        if (barQuantity % 2 == 0)
        {
            for (int i = 0; i < barQuantity; i++)
            {
                Bar bar = Instantiate(barPrefabSingle);
                bar.ImageBar.sprite = skinBar;
                if (i % 2 == 0) //bar left
                    bar.tag = Constanst.NameBarLeftTag;
                else //bar right
                {
                    bar.tag = Constanst.NameBarRightTag;

                    Transform tform = bar.ImageBar.transform;
                    //Set Position image bar
                    Vector3 tempPos = tform.position;
                    tempPos.x = -0.5f;
                    tempPos.y = 0.6f;
                    tform.localPosition = tempPos;
                    //reverse image bar
                    Vector3 temp = tform.localScale;
                    temp.x *= -1f;
                    tform.localScale = temp;
                    

                }
                Vector3 newPosition = bar.transform.position;
                newPosition.y = positionY;
                newPosition.x = positionX;
                bar.transform.position = newPosition;
                //update position for next bar
                if (i % 2 == 1)
                {
                    positionY -= spacingBarY;
                    positionX = -worldWidth / 4f;
                }      
                else positionX *= -1.2f;
                bars.Add(bar);
                
                bar.transform.SetParent(environment);
                bar.transform.localScale = Vector3.one;
            }
        }
        else
        {
            for (int i = 0; i < barQuantity; i++)
            {
                Bar bar = Instantiate(barPrefabSingle);
                bar.ImageBar.sprite = skinBar;
                if (i % 2 == 0) //bar left
                {
                    bar.tag = Constanst.NameBarLeftTag;
                }
                else //bar right
                {
                    bar.tag = Constanst.NameBarRightTag;

                    Transform tform = bar.ImageBar.transform;
                    //Set Position image bar
                    Vector3 tempPos = tform.position;
                    tempPos.x = -0.5f;
                    tempPos.y = 0.6f;
                    tform.localPosition = tempPos;
                    //reverse image bar
                    Vector3 temp = tform.localScale;
                    temp.x *= -1f;
                    tform.localScale = temp;
                    
                }
                Vector3 newPosition = bar.transform.position;
                newPosition.y = positionY;
                newPosition.x = positionX;
                bar.transform.position = newPosition;
                //update position for next bar
                positionY -= spacingBarY/2f;
                if (i % 2 == 1)
                {
                    positionX = -worldWidth / 4f;
                }
                else
                {
                    positionX *= -1.2f;
                }
                bars.Add(bar);
                bar.transform.SetParent(environment);
                bar.transform.localScale = Vector3.one;
            }
        }
        AddCatToBar();
    }
    public void AddBarBooster()
    {
        Bar barLast = bars[bars.Count - 1];
        Bar barBeforeLast = bars[bars.Count - 2];
        
        if (barQuantity % 2 == 0)
        {
            //bar left
            Bar bar = Instantiate(barPrefabSingle);
            bar.ImageBar.sprite = skinBar;
            bar.tag = Constanst.NameBarLeftTag;
            //Set position bar new
            Vector3 newPosition = barLast.transform.position;
            newPosition.y -= spacingBarY;
            newPosition.x = barBeforeLast.transform.position.x;
            bar.transform.position = newPosition;
            bars.Add(bar);
            barQuantity++;
            bar.transform.SetParent(environment);
            bar.transform.localScale = Vector3.one;
        }
        else
        {
            //bar right
            Bar bar = Instantiate(barPrefabSingle);
            Transform tform = bar.ImageBar.transform;
            //reverse image bar
            Vector3 temp = tform.localScale;
            temp.x *= -1f;
            tform.localScale = temp;
            //Set Position image bar
            Vector3 tempPos = tform.position;
            tempPos.x = -0.5f;
            tempPos.y = 0.6f;
            tform.localPosition = tempPos;
            bar.ImageBar.sprite = skinBar;
            bar.tag = Constanst.NameBarRightTag;
            //Set position bar new
            Vector3 newPosition = barBeforeLast.transform.position;
            newPosition.y -= spacingBarY;
            bar.transform.position = newPosition;
            bars.Add(bar);
            barQuantity++;
            bar.transform.SetParent(environment);
            bar.transform.localScale = Vector3.one;
        }
    }
    private void Update()
    {
        if (isMovementBarWhenBooster)
        {
            MovementBarBooster();
        }
        
    }
    public void MovementBarBooster()
    {
        if (barQuantity % 2 != 0)
        {
            for(int i = 0;i < barQuantity; i++)
            {
                if(bars[i].CompareTag(Constanst.NameBarLeftTag))
                {
                    bars[i].transform.position += Vector3.up * spacingBarY * Time.deltaTime;
                    if (bars[0].transform.position.y >= bars[1].transform.position.y + spacingBarY/2f) isMovementBarWhenBooster = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < barQuantity; i++)
            {
                if (bars[i].CompareTag(Constanst.NameBarRightTag))
                {
                    bars[i].transform.position += Vector3.up * spacingBarY * Time.deltaTime;
                    if (bars[1].transform.position.y >= bars[0].transform.position.y) isMovementBarWhenBooster = false;
                }
            }
        }

    }
    public void UpdateWhenEquipSkin(Sprite image)
    {
        foreach(var bar in bars)
        {
            bar.ImageBar.sprite = image;
        }    
    }    
    public bool CheckFinishMap()
    {
        foreach (var bar in bars)
        {
            if(bar.GetLengthStack() > 0)
                return false;
        }  
        return true;
    }    
    public void AddCatToBar()
    {
        for (int i = 0; i < barQuantity; i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j < map[mapLevel - 1].Bars[i].Cats.Count; j++)
                {
                    Cat cat = catSpawner.GetCat(map[mapLevel - 1].Bars[i].Cats[j]);
                    Transform parentPos =  bars[i].GetPositionEmptyInBarLeftAndSetDirectionCat(cat);
                    cat.transform.SetParent(parentPos, false);
                    bars[i].AddToStackCat(cat);
                }
            }
            else
            {
                for (int j = map[mapLevel - 1].Bars[i].Cats.Count - 1; j >= 0; j--)
                {
                    Cat cat = catSpawner.GetCat(map[mapLevel - 1].Bars[i].Cats[j]);
                    Transform parentPos = bars[i].GetPositionEmptyInBarRight();
                    cat.transform.SetParent(parentPos, false);
                    bars[i].AddToStackCat(cat);
                }
            }
        }
    }
    public void MoveBarStartGame()
    {
        foreach (var bar in bars)
        {
            if (bar.CompareTag(Constanst.NameBarLeftTag))
            {
                Vector3 tempPos = bar.transform.position;
                Vector3 tempPos1 = tempPos;
                tempPos.x = -worldWidth  ;
                bar.transform.DOMove(tempPos, 1f).OnComplete(() =>
                {
                    bar.transform.DOMove(tempPos1, 1f);
                });
            }
            else if(bar.CompareTag(Constanst.NameBarRightTag))
            {
                Vector3 tempPos = bar.transform.position;
                Vector3 tempPos1 = tempPos;
                tempPos.x = worldWidth;
                bar.transform.DOMove(tempPos, 1f).OnComplete(() =>
                {
                    bar.transform.DOMove(tempPos1, 1f);
                });
            }    
            
        }    
    }    

    //Code Auto config map
    //public void CreateMap()
    //{
    //    for (int i = 0; i < allMap.AllMaps.Count; i++)
    //    {
    //        int numberMapLevel = i;
    //        this.ConfigMap(allMap.AllMaps[i].QuantityCatType, allMap.AllMaps[i].CountJump, allMap.AllMaps[i].QuantityBar, numberMapLevel + 1);
    //    }
    //}
    //public void ConfigMap(int quantityCatType, int countJump, int quantityBar, int indexMap)
    //{
    //    List<CatConfig> cats = catSpawner.GetCat(quantityCatType);
    //    Map mapTemp = ScriptableObject.CreateInstance<Map>();
    //    //Map mapTemp = new Map();
    //    int index = 0;
    //    for (int i = 0; i < quantityBar; i++)
    //    {
    //        BarConfig barConfig = new BarConfig();
    //        if (index < quantityCatType * 4)
    //        {
    //            for (int j = 0; j < 4; j++)
    //            {
    //                barConfig.Cats.Add(cats[index]);
    //                index++;
    //            }
    //        }
    //        mapTemp.Bars.Add(barConfig);
    //    }
    //    int randomIndexStart = -1;
    //    int randomIndexTarget = -1;
    //    for (int i = 0; i < countJump; i++)
    //    {
    //        RandomizeObjectPositions(quantityBar, mapTemp.Bars, ref randomIndexStart, ref randomIndexTarget);
    //    }


    //    Map mapConfig = ScriptableObject.CreateInstance<Map>();

    //    mapConfig.Bars = mapTemp.Bars;
    //    string filePath = "Assets/Scripts/Games/CatSort/Map/MapLevel/MapLevelTest" + indexMap + ".asset";
    //    AssetDatabase.CreateAsset(mapConfig, filePath);
    //    AssetDatabase.SaveAssets();
    //}
    //private void RandomizeObjectPositions(int quantityBar, List<BarConfig> bars, ref int randomIndexStart, ref int randomIndexTarget)
    //{
    //    int randomIndex1;
    //    int randomIndex2;
    //    do
    //    {
    //        randomIndex1 = GetRandomIndexStart(bars, quantityBar);
    //        randomIndex2 = GetRandomIndexTarget(bars, quantityBar, randomIndex1);
    //    }
    //    while (randomIndexTarget == randomIndex1 && randomIndexStart == randomIndex2);
    //    if (randomIndex1 % 2 == 0)
    //    {
    //        bars[randomIndex2].Cats.Add(bars[randomIndex1].Cats[bars[randomIndex1].Cats.Count - 1]);
    //        bars[randomIndex1].Cats.Remove(bars[randomIndex1].Cats[bars[randomIndex1].Cats.Count - 1]);
    //    }
    //    else
    //    {
    //        bars[randomIndex2].Cats.Add(bars[randomIndex1].Cats[0]);
    //        bars[randomIndex1].Cats.Remove(bars[randomIndex1].Cats[0]);
    //    }
    //    randomIndexStart = randomIndex1;
    //    Debug.Log(randomIndexStart);
    //    randomIndexTarget = randomIndex2;
    //}
    //private int GetRandomIndexStart(List<BarConfig> bars, int quantityBar)
    //{
    //    int randomIndex1;
    //    do
    //    {
    //        randomIndex1 = UnityEngine.Random.Range(0, quantityBar);
    //    } while (bars[randomIndex1].Cats.Count == 0);
    //    int randomIndex1Temp = randomIndex1;
    //    for (int i = 0; i < bars.Count; i++)
    //    {
    //        if (bars[i].Cats.Count > 0)
    //        {
    //            int count = 0;
    //            CatConfig cat = bars[i].Cats[0];
    //            for (int j = 1; j < bars[i].Cats.Count; j++)
    //            {
    //                if (cat.CatType == bars[i].Cats[j].CatType)
    //                    count++;
    //            }
    //            if (count == 3)
    //            {
    //                randomIndex1Temp = i;
    //                break;
    //            }
    //        }

    //    }
    //    if (randomIndex1 != randomIndex1Temp)
    //    {
    //        randomIndex1 = randomIndex1Temp;
    //    }
    //    else
    //    {
    //        for (int i = 0; i < bars.Count; i++)
    //        {
    //            if (i % 2 == 0)
    //            {
    //                if (bars[i].Cats.Count > 0)
    //                {
    //                    int count = 0;
    //                    CatConfig cat = bars[i].Cats[bars[i].Cats.Count - 1];
    //                    for (int j = bars[i].Cats.Count - 2; j >= 0; j--)
    //                    {
    //                        if (cat.CatType == bars[i].Cats[j].CatType)
    //                            count++;
    //                    }
    //                    if (count == 2)
    //                    {
    //                        randomIndex1 = i;
    //                        break;
    //                    }
    //                }

    //                if (bars[i].Cats.Count > 0)
    //                {
    //                    int count = 0;
    //                    CatConfig cat = bars[i].Cats[bars[i].Cats.Count - 1];
    //                    for (int j = bars[i].Cats.Count - 2; j >= 0; j--)
    //                    {
    //                        if (cat.CatType == bars[i].Cats[j].CatType)
    //                            count++;
    //                    }
    //                    if (count == 1)
    //                    {
    //                        randomIndex1 = i;
    //                        break;
    //                    }
    //                }

    //            }
    //            else
    //            {
    //                if (bars[i].Cats.Count > 0)
    //                {
    //                    int count = 0;
    //                    CatConfig cat = bars[i].Cats[0];
    //                    for (int j = 1; j < bars[i].Cats.Count; j++)
    //                    {
    //                        if (cat.CatType == bars[i].Cats[j].CatType)
    //                            count++;
    //                    }
    //                    if (count == 2)
    //                    {
    //                        randomIndex1 = i;
    //                        break;
    //                    }
    //                }
    //                if (bars[i].Cats.Count > 0)
    //                {
    //                    int count = 0;
    //                    CatConfig cat = bars[i].Cats[bars[i].Cats.Count - 1];
    //                    for (int j = bars[i].Cats.Count - 2; j >= 0; j--)
    //                    {
    //                        if (cat.CatType == bars[i].Cats[j].CatType)
    //                            count++;
    //                    }
    //                    if (count == 1)
    //                    {
    //                        randomIndex1 = i;

    //                        break;
    //                    }
    //                }

    //            }


    //        }
    //    }
    //    return randomIndex1;
    //}

    //private int GetRandomIndexTarget(List<BarConfig> bars, int quantityBar, int randomIndex1)
    //{
    //    int randomIndex;
    //    do
    //    {
    //        randomIndex = UnityEngine.Random.Range(0, quantityBar);
    //    } while (randomIndex == randomIndex1 || bars[randomIndex].Cats.Count == 4);

    //    return randomIndex;
    //}

}
