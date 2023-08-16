using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeCat
{
    Cat1, Cat2, Cat3, Cat4,
}
public class Cat : MonoBehaviour
{
    private TypeCat typeCat;
    public TypeCat TypeCat { get => typeCat; set => typeCat = value; }


    public void Jump(Vector3 targetPosition)
    {
        //jump cong
        transform.position = targetPosition;
    }    
    public void Match()
    {
        //anim
    }    
}
