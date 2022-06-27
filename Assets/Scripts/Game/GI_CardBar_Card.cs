using Assets.Scripts.Enum;
using Assets.Scripts.Game.Foods;
using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GI_CardBar_Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public FoodsType FoodType;

    private Image foodImg;  // 食物卡片图
    private Image maskImg;  // 冷却遮罩

    public int NeedFire;    // 需要火苗数

    public float CD;    // 食物的冷却时间
    private float cdTime;     // 已冷却时长

    private bool cdDone;   // 是否激活(是否可以放置)
    public bool CDDone
    {
        get => cdDone;
        set {
            cdDone = value;
            CheckStatu();
        }
    }
    private GameObject[] spanFood;  // [0] 生成时候跟随鼠标的食物，[1] 生成时候格子半透明食物
    private bool createingFood; // 是否正在放置食物
    public bool CreateingFood
    {
        get => createingFood;
        set
        {
            createingFood = value;
            if (createingFood)  // 正在放置食物
            {
                spanFood[0] = GameObject
                    .Instantiate<GameObject>(GI_GameManager.Instance.Conf.Foods[(int)FoodType], Vector3.zero, Quaternion.identity, GI_GameManager.Instance.FoodBox.transform)
                    .GetComponent<FoodBase>()
                    .InitCreate();
                foodImg.color = new Color(0.8F, 0.8F, 0.8F, 1);
                GI_PlayManager.Instance.SelectedCard = this;
            }
            else
            {
                if (spanFood[0] != null)
                {
                    Destroy(spanFood[0]);
                    spanFood[0] = null;
                }
                if (spanFood[1] != null)
                {
                    Destroy(spanFood[1]);
                    spanFood[1] = null;
                }
                foodImg.color = new Color(1, 1, 1, 1F);
            }
        }
    }

    private CardBarStatu statu; // 卡片状态
    public CardBarStatu Statu
    {
        get => statu;
        set {
            if (statu == value) {
                
            }
            statu = value;
            switch (statu)
            {
                case CardBarStatu.Feasible: // 可种植
                    maskImg.fillAmount = 0;
                    foodImg.color = Color.white;
                    break;
                case CardBarStatu.NeedCD:   // CD中
                    foodImg.color = Color.white;
                    CallCD();
                    break;
                case CardBarStatu.NeedFire: // 火苗不足
                        maskImg.fillAmount = 0;
                    foodImg.color = new Color(0.7F, 0.7F, 0.7F);
                    break;
                case CardBarStatu.Disabled: // 火苗不足且CD中
                    foodImg.color = new Color(0.7F, 0.7F, 0.7F);
                    CallCD();
                    break;
            }
        }
    }

    private void Awake()
    {
        foodImg = transform.Find("CardImg").GetComponent<Image>();
        maskImg = transform.Find("Fill").GetComponent<Image>();
        spanFood = new GameObject[2];
    }

    // Start is called before the first frame update
    void Start()
    {
        CDDone = false;
        maskImg.fillAmount = 0;
        transform.Find("CardImg/NeedFire").GetComponent<Text>().text = NeedFire.ToString();
        foodImg.sprite = GI_GameManager.Instance.Conf.FoodsCard[(int)FoodType];
        GI_PlayManager.Instance.AddFireNumUpdateListener(CheckStatu);
    }

    // Update is called once per frame
    void Update()
    {
        if (CreateingFood && spanFood[0] != null)   // 需要创建
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + spanFood[0].transform.Find("Food").position);
            spanFood[0].transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            M_Grid grid = GI_GridManager.Instance.GetGridPointByMouse();    // 当前鼠标指向格子

            if (Vector2.Distance(mousePos, grid.Position) < 1F && grid.Statu == GridStatu.main)    // 距离格子1F以内
            {
                if (spanFood[1] == null)
                    spanFood[1] = GameObject
                        .Instantiate<GameObject>(spanFood[0], grid.Position, Quaternion.identity, GI_GameManager.Instance.FoodBox.transform)
                        .GetComponent<FoodBase>()
                        .InitGrid();
                else
                    spanFood[1].transform.position = grid.Position;

                if (Input.GetMouseButtonDown(0))    // 左键按下
                {
                    grid.Statu = GridStatu.food;    // 已有食物
                    grid.Food = spanFood[0];    // 持有食物
                    spanFood[0].transform.position = grid.Position;
                    spanFood[0].GetComponent<FoodBase>().InitFood(grid);
                    spanFood[0] = null;
                    if (spanFood[1] != null)
                    {
                        Destroy(spanFood[1]);
                        spanFood[1] = null;
                    }
                    CreateingFood = false;
                    CDDone = false; // 进入CD

                    GI_PlayManager.Instance.FireNum -= NeedFire;
                }
            }
            else
            {
                if (spanFood[1] != null)
                {
                    Destroy(spanFood[1]);
                    spanFood[1] = null;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                CreateingFood = false;
            }
        }
    }

    /// <summary>
    /// 状态检测
    /// </summary>
    public void CheckStatu() {
        if (cdDone &&
                GI_PlayManager.Instance.FireNum >= NeedFire)
            Statu = CardBarStatu.Feasible;
        else if (!cdDone &&
            GI_PlayManager.Instance.FireNum >= NeedFire)
            Statu = CardBarStatu.NeedCD;
        else if (cdDone &&
            GI_PlayManager.Instance.FireNum < NeedFire)
            Statu = CardBarStatu.NeedFire;
        else
            Statu = CardBarStatu.Disabled;
    }

    /// <summary>
    /// 鼠标进入
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CDDone || Statu != CardBarStatu.Feasible)
            return;
        foodImg.transform.localPosition = new Vector3(0, 1, 0);
    }

    /// <summary>
    /// 鼠标离开
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CDDone || Statu != CardBarStatu.Feasible)
            return;
        foodImg.transform.localPosition = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// 鼠标按下
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CDDone || Statu != CardBarStatu.Feasible)
            return;
        if (!CreateingFood)
            CreateingFood = true;
        else
            CreateingFood = false;
    }

    /// <summary>
    /// 进入冷却
    /// </summary>
    private void CallCD()
    {
        if (maskImg.fillAmount != 0)
            return;
        maskImg.fillAmount = 1;
        cdTime = CD;    // 临时CD赋值
        StartCoroutine(CalCD());
    }
    IEnumerator CalCD()
    {
        float cd = (1 / CD) * 0.1F;
        while (cdTime >= 0)
        {
            yield return new WaitForSeconds(0.1F);
            maskImg.fillAmount -= cd;
            cdTime -= 0.1F;
        }
        CDDone = true;  // 可放置
    }


}
