using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bar : MonoBehaviour
{
    private Stack<Cat> stackCats;
    [SerializeField] private List<Transform> listCatPosition;
    private void Awake()
    {
        stackCats = new();
    }
    public bool AddCat(List<Cat> cats)
    {
        if ((stackCats.Count > 0 && cats[0].TypeCat != stackCats.Peek().TypeCat) || stackCats.Count >= 4 || stackCats == null) return false;
        bool isAdd = false;
        foreach(Cat cat in cats)
        {
            if (stackCats.Count >= 4) break;
            stackCats.Push(cat);
            isAdd = true;
        }
        return isAdd;
    }
    public void RemoveCat()
    {
        if (stackCats.Count == 0) return;
        //TypeCat typeCat = stackCats.Peek().TypeCat;
        //while(stackCats.Count > 0)
        //{
        //    if (typeCat == stackCats.Peek().TypeCat)
        //    {
        //        stackCats.Pop();
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}
        stackCats.Pop();
    }
    public int getLength()
    {
        return stackCats.Count;
    }
    public bool MatchCat()
    {
        if (stackCats.Count < 4) return false;
        TypeCat typeCat = stackCats.Peek().TypeCat;
        List<Cat> temp = stackCats.ToList();
        for(int i = temp.Count - 1; i >= 0; i-- )
        {
            if(temp[i].TypeCat != typeCat)
            {
                return false;
            }
        }
        return true;
    }
    public List<Cat> SelectCat()
    {
        if (stackCats.Count == 0) return null;
        TypeCat typeCat = stackCats.Peek().TypeCat;
        List<Cat> catsStackToList = stackCats.ToList();
        List<Cat> temp = new List<Cat>();
        for (int i = 0; i < catsStackToList.Count; i++)
        {
            if (catsStackToList[i].TypeCat == typeCat)
            {
                temp.Add(catsStackToList[i]);
            }
            else
            {
                break;
            }
        }
        //anim like : hight when select cat
        return temp;
    }
    public Transform SetCatPositionInBarLeft(Cat cat)
    {
        for(int i = 0; i < listCatPosition.Count; i++ )
        {
            if(listCatPosition[i].childCount == 0 )
            {
                cat.transform.SetParent(listCatPosition[i], false);
                return listCatPosition[i];
            }
        }
        return null;
    }
    public Transform SetCatPositionInBarRight(Cat cat)
    {
        for (int i = listCatPosition.Count - 1; i >= 0; i--)
        {
            if (listCatPosition[i].childCount == 0)
            {
                cat.transform.SetParent(listCatPosition[i],false);
                return listCatPosition[i];
            }
        }
        return null;
    }
    public void AddToStackCat(Cat cat)
    {
        stackCats.Push(cat);
    }
}
