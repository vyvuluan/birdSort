using DG.Tweening;
using Extensions;
using Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Rollback
{
    private Bar barSelected;
    private Bar barSelectedTarget;
    private List<Cat> cats;

    public Bar BarSelected { get => barSelected; set => barSelected = value; }
    public Bar BarSelectedTarget { get => barSelectedTarget; set => barSelectedTarget = value; }
    public List<Cat> Cats { get => cats; set => cats = value; }
}
public class GameController : MonoBehaviour
{

    [Header("MVC")]
    [SerializeField] private GameModel model;
    [SerializeField] private GameView view;

    [Header("Preference")]
    [SerializeField] private CatSpawner catSpawner;
    [SerializeField] private BarSpawner barSpawner;
    [SerializeField] private PanelShop panelShop;
    private bool isPause;
    private Bar barSelected;
    private Bar barSelectedTarget;
    private Stack<Rollback> stackRollback;
    private List<SkinItem> skins;
    private List<Cat> catSelected;
    private bool isCheckInput;
    private bool isCheckRollback;
    private bool isMatching;
    private GameServices gameServices;
    private PlayerService playerService;
    private AdsService adsService;
    private DisplayService displayService;
    private IAPService iapService;
    private AudioService audioService;
    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag(Constanst.ServicesTag) == null)
        {
            SceneManager.LoadScene(Constanst.EntryScene);
        }
        else
        {
            GameObject gameServiecObject = GameObject.FindGameObjectWithTag(Constanst.ServicesTag);
            gameServices = gameServiecObject.GetComponent<GameServices>();
        }

        playerService = gameServices.GetService<PlayerService>();
        adsService = gameServices.GetService<AdsService>();
        displayService = gameServices.GetService<DisplayService>();
        iapService = gameServices.GetService<IAPService>();
        audioService = gameServices.GetService<AudioService>();
        InitSkinItem();
        if (playerService.GetStartGameFirstTime() == 0)
        {
            playerService.SetAds(0);
            playerService.SetLevel(1);
            playerService.SetScore(1000);
            playerService.SetQuantityBoosterAddBar(14);
            playerService.SetQuantityBoosterNextMap(4);
            playerService.SetQuantityBoosterRollback(4);
            playerService.SetStartGameFirstTime(1);

        }
        int level = playerService.GetLevel();
        int score = playerService.GetScore();
        int skinBG = playerService.GetSkinBG();
        int skinBar = playerService.GetSkinBar();
        List<int> skinUsing = playerService.GetSkinCat();
        InitBGAndScoreAndMapLevel(score, level, skinBG);
        barSpawner.Initialized(model.SpacingBarY, level, skins[FindSkinIndexById(skinBar)].Image, model.Map, model.AllMap, model.Map.Count);
        catSpawner.Initialized(model.TimeJump, model.HeightCatJump, model.HeightFishJump, view.FishScore, model.QuantityCatIn1CatType, skinUsing, skins, model.TimeCoinMove);
        Application.targetFrameRate = 60;
        isPause = false;
        isCheckInput = true;
        isCheckRollback = true;
        isMatching = false;
        stackRollback = new Stack<Rollback>();
        catSelected = new();


        if (iapService.IsRemoveAds() == false)
        {
            adsService.ShowBannerAds();
        }
        AvoidBanner(adsService.GetHightBanner());
        view.HideBtnWatchAds(adsService.IsRewardedReady());
        adsService.OnRewardedAdsLoad = view.HideBtnWatchAds;

        Debug.Log(playerService.GetAds());
        if (playerService.GetAds() == 2)
        {
            playerService.SetAds(0);
            Debug.Log("InterstitialAds");
            InterstitialAds();

        }

        model.ThrowIfNull();
        view.ThrowIfNull();
        catSpawner.ThrowIfNull();
        barSpawner.ThrowIfNull();
        panelShop.ThrowIfNull();



    }
    public void InterstitialAds()
    {
        if (adsService.IsInterstitialReady() == true && iapService.IsRemoveAds() == false)
        {
            adsService.OnInterstitialClose = () =>
            {

            };
            adsService.ShowLimitInterstitialAd();
        }
        else
        {

        }
    }
    public void AvoidBanner(float bannerHeight)
    {
        //Debug.Log(view.Main.scaleFactor);
        //bannerHeight /= view.Main.scaleFactor;
        CanvasScaler canvasScaler = view.Main.GetComponent<CanvasScaler>();
        float canvasScale = canvasScaler.scaleFactor;
        Debug.Log("Canvas Scale: " + canvasScale);
        view.AdsBannerBottomLeft.sizeDelta = new Vector2(view.AdsBannerBottomLeft.sizeDelta.x, view.AdsBannerBottomLeft.sizeDelta.y + bannerHeight + 100f);
        view.AdsBannerBottomRight.sizeDelta = new Vector2(view.AdsBannerBottomRight.sizeDelta.x, view.AdsBannerBottomRight.sizeDelta.y + bannerHeight + 100f);
    }
    public void GetCoin()
    {
        adsService.InitRewardedAd(() =>
        {
            int currentCoin = playerService.GetScore();
            playerService.SetScore(currentCoin + 100);
            StartCoroutine(SpamCoin(10, 100, currentCoin));
        }, () =>
        {
            Debug.Log("fail");
        });
        adsService.ShowRewardedAd();
    }
    public void GetBoosterAds(BoosterType boosterType)
    {
        adsService.InitRewardedAd(() =>
        {
            switch (boosterType)
            {
                case BoosterType.Add:
                    int quantityCurrentAdd = playerService.GetQuantityBoosterAddBar() + 1;
                    playerService.SetQuantityBoosterAddBar(quantityCurrentAdd);
                    view.SetQuantityBoosterAddbarText(quantityCurrentAdd);
                    break;
                case BoosterType.Rollback:
                    int quantityCurrentUndo = playerService.GetQuantityBoosterRollback() + 1;
                    playerService.SetQuantityBoosterRollback(quantityCurrentUndo);
                    view.SetQuantityBoosterRollback(quantityCurrentUndo, stackRollback.Count);
                    break;
                case BoosterType.NextMap:
                    int quantityCurrentNext = playerService.GetQuantityBoosterNextMap() + 1;
                    playerService.SetQuantityBoosterNextMap(quantityCurrentNext);
                    view.SetQuantityBoosterNextMap(quantityCurrentNext);
                    break;
            }
        }, () =>
        {
            Debug.Log("fail");
        });
        adsService.ShowRewardedAd();

    }
    private IEnumerator SpamCoin(int quantity, int coin, int totalCoin)
    {
        for (int i = 0; i < quantity; i++)
        {
            Vector2 temp = new Vector2(Random.Range(-3f, 3f), Random.Range(1f, 3f));
            GameObject go = SimplePool.Spawn(view.PrefabCoin, view.ImageCoinInPanelGetCoin.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.05f);
            go.transform.SetParent(view.ImagePanelGetCoin.transform);
            go.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            go.transform.DOMove(temp, 1f).OnComplete(() =>
            {
                go.transform.DOMove(view.FishScore.transform.position, 1f).OnComplete(() =>
                {
                    SimplePool.Despawn(go);
                    StartCoroutineIncrementCoin(coin, totalCoin);
                });
            });
        }
    }
    private void StartCoroutineIncrementCoin(int coin, int totalCoin)
    {
        StartCoroutine(IncrementCoin(coin, totalCoin));
    }
    private IEnumerator IncrementCoin(int coin, int totalCoin)
    {
        float increaseTime = 1.0f;
        int targetCoin = totalCoin + coin;
        float elapsedTime = 0.0f;
        while (totalCoin < targetCoin)
        {
            elapsedTime += Time.deltaTime;
            float incrementalValue = Mathf.Lerp(0, targetCoin - totalCoin, elapsedTime / increaseTime);
            totalCoin += Mathf.FloorToInt(incrementalValue);
            view.SetScore(totalCoin);
            yield return null;
        }
    }
    private void InitBGAndScoreAndMapLevel(int score, int mapLevel, int skinBG)
    {
        view.SetScore(score);
        view.SetMapLevel(mapLevel);
        view.SetBackground(skins[FindSkinIndexById(skinBG)].Image);
    }

    private void Start()
    {
        if (barSpawner.MaxCountJump > 0)
        {
            view.JumpCount(true);
            view.SetJumpCount(barSpawner.MaxCountJump);
        }
    }
    public void InitSkinItem()
    {
        //copy list skinItem
        this.skins = model.SkinItems.Select(n => new SkinItem(n.Id, n.Price, n.Status, n.SkinType, n.Image, n.LevelUnlock, n.PrefabBird)).ToList();
        List<int> skinOwned = new();
        List<int> skinBirdUsing = new();
        int bgUsing = 0;
        int barUsing = 0;
        if (playerService.GetStartGameFirstTime() == 0)
        {
            foreach (var skinItem in skins)
            {
                if (skinItem.Status == StatusState.Own || skinItem.Status == StatusState.Equip)
                {
                    skinOwned.Add(skinItem.Id);
                }
                if (skinItem.Status == StatusState.Equip)
                {
                    switch (skinItem.SkinType)
                    {
                        case SkinType.Bird:
                            skinBirdUsing.Add(skinItem.Id);
                            break;
                        case SkinType.Bar:
                            barUsing = skinItem.Id;
                            break;
                        case SkinType.Background:
                            bgUsing = skinItem.Id;
                            break;
                    }
                }
            }
            playerService.SetSkinOwned(skinOwned);
            playerService.SetSkinBG(bgUsing);
            playerService.SetSkinBar(barUsing);
            playerService.SetSkinCat(skinBirdUsing);
        }
        else
        {
            skinOwned = playerService.GetSkinOwned();
            skinBirdUsing = playerService.GetSkinCat();
            bgUsing = playerService.GetSkinBG();
            barUsing = playerService.GetSkinBar();

            foreach (var skin in skins)
            {
                if (skinOwned.Contains(skin.Id))
                {
                    if (bgUsing == skin.Id || barUsing == skin.Id || skinBirdUsing.Contains(skin.Id))
                    {
                        skin.Status = StatusState.Equip;
                    }
                    else skin.Status = StatusState.Own;
                }
            }
            // Sort out which bird skins you are using and bring them to the front
            for (int i = 0; i < skinBirdUsing.Count; i++)
            {
                int index = FindSkinIndexById(skinBirdUsing[i]);
                if (index > 0)
                {
                    SkinItem temp = skins[index];
                    skins[index] = skins[i];
                    skins[i] = temp;
                }
            }
        }
        panelShop.Init(skins, ProcessBuy, ProcessUsing, playerService.GetLevel());
    }

    private int FindSkinIndexById(int id)
    {
        for (int i = 0; i < skins.Count; i++)
        {
            if (skins[i].Id == id)
            { return i; }
        }
        return -1;
    }
    //if the quanity of booster is greater than one, set opacity = 1; if the quantity of booster is equal to 0 or cannot be used, set opacity = 0.5.
    public void CheckBooster()
    {
        int quantityBoosterAddBar = playerService.GetQuantityBoosterAddBar();
        int quantityBoosterRollback = playerService.GetQuantityBoosterRollback();
        int quantityBoosterNextMap = playerService.GetQuantityBoosterNextMap();
        view.SetQuantityBoosterAddbarText(quantityBoosterAddBar);
        view.SetQuantityBoosterRollback(quantityBoosterRollback, stackRollback.Count);
        view.SetQuantityBoosterNextMap(quantityBoosterNextMap);
    }
    private void Update()
    {
        if (!isPause)
        {
            if (isCheckInput)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    InputControl();
                }
            }

            //if (stackRollback.Count <= 0) view.ImageButtonRollback(0.5f);
            CheckBooster();
        }
        if (barSpawner.MaxCountJump == 0)
        {
            if (playerService.GetLevel() >= model.Map.Count)
            {
                playerService.SetLevel(1);
            }
            isPause = true;
            view.PanelGameOver(true);
        }
        else if (barSpawner.MaxCountJump < 0)
        {
            view.JumpCount(false);
        }
    }
    private void ClickCatSelect(GameObject gObject)
    {
        if (gObject.CompareTag(Constanst.NameBarLeftTag) || gObject.CompareTag(Constanst.NameBarRightTag))
        {
            barSelected = gObject.GetComponent<Bar>();
            catSelected = barSelected.SelectCat();
            if (catSelected == null) barSelected = null;
        }
    }
    private void InputControl()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        audioService.PlaySelect();
        if (barSelected == null)
        {
            if (hit.collider != null)
            {
                ClickCatSelect(hit.collider.gameObject);
            }
        }
        else
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<Bar>() != barSelected && catSelected != null)
            {
                barSelectedTarget = hit.collider.gameObject.GetComponent<Bar>();
                List<Cat> catTemp = new List<Cat>();
                if (barSelected.tag.CompareTo(barSelectedTarget.tag) == 0)
                {
                    if (barSelectedTarget.AddCat1(catSelected))
                    {
                        GameObject tempGameObject = hit.collider.gameObject;

                        for (int i = catSelected.Count - 1; i >= 0; i--)
                        {
                            if (i == catSelected.Count - 1)
                            {
                                catSelected[i].OnSetAnimBar = barSelectedTarget.SetAnimation;
                            }
                            catSelected[i].StartPosition = catSelected[i].transform.position;
                            Transform getPosition = null;
                            if (tempGameObject.CompareTag(Constanst.NameBarLeftTag))
                            {
                                getPosition = barSelectedTarget.GetPositionEmptyInBarLeft();
                            }
                            else if (tempGameObject.CompareTag(Constanst.NameBarRightTag))
                            {
                                getPosition = barSelectedTarget.GetPositionEmptyInBarRight();
                            }
                            if (getPosition != null)
                            {
                                //jump to tagetPosition
                                barSelected.RemoveCat();
                                //catSelected[i].TargetTransform = getPosition;
                                //catSelected[i].CatCountJump = catSelected.Count;
                                catSelected[i].Jump(getPosition, catSelected.Count);
                                //catSelected[i].transform.SetParent(getPosition);
                                catTemp.Add(catSelected[i]);
                                //yield return new WaitForSeconds(0.03f);
                            }
                        }
                        barSelected.OffOutLineSelect(catSelected);
                        //add to stack rollback
                        Rollback rollback = new Rollback();
                        rollback.BarSelected = barSelected;
                        rollback.BarSelectedTarget = barSelectedTarget;
                        rollback.Cats = catTemp;
                        this.barSelected = null;
                        barSpawner.MaxCountJump--;
                        view.SetJumpCount(barSpawner.MaxCountJump);
                        //Match Cat
                        StartCoroutine(this.MatchCat(barSelectedTarget, rollback));

                    }
                    else
                    {
                        barSelected.OffOutLineSelect(catSelected);
                        ClickCatSelect(hit.collider.gameObject);
                    }



                }
                else
                {
                    if (barSelectedTarget.AddCat(catSelected))
                    {
                        GameObject tempGameObject = hit.collider.gameObject;
                        for (int i = 0; i < catSelected.Count; i++)
                        {
                            audioService.PlayFly();
                            if (i == catSelected.Count - 1)
                            {
                                catSelected[i].OnSetAnimBar = barSelectedTarget.SetAnimation;
                            }
                            catSelected[i].StartPosition = catSelected[i].transform.position;
                            Transform getPosition = null;
                            if (tempGameObject.CompareTag(Constanst.NameBarLeftTag))
                            {
                                getPosition = barSelectedTarget.GetPositionEmptyInBarLeft();
                            }
                            else if (tempGameObject.CompareTag(Constanst.NameBarRightTag))
                            {
                                getPosition = barSelectedTarget.GetPositionEmptyInBarRight();
                            }
                            if (getPosition != null)
                            {
                                //jump to tagetPosition
                                barSelected.RemoveCat();
                                //catSelected[i].TargetTransform = getPosition;
                                //catSelected[i].CatCountJump = catSelected.Count;
                                catSelected[i].Jump(getPosition, catSelected.Count);
                                //catSelected[i].transform.SetParent(getPosition);
                                catTemp.Add(catSelected[i]);
                                //yield return new WaitForSeconds(0.03f);
                            }
                        }
                        barSelected.OffOutLineSelect(catSelected);
                        //add to stack rollback
                        Rollback rollback = new Rollback();
                        rollback.BarSelected = barSelected;
                        rollback.BarSelectedTarget = barSelectedTarget;
                        rollback.Cats = catTemp;
                        this.barSelected = null;
                        barSpawner.MaxCountJump--;
                        view.SetJumpCount(barSpawner.MaxCountJump);
                        //Match Cat
                        StartCoroutine(this.MatchCat(barSelectedTarget, rollback));

                    }
                    else
                    {
                        barSelected.OffOutLineSelect(catSelected);
                        ClickCatSelect(hit.collider.gameObject);
                    }
                }


            }

        }
    }
    public void AddBooster()
    {
        int quantityBoosterAddBar = playerService.GetQuantityBoosterAddBar();
        if (quantityBoosterAddBar > 0)
        {
            if (barSpawner.BarQuantity < 10)
            {
                barSpawner.AddBarBooster();
                barSpawner.IsMovementBarWhenBooster = true;
                playerService.SetQuantityBoosterAddBar(quantityBoosterAddBar - 1);
            }
            else StartCoroutine(OnTextStatusOneSecond(Constanst.TextMaxBranches));

        }
    }

    public void BoosterRollback()
    {
        int quantityBoosterRollback = playerService.GetQuantityBoosterRollback();
        if (stackRollback.Count > 0 && quantityBoosterRollback > 0 && isCheckRollback && !isMatching)
        {
            Rollback rollback = stackRollback.Pop();
            List<Cat> listCatSelected = rollback.Cats;
            //remove cat form bar
            rollback.BarSelectedTarget.PopStackBar(listCatSelected.Count);
            if (rollback.BarSelected.AddCatRollback(listCatSelected))
            {
                //moving bar old
                for (int i = 0; i < listCatSelected.Count; i++)
                {
                    if (i == listCatSelected.Count - 1)
                    {
                        listCatSelected[i].OnSetAnimBar = rollback.BarSelected.SetAnimation;
                    }
                    listCatSelected[i].StartPosition = listCatSelected[i].transform.position;
                    Transform getPosition = null;
                    if (rollback.BarSelected.CompareTag(Constanst.NameBarLeftTag))
                    {
                        getPosition = rollback.BarSelected.GetPositionEmptyInBarLeft();
                    }
                    else if (rollback.BarSelected.CompareTag(Constanst.NameBarRightTag))
                    {
                        getPosition = rollback.BarSelected.GetPositionEmptyInBarRight();
                    }
                    if (getPosition != null)
                    {
                        //listCatSelected[i].TargetTransform = getPosition;
                        //listCatSelected[i].CatCountJump = catSelected.Count;
                        listCatSelected[i].Jump(getPosition, catSelected.Count);
                        //listCatSelected[i].transform.SetParent(getPosition);
                    }
                }
            }
            playerService.SetQuantityBoosterRollback(quantityBoosterRollback - 1);
        }
    }
    public void BoosterNextMapLevel()
    {
        int quantityBoosterNextMap = playerService.GetQuantityBoosterNextMap();
        if (quantityBoosterNextMap > 0)
        {
            int mapLevelCurrent = playerService.GetLevel();
            playerService.SetLevel(mapLevelCurrent + 1);
            playerService.SetQuantityBoosterNextMap(quantityBoosterNextMap - 1);
            int numAds = playerService.GetAds();
            playerService.SetAds(numAds + 1);
            SceneManager.LoadScene(Constanst.GamePlayScene);
        }

    }
    public void OnButtonBooster(BoosterType boosterType)
    {
        switch (boosterType)
        {
            case BoosterType.Add:
                if (playerService.GetQuantityBoosterAddBar() > 0)
                    this.AddBooster();
                else
                    this.OnPanelAddBarBooster(true);
                break;
            case BoosterType.Rollback:
                if (playerService.GetQuantityBoosterRollback() > 0)
                    this.BoosterRollback();
                else
                    this.OnPanelRollbackBooster(true);
                break;
            case BoosterType.NextMap:
                if (playerService.GetQuantityBoosterNextMap() > 0)
                    this.BoosterNextMapLevel();
                else
                    this.OnPanelNextMapBooster(true);
                break;
        }
    }

    private IEnumerator MatchCat(Bar bar, Rollback rollback)
    {
        if (bar.MatchCat())
        {
            isMatching = true;
            isCheckInput = false;
            yield return new WaitForSeconds(1.5f);
            //clear stack roll back
            stackRollback.Clear();
            List<Cat> listCatTemp = bar.StackToList();
            //clear stack bar
            bar.ClearStack();
            //switch to coin
            foreach (Cat cat in listCatTemp)
            {
                cat.StartAnimIdle = false;
                cat.IncreaseTimeScaleAnimTransform(1.2f);
                cat.StartAnimIdle10();
                Vector3 temp = cat.transform.position;
                cat.transform.SetParent(null);
                temp.y += 0.1f;
                cat.transform.position = temp;
            }
            isCheckInput = true;
            audioService.PlayMatch();
            yield return new WaitForSeconds(1f);
            foreach (Cat cat in listCatTemp)
            {
                cat.StartAnimIdle = false;
                cat.IncreaseTimeScaleAnimTransform(1.2f);
                cat.TransfromToCoin();
            }
            yield return new WaitForSeconds(0.6f);
            //move coin
            foreach (Cat cat in listCatTemp)
            {
                cat.StartPosition = cat.transform.position;
                cat.MeshRenderer.sortingOrder = 30;
                cat.IsMatchScore = true;
                cat.InitializedActionSetScore(SetAndSaveScorePlusOne);
                yield return new WaitForSeconds(0.1f);
            }
            //check next level map
            if (barSpawner.CheckFinishMap())
            {
                //wait coin move 
                yield return new WaitForSeconds(2f);
                audioService.PlayWin();
                int mapLevelCurrent = playerService.GetLevel();
                view.ShowPanelNextMapLevel(mapLevelCurrent);
                view.StatusCoinTop(true);
                playerService.SetLevel(mapLevelCurrent + 1);
                int numAds = playerService.GetAds();
                playerService.SetAds(numAds + 1);
            }
            isMatching = false;
        }
        else
        {
            isCheckRollback = false;
            yield return new WaitForSeconds(1.5f);
            isCheckRollback = true;
            stackRollback.Push(rollback);
        }
    }
    public void SetAndSaveScorePlusOne()
    {
        int scoreCurrent = playerService.GetScore();
        view.SetScore(scoreCurrent + 1);
        playerService.SetScore(scoreCurrent + 1);
    }
    public void OnButtonReset()
    {
        int numAds = playerService.GetAds();
        playerService.SetAds(numAds + 1);
        audioService.PlayButton();
        SceneManager.LoadScene(Constanst.GamePlayScene);

    }
    public void OnPanelReset(bool status)
    {
        isPause = status;
        view.StatusCoinTop(status);
        view.PanelReset(status);
        audioService.PlayButton();
    }
    public void OnPanelGetCoin(bool status)
    {
        isPause = status;
        view.PanelGetCoin(status);
        audioService.PlayButton();
    }
    public void OnPanelHome(bool status)
    {
        isPause = status;
        view.PanelHome(status);
        audioService.PlayButton();
    }
    public void OnPanelSetting(bool status)
    {
        isPause = status;
        view.PanelSetting(status);
        audioService.PlayButton();

    }
    public void OnPanelShop(bool status)
    {
        if (status)
        {
            isPause = true;
            view.PanelShop(true);
        }
        else
        {
            if (playerService.GetSkinCat().Count < 8)
                StartCoroutine(OnTextStatusOneSecond(Constanst.TextNotFullBird));
            else
            {
                isPause = false;
                view.PanelShop(false);
            }
        }
        audioService.PlayButton();
    }
    public void OnPanelLimitPacked(bool status)
    {
        isPause = status;
        view.StatusCoinTop(status);
        view.PanelLimitPacked(status);
        audioService.PlayButton();
    }
    public void OnPanelNextMapBooster(bool status)
    {
        isPause = status;
        view.PanelBoosterNextMap(status);
        audioService.PlayButton();
    }
    public void OnPanelRollbackBooster(bool status)
    {
        isPause = status;
        view.PanelBoosterRollback(status);
        audioService.PlayButton();
    }
    public void OnPanelAddBarBooster(bool status)
    {
        isPause = status;
        view.PanelBoosterAddBar(status);
        audioService.PlayButton();
    }
    //Buy booster
    public void OnButtonBuyBooster(BoosterType boosterType, int quantity)
    {
        audioService.PlayButton();
        int score = playerService.GetScore();
        int totalPrice;
        switch (boosterType)
        {
            case BoosterType.Add:
                //get price
                totalPrice = (quantity == 1) ? model.PriceOneAddBarBooster : (quantity == 3) ? model.PriceThreeAddBarBooster : 0;
                if (score >= totalPrice)
                {
                    int scoreCurrent = score - totalPrice;
                    playerService.SetScore(scoreCurrent);
                    int quantityBoosterAddBar = playerService.GetQuantityBoosterAddBar();
                    int quantityBoosterAddBarCurrent = quantityBoosterAddBar + quantity;
                    playerService.SetQuantityBoosterAddBar(quantityBoosterAddBarCurrent);
                    view.SetScore(scoreCurrent);
                    StartCoroutine(OnTextStatusOneSecond(Constanst.TextStatusSuccess));
                }
                else StartCoroutine(OnTextStatusOneSecond(Constanst.TextStatusFail));
                break;
            case BoosterType.Rollback:
                totalPrice = (quantity == 1) ? model.PriceOneRollbackBooster : (quantity == 3) ? model.PriceThreeRollbackBooster : 0;
                if (score >= totalPrice)
                {
                    int scoreCurrent = score - totalPrice;
                    playerService.SetScore(scoreCurrent);
                    int quantityBoosterRollback = playerService.GetQuantityBoosterRollback();
                    int quantityBoosterRollbackCurrent = quantityBoosterRollback + quantity;
                    playerService.SetQuantityBoosterRollback(quantityBoosterRollbackCurrent);
                    view.SetScore(scoreCurrent);
                    StartCoroutine(OnTextStatusOneSecond(Constanst.TextStatusSuccess));
                }
                else StartCoroutine(OnTextStatusOneSecond(Constanst.TextStatusFail));
                break;
            case BoosterType.NextMap:
                totalPrice = (quantity == 1) ? model.PriceOneNextMapBooster : (quantity == 3) ? model.PriceThreeNextMapBooster : 0;
                if (score >= totalPrice)
                {
                    int scoreCurrent = score - totalPrice;
                    playerService.SetScore(scoreCurrent);
                    int quantityBoosterNextMap = playerService.GetQuantityBoosterNextMap();
                    int quantityBoosterNextMapCurrent = quantityBoosterNextMap + quantity;
                    playerService.SetQuantityBoosterNextMap(quantityBoosterNextMapCurrent);
                    view.SetScore(scoreCurrent);
                    StartCoroutine(OnTextStatusOneSecond(Constanst.TextStatusSuccess));
                }
                else StartCoroutine(OnTextStatusOneSecond(Constanst.TextStatusFail));
                break;
        }
    }
    private IEnumerator OnTextStatusOneSecond(string textStatus)
    {
        view.SetTextStatus(textStatus);
        view.TextStatus.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        view.TextStatus.gameObject.SetActive(false);
    }
    public void ProcessBuy(SkinItem skinItem)
    {
        if (skinItem.Status == StatusState.Buy)
        {
            int score = playerService.GetScore();
            if (score >= skinItem.Price)
            {
                score -= skinItem.Price;
                StartCoroutine(OnTextStatusOneSecond(Constanst.TextStatusSuccess));
                playerService.SetScore(score);
                view.SetScore(score);
                List<int> skinOwned = playerService.GetSkinOwned();
                skinOwned.Add(skinItem.Id);
                playerService.SetSkinOwned(skinOwned);
                panelShop.UpdateBuy(skinItem, ProcessUsing);
            }
            else
            {
                StartCoroutine(OnTextStatusOneSecond(Constanst.TextStatusFail));
            }

        }

    }
    public void SetSkinOwned(int index, int value)
    {
        List<int> skinBirdUsing = playerService.GetSkinCat();
        skinBirdUsing.Insert(index - 1, value);
        playerService.SetSkinCat(skinBirdUsing);
    }
    public void ProcessUsing(SkinItem skinItem)
    {
        if (skinItem.Status == StatusState.Own)
        {
            switch (skinItem.SkinType)
            {
                case SkinType.Bird:
                    List<int> skinBirdUsing = playerService.GetSkinCat();
                    if (skinBirdUsing.Count < 8)
                    {
                        panelShop.UpdateEquip(skinItem, SetSkinOwned);
                    }
                    else StartCoroutine(OnTextStatusOneSecond(Constanst.TextFullBird));
                    break;
                case SkinType.Bar:
                    playerService.SetSkinBar(skinItem.Id);
                    panelShop.UpdateEquip(skinItem, SetSkinOwned);
                    //call real time change bar
                    barSpawner.UpdateWhenEquipSkin(skinItem.Image);
                    break;
                case SkinType.Background:
                    playerService.SetSkinBG(skinItem.Id);
                    //call real time change background
                    view.SetBackground(skinItem.Image);
                    panelShop.UpdateEquip(skinItem, SetSkinOwned);
                    break;

            }

        }
        //Un pick bird
        else if (skinItem.Status == StatusState.Equip && skinItem.SkinType == SkinType.Bird)
        {
            List<int> skinBirdUsing = playerService.GetSkinCat();
            panelShop.UpdateEquip(skinItem, SetSkinOwned);
            skinBirdUsing.Remove(skinItem.Id);

            playerService.SetSkinCat(skinBirdUsing);
        }
    }

}
