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

    private Image foodImg;  // ʳ�￨Ƭͼ
    private Image maskImg;  // ��ȴ����

    public int NeedFire;    // ��Ҫ������

    public float CD;    // ʳ�����ȴʱ��
    private float cdTime;     // ����ȴʱ��

    private bool cdDone;   // �Ƿ񼤻�(�Ƿ���Է���)
    public bool CDDone
    {
        get => cdDone;
        set {
            cdDone = value;
            CheckStatu();
        }
    }
    private GameObject[] spanFood;  // [0] ����ʱ���������ʳ�[1] ����ʱ����Ӱ�͸��ʳ��
    private bool createingFood; // �Ƿ����ڷ���ʳ��
    public bool CreateingFood
    {
        get => createingFood;
        set
        {
            createingFood = value;
            if (createingFood)  // ���ڷ���ʳ��
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

    private CardBarStatu statu; // ��Ƭ״̬
    public CardBarStatu Statu
    {
        get => statu;
        set {
            if (statu == value) {
                
            }
            statu = value;
            switch (statu)
            {
                case CardBarStatu.Feasible: // ����ֲ
                    maskImg.fillAmount = 0;
                    foodImg.color = Color.white;
                    break;
                case CardBarStatu.NeedCD:   // CD��
                    foodImg.color = Color.white;
                    CallCD();
                    break;
                case CardBarStatu.NeedFire: // ���粻��
                        maskImg.fillAmount = 0;
                    foodImg.color = new Color(0.7F, 0.7F, 0.7F);
                    break;
                case CardBarStatu.Disabled: // ���粻����CD��
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
        if (CreateingFood && spanFood[0] != null)   // ��Ҫ����
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + spanFood[0].transform.Find("Food").position);
            spanFood[0].transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            M_Grid grid = GI_GridManager.Instance.GetGridPointByMouse();    // ��ǰ���ָ�����

            if (Vector2.Distance(mousePos, grid.Position) < 1F && grid.Statu == GridStatu.main)    // �������1F����
            {
                if (spanFood[1] == null)
                    spanFood[1] = GameObject
                        .Instantiate<GameObject>(spanFood[0], grid.Position, Quaternion.identity, GI_GameManager.Instance.FoodBox.transform)
                        .GetComponent<FoodBase>()
                        .InitGrid();
                else
                    spanFood[1].transform.position = grid.Position;

                if (Input.GetMouseButtonDown(0))    // �������
                {
                    grid.Statu = GridStatu.food;    // ����ʳ��
                    grid.Food = spanFood[0];    // ����ʳ��
                    spanFood[0].transform.position = grid.Position;
                    spanFood[0].GetComponent<FoodBase>().InitFood(grid);
                    spanFood[0] = null;
                    if (spanFood[1] != null)
                    {
                        Destroy(spanFood[1]);
                        spanFood[1] = null;
                    }
                    CreateingFood = false;
                    CDDone = false; // ����CD

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
    /// ״̬���
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
    /// ������
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
    /// ����뿪
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
    /// ��갴��
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
    /// ������ȴ
    /// </summary>
    private void CallCD()
    {
        if (maskImg.fillAmount != 0)
            return;
        maskImg.fillAmount = 1;
        cdTime = CD;    // ��ʱCD��ֵ
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
        CDDone = true;  // �ɷ���
    }


}
