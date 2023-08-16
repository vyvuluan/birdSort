using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CatTypeList
{
    [SerializeField] private TypeCat typeCat;
    [SerializeField] private Cat cat;

    public TypeCat TypeCat { get => typeCat; set => typeCat = value; }
    public Cat Cat { get => cat; set => cat = value; }
}

public class CatSpawner : MonoBehaviour
{
    private List<Cat> catPool;
    private int poolCatSize;
    [SerializeField] private List<CatTypeList> cats;

    private void Awake()
    {
        poolCatSize = 4;
        catPool = new List<Cat>();

        foreach (CatTypeList catTypeList in cats)
        {
            for (int i = 0; i < poolCatSize; i++)
            {
                Cat cat = Instantiate(catTypeList.Cat);
                cat.TypeCat = catTypeList.TypeCat;
                cat.gameObject.SetActive(false);
                catPool.Add(cat);

            }

        }
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //float worldHeight = Camera.main.orthographicSize * 2f;
        //float worldWidth = worldHeight * Screen.width / Screen.height;
        //foreach (GameObject bullet in bulletPool)
        //{
        //    if (bullet.activeInHierarchy && bullet.transform.position.x > worldWidth / 2)
        //    {
        //        bullet.SetActive(false);
        //    }
        //}
    }
    public void GetBullet()
    {
        foreach (Cat cat in catPool)
        {
            if (!cat.gameObject.activeInHierarchy)
            {
                //cat.transform.position = currentPosition;
                cat.gameObject.SetActive(true);
                break;
            }

        }

    }
    public List<Cat> GetCat(int catTypeCount)
    {
        
        List<Cat> temp = new();
        for (int i = 0; i < catTypeCount*4; i++)
        {
            if (!catPool[i].gameObject.activeInHierarchy)
            {
                //cat.transform.position = currentPosition;
                catPool[i].gameObject.SetActive(true);
                temp.Add(catPool[i]);
            }

        }
        RandomizeObjectPositions(temp);
        return temp;

    }
    private void RandomizeObjectPositions(List<Cat> cats)
    {
        int count = cats.Count;
        for (int i = 0; i < count - 1; i++)
        {
            int randomIndex = Random.Range(i, count);
            Cat temp = cats[i];
            cats[i] = cats[randomIndex];
            cats[randomIndex] = temp;
        }
    }
}
