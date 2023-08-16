using Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bar : MonoBehaviour
{
    private Stack<Cat> stackCats;
    [SerializeField] private List<Transform> listCatPosition;
    [SerializeField] private Material outline;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private SpriteRenderer imageBar;
    [SerializeField] private Animator animator;
    public SpriteRenderer ImageBar { get => imageBar; set => imageBar = value; }

    private void Awake()
    {
        stackCats = new Stack<Cat>();
        outline.ThrowIfNull();
        defaultMaterial.ThrowIfNull();
        imageBar.ThrowIfNull();
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
    public bool AddCat1(List<Cat> cats)
    {
        if ((stackCats.Count > 0 && cats[0].TypeCat != stackCats.Peek().TypeCat) || stackCats.Count >= 4 || stackCats == null) return false;
        bool isAdd = false;
        for (int i = cats.Count - 1; i >= 0; i--)
        {
            if (stackCats.Count >= 4) break;
            stackCats.Push(cats[i]);
            isAdd = true;
        }
        return isAdd;
    }
    public bool AddCatRollback (List<Cat> cats)
    {
        bool isAdd = false;
        foreach (Cat cat in cats)
        {
            if (stackCats.Count >= 4) break;
            stackCats.Push(cat);
            isAdd = true;
        }
        return isAdd;
    }
    public Cat RemoveCat()
    {
        if (stackCats.Count == 0) return null;
        return stackCats.Pop();
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
        //anim like : hight light when select cat
        OutLineSelect(temp); 
        return temp;  
        
    }

    public void OutLineSelect(List <Cat> cats)
    {
        foreach (Cat cat in cats)
        {
            cat.StartAnimIdle = false;
            cat.ClearAnim();
            cat.GetComponentInChildren<SpriteRenderer>().material = outline;
        }
    }
    public void OffOutLineSelect(List<Cat> cats)
    {
       
        foreach (Cat cat in cats)
        {
            cat.StartAnimIdle = true;
            cat.SetAnimIdle();
            cat.GetComponentInChildren<SpriteRenderer>().material = defaultMaterial;
        }
        
    }
    public Transform GetPositionEmptyInBarLeftAndSetDirectionCat(Cat cat)
    {
        for(int i = 0; i < listCatPosition.Count; i++ )
        {
            if(listCatPosition[i].childCount == 0 )
            {
                Vector3 temp = cat.transform.localScale;
                temp.x *= -1f;
                cat.transform.localScale = temp;
                return listCatPosition[i];
            }
        }
        return null;
    }
    public Transform GetPositionEmptyInBarLeft()
    {
        for (int i = 0; i < listCatPosition.Count; i++)
        {
            if (listCatPosition[i].childCount == 0)
            {
                return listCatPosition[i];
            }
        }
        return null;
    }
    public Transform GetPositionEmptyInBarRight()
    {
        for (int i = listCatPosition.Count - 1; i >= 0; i--)
        {
            if (listCatPosition[i].childCount == 0)
            {
                return listCatPosition[i];
            }
        }
        return null;
    }
    public void AddToStackCat(Cat cat)
    {
        stackCats.Push(cat);
    }
    public void PopStackBar(int quantityCat)
    {
        for(int i = 0; i < quantityCat; i++)
        {
            stackCats.Pop();
        }    
    }
    public int GetLengthStack()
    {
        return stackCats.Count;
    }
    public List<Cat> StackToList()
    {
        return stackCats.ToList();
    }
    public void ClearStack()
    {
        stackCats.Clear();
    }
    public void SetAnimation()
    {
        animator.SetTrigger(Constanst.AnimBar);
    }    
}
