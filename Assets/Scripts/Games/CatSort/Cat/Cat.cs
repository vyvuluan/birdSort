using DG.Tweening;
using Extensions;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TypeCat
{
    Cat1, Cat2, Cat3, Cat4, Cat5, Cat6, Cat7, Cat8
}
public enum CatSpecial
{
    None, Boom, Lock, Key, Sleep, Alarm
}
public enum CatColor
{
    Red, Turquoise, TurquoiseBlack, Pink, Pink1, Yellow, Purple, White, Organ, Green, Blue, BlueOrgan
}
public class Cat : MonoBehaviour
{
    private TypeCat typeCat;
    private CatSpecial catSpecial;
    private bool isJump;
    private bool isMatchScore;
    private bool startAnimIdle;
    private float timeJump;
    private float timeCoinMove;
    private float heightCatJump;
    private float heightFishJump;
    private GameObject fishScore;
    private List<string> animationList;
    private Vector3 startPosition;
    private Transform targetTransform;
    private Action increaseOneScore;
    private Action onSetAnimBar;
    private int catCountJump;
    [SerializeField] private GameObject boom;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private CatColor catColor;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private GameObject featherBird;
    public TypeCat TypeCat { get => typeCat; set => typeCat = value; }
    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }
    public bool IsMatchScore { get => isMatchScore; set => isMatchScore = value; }
    public CatSpecial CatSpecial { get => catSpecial; set => catSpecial = value; }
    public Transform TargetTransform { get => targetTransform; set => targetTransform = value; }
    public GameObject Boom { get => boom; set => boom = value; }
    public bool StartAnimIdle { get => startAnimIdle; set => startAnimIdle = value; }
    public MeshRenderer MeshRenderer { get => meshRenderer; set => meshRenderer = value; }
    public Action OnSetAnimBar { get => onSetAnimBar; set => onSetAnimBar = value; }

    public void Initialized(float timeJump, float heightCatJump, float heightFishJump, GameObject fishScore, float timeCoinMove)
    {
        this.timeJump = timeJump;
        this.heightCatJump = heightCatJump;
        this.heightFishJump = heightFishJump;
        this.fishScore = fishScore;
        this.timeCoinMove = timeCoinMove;
    }
    public void InitializedActionSetScore(Action increaseOneScore)
    {
        this.increaseOneScore = increaseOneScore;
    }
    private void Awake()
    {
        animationList = new List<string>
        {
            Constanst.AnimIdle1,
            Constanst.AnimIdle2,
            Constanst.AnimIdle3,
            Constanst.AnimIdle4,
            Constanst.AnimIdle5,
            Constanst.AnimIdle6,
            Constanst.AnimIdle7,
            Constanst.AnimIdle8,
            Constanst.AnimIdle9,
            Constanst.AnimIdle11
        };
        switch (catColor)
        {
            case CatColor.White:
                animationList.Add(Constanst.AnimIdle1BirdWhite);
                break;
            case CatColor.Organ:
                animationList.Add(Constanst.AnimIdle1BirdOrgan);
                break;
            case CatColor.BlueOrgan:
                animationList.Add(Constanst.AnimIdle1BirdBlueOrgan);
                break;
        }
        startAnimIdle = true;
        isJump = false;
        isMatchScore = false;
        //InitColoFeather();
        meshRenderer.ThrowIfNull();
        skeletonAnimation.ThrowIfNull();
    }
    private void Update()
    {
        if (isJump)
        {
            StartCoroutine(MoveToTarget(timeJump));
            isJump = false;
        }
        if (isMatchScore)
        {
            StartCoroutine(MoveCoinWhenMatchScore(timeCoinMove));
            isMatchScore = false;
        }


    }
    private void Start()
    {
        //init anim idle bird 
        this.SetAnimIdle();
    }

    private void OnAnimationComplete(TrackEntry trackEntry)
    {
        if (startAnimIdle)
        {
            int randomIndex = UnityEngine.Random.Range(0, animationList.Count);
            string randomAnimation = animationList[randomIndex];
            skeletonAnimation.AnimationState.SetAnimation(0, randomAnimation, false);
        }

    }
    public void Jump(Transform targetPos, int catCountSelected)
    {
        this.TargetTransform = targetPos;
        this.catCountJump = catCountSelected;
        transform.SetParent(targetPos);
        this.isJump = true;
    }
    public void Match()
    {
        //gameObject.transform.SetParent(null);
        gameObject.SetActive(false);
        this.increaseOneScore.Invoke();
    }
    private IEnumerator MoveCoinWhenMatchScore(float jumpTime)
    {   
        Vector3 targetPositionScore = fishScore.transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < jumpTime)
        {
            float t = elapsedTime / jumpTime;
            float height = Mathf.Sin(t * Mathf.PI) * heightFishJump;
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPositionScore, t) + Vector3.up * height;
            transform.position = currentPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPositionScore;
        this.Match();
    }   
    private IEnumerator MoveToTarget(float jumpTime)
    {
        meshRenderer.sortingOrder = 50;
        Vector3 targetPositionScore = targetTransform.position;
        float elapsedTime = 0f;
        
        StartAnimIdle = false;
        skeletonAnimation.AnimationState.SetAnimation(0, Constanst.AnimFly, false);
        //wait anim fly finish
        yield return new WaitForSeconds(0.333f);
        IncreaseTimeScaleAnimTransform(1.4f);
        skeletonAnimation.AnimationState.SetAnimation(0, Constanst.AnimFlyIdle, true);

        bool isRe = false;
        while (elapsedTime < jumpTime)
        {
            //move in the same bar left
            if (!isRe && startPosition.x < 0 && targetPositionScore.x < 0) 
            {
                if (startPosition.x > targetPositionScore.x || catCountJump > 1)
                {
                    Vector3 temp = transform.localScale;
                    temp.x *= -1f;
                    transform.localScale = temp;
                    isRe = true;


                }
            }
            //move in the same bar right
            else if (!isRe && startPosition.x > 0 && targetPositionScore.x > 0)
            {
                if (startPosition.x < targetPositionScore.x || catCountJump > 1)
                {
                    Vector3 temp = transform.localScale;
                    temp.x *= -1f;
                    transform.localScale = temp;
                    isRe = true;

                }
            }    
            float t = elapsedTime / jumpTime;
            float height = Mathf.Sin(t * Mathf.PI) * heightCatJump;
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPositionScore, t) + Vector3.up * height;
            transform.position = currentPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //change direction cat
        if (isRe || startPosition.x < 0 && targetPositionScore.x > 0 || startPosition.x > 0 && targetPositionScore.x < 0)
        {
            Vector3 temp1 = transform.localScale;
            temp1.x *= -1f;
            transform.localScale = temp1;


        }    
        skeletonAnimation.AnimationState.SetAnimation(0, Constanst.AnimFlyLanding, false);
        yield return new WaitForSeconds(0.01f);
        transform.position = targetPositionScore;
        //anim bar
        if (onSetAnimBar != null)
        {
            onSetAnimBar.Invoke();
            onSetAnimBar = null;
        }
        meshRenderer.sortingOrder = 20;
        
        if (transform.localScale.x == -0.8f)
        {
            featherBird.transform.localScale = new Vector3(-1f,1f,1f);
        }
        else
        {
            featherBird.transform.localScale = Vector3.one;
        }    
        featherBird.SetActive(true);
        yield return new WaitForSeconds(1f);
        featherBird.SetActive(false);
        //yield return new WaitForSeconds(0.2f);
        IncreaseTimeScaleAnimTransform(0.8f);
        startAnimIdle = true;
        this.SetAnimIdle();
    }
    public void TransfromToCoin()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, Constanst.AnimTransform, false);
    }      
    public void ClearAnim()
    {
        skeletonAnimation.AnimationState.ClearTracks();
        skeletonAnimation.skeleton.SetToSetupPose();
        skeletonAnimation.Update(0f);
    } 
    public void SetAnimIdle()
    {
        int randomIndex = UnityEngine.Random.Range(0, animationList.Count);
        string randomAnimation = animationList[randomIndex];
        skeletonAnimation.AnimationState.SetAnimation(0, randomAnimation, false);
        skeletonAnimation.AnimationState.Complete += OnAnimationComplete;
    } 
    public void StartAnimIdle10()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, Constanst.AnimIdle10, false);
        
    }     
    public void IncreaseTimeScaleAnimTransform(float scale)
    {
        skeletonAnimation.timeScale = scale;
    }

}
