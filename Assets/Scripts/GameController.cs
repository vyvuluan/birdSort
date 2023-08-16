using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController : MonoBehaviour
{

    [Header("MVC")]
    [SerializeField] private GameModel model;
    [SerializeField] private GameView view;
    [SerializeField] private GameAudio gameAudio;

    [Header("Preference")]
    [SerializeField] private CatSpawner catSpawner;
    [SerializeField] private BarSpawner barSpawner;

    private List<Cat> catSelected;
    private Bar barSelected;
    private Bar barSelectedTarget;

    private const string tagNameBarLeft = "bar left";
    private const string tagNameBarRight = "bar right";
    private const string tagNameCase = "case";

    private void Awake()
    {
        catSelected = new();
        barSpawner.Initialized(model.BarQuantity, model.SpacingBarY);
    }

    private void Update()
    {
        InputControl();
    }
    public void ClickCatSelect(GameObject gObject)
    {
        if(gObject.CompareTag(tagNameCase))
        {
            barSelected = gObject.transform.parent.gameObject.GetComponent<Bar>();
            catSelected = barSelected.SelectCat();
        }
    }
    public void InputControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (barSelected == null)
            {
                if (hit.collider != null )
                {
                    ClickCatSelect(hit.collider.gameObject);
                }
            }
            else
            {
                if (hit.collider != null && hit.collider.gameObject.transform.parent.gameObject.GetComponent<Bar>() != barSelected)
                {
                    barSelectedTarget = hit.collider.gameObject.transform.parent.gameObject.GetComponent<Bar>();
                    if(catSelected.Count > 0)
                    {
                        if (barSelectedTarget.AddCat(catSelected))
                        {
                            GameObject tempGameObject = hit.collider.gameObject;
                            foreach (Cat cat in catSelected)
                            {
                                Transform getPosition = null;
                                if (tempGameObject.transform.parent.CompareTag(tagNameBarLeft))
                                {
                                    getPosition = barSelectedTarget.SetCatPositionInBarLeft(cat);
                                }
                                else if (tempGameObject.transform.parent.CompareTag(tagNameBarRight))
                                {
                                    getPosition = barSelectedTarget.SetCatPositionInBarRight(cat);
                                }
                                if (getPosition != null)
                                {
                                    cat.Jump(getPosition.position);
                                    barSelected.RemoveCat();
                                }
                            }
                            this.barSelected = null;
                            //Match Cat
                            if (barSelectedTarget.MatchCat()) Debug.Log("ok");
                        }
                        else
                        {
                            ClickCatSelect(hit.collider.gameObject);
                        }
                    }
                }


            }

        }
    }





}
