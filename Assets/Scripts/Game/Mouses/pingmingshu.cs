using Assets.Scripts.Enum;
using Assets.Scripts.Game.Foods;
using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pingmingshu : MonoBehaviour
{
    private Animator an;
    private MouseStatu statu;   // 老鼠当前状态
    public MouseStatu Statu
    {
        get => statu;
        private set
        {
            statu = value;
            switch (statu)
            {
                case MouseStatu.Main:
                    an.speed = 0;
                    an.Play("Main",0,0);
                    break;
                case MouseStatu.Walk:
                    an.speed = 1;
                    an.Play("Main", 0, 0);
                    break;
                case MouseStatu.Disabled:
                    an.speed = 1;
                    an.Play("Disabled");
                    break;
                case MouseStatu.Eat:
                    an.speed = 1;
                    an.Play("Eat");
                    break;
                case MouseStatu.DisabledEat:
                    an.speed = 1;
                    an.Play("DisabledEat");
                    break;
                case MouseStatu.Dead:
                    an.speed = 1;
                    an.Play("Dead");
                    break;
            }
        }
    }
    private M_Grid grid;    // 老鼠所在网格
    private float speed;  // 老鼠速度
    private float attack;   // 老鼠攻击力
    private bool isAttack;  // 是否在攻击

    private void Awake()
    {
        speed = 5;
        attack = 100F;
        an = GetComponentInChildren<Animator>();
        isAttack = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetGridByVerticalNum(0);
    }

    // Update is called once per frame
    void Update()
    {
        StatuFSM();
    }

    /// <summary>
    /// 老鼠状态
    /// </summary>
    public void StatuFSM() {
        grid = GI_GridManager.Instance.GetGridPointByWorldPos(transform.position);
        if (grid is null)
            return;
        switch (Statu)
        {
            case MouseStatu.Main:
                Statu = MouseStatu.Walk;
                break;
            case MouseStatu.Walk:   // 正常移动
                transform.Translate(new Vector2(-1.33F, 0) * (Time.deltaTime / 1) / speed);
                if (grid.Food != null && 
                    grid.Statu == GridStatu.food && 
                    grid.Food.transform.position.x < transform.position.x && 
                    transform.position.x - grid.Food.transform.position.x < 0.1F)
                    Statu = MouseStatu.Eat;
                break;
            case MouseStatu.Eat:
                if (isAttack)
                    break;
                Attack(grid.Food.GetComponent<FoodBase>());
                break;
            case MouseStatu.Disabled:
                break;
            case MouseStatu.DisabledEat:
                break;
            case MouseStatu.Dead:
                break;
        }
    }

    /// <summary>
    /// 攻击
    /// </summary>
    /// <param name="food"></param>
    public void Attack(FoodBase food)
    {
        isAttack = true;
        StartCoroutine(DoDamage(food));
    }
    IEnumerator DoDamage(FoodBase food) {
        while (grid.Food != null && food.Health > 0)
        {
            food.Damage(attack / 5);
            yield return new WaitForSeconds(0.2F);
        }
        Statu = MouseStatu.Walk;
        isAttack = false;
    }

    /// <summary>
    /// 移动到网格
    /// </summary>
    /// <param name="vNum"></param>
    public void GetGridByVerticalNum(int vNum)
    {
        grid = GI_GridManager.Instance.GetGridByVerticalNum(vNum);
        transform.position = new Vector3(transform.position.x, grid.Position.y, 0);
    }
}
