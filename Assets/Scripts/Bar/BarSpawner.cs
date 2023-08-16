using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarSpawner : MonoBehaviour
{
    private int barQuantity;
    [SerializeField] private Bar barPrefabSingle;
    private List<Bar> bars;
    private float spacingBarY;
    [SerializeField] CatSpawner catSpawner;

    private const string tagNameBarLeft = "bar left";
    private const string tagNameBarRight = "bar right";
    private void Awake()
    {
        bars = new();
    }
    public void Initialized(int barQuantity, float spacingBarY)
    {
        this.barQuantity = barQuantity;
        this.spacingBarY = spacingBarY;
    }
    void Start()
    {
        BarSpawn();
    }
    public void BarSpawn()
    {
        float positionX = barPrefabSingle.transform.position.x;
        float positionY = barPrefabSingle.transform.position.y;
        if (barQuantity % 2 == 0)
        {
            for (int i = 0; i < barQuantity; i++)
            {
                Bar bar = Instantiate(barPrefabSingle);
                if (i % 2 == 0)
                    bar.tag = tagNameBarLeft;
                else bar.tag = tagNameBarRight;
                Vector3 newPosition = bar.transform.position;
                newPosition.y = positionY;
                newPosition.x = positionX;
                bar.transform.position = newPosition;
                if (i % 2 == 1)
                    positionY -= spacingBarY;
                positionX *= -1f;
                bars.Add(bar);
            }
        }
        else
        {
            for (int i = 0; i < barQuantity; i++)
            {
                Bar bar = Instantiate(barPrefabSingle);
                if (i % 2 == 0)
                    bar.tag = tagNameBarLeft;
                else bar.tag = tagNameBarRight;
                Vector3 newPosition = bar.transform.position;
                newPosition.y = positionY;
                newPosition.x = positionX;
                bar.transform.position = newPosition;
                positionY -= spacingBarY;
                positionX *= -1f;
                bars.Add(bar);
            }
        }
        AddCatToBar();
    }
    public void AddCatToBar()
    {
        List<Cat> cats = catSpawner.GetCat(barQuantity - 2);
        int index = 0;
        for (int i = 0; i < barQuantity - 2; i++)
        {
            if(bars[i].gameObject.transform.CompareTag(tagNameBarLeft))
            {
                for (int j = 0; j < 4; j++)
                {
                    bars[i].SetCatPositionInBarLeft(cats[index]);
                    bars[i].AddToStackCat(cats[index]);
                    index++;
                }
            }
            else if(bars[i].gameObject.transform.CompareTag(tagNameBarRight))
            {
                for (int j = 0; j < 4; j++)
                {
                    bars[i].SetCatPositionInBarRight(cats[index]);
                    bars[i].AddToStackCat(cats[index]);
                    index++;
                }
            }
        }
    }
}
