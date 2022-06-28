using Assets.Scripts.Enum;
using Assets.Scripts.Game.Foods;
using System.Collections;
using UnityEngine;

public class pingmingshu : MouseBase
{
    protected override float Speed => 5;
    protected override float AttackVal => 100F;
    protected override float MaxHealth => 100F;

    private bool isAttack;  // 是否在攻击

    protected override void Awake()
    {
        base.Awake();

        isAttack = false;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        StatuFSM();
    }

    /// <summary>
    /// 老鼠状态
    /// </summary>
    public void StatuFSM()
    {
        grid = GI_GridManager.Instance.GetGridPointByWorldPos(transform.position);
        if (grid is null)
            return;
        switch (Statu)
        {
            case MouseStatu.Main:
                Statu = MouseStatu.Walk;
                break;
            case MouseStatu.Walk:   // 正常移动
                transform.Translate(new Vector2(-1.33F, 0) * (Time.deltaTime / 1) / Speed);
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
    IEnumerator DoDamage(FoodBase food)
    {
        while (grid.Food != null && food.Health > 0)
        {
            food.Damage(AttackVal / 5);
            yield return new WaitForSeconds(0.2F);
        }
        Statu = MouseStatu.Walk;
        isAttack = false;
    }

    
}
