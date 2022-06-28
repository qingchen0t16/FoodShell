using Assets.Scripts.Game.Foods;
using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class xiaohuolu : FoodBase
{
    private float SpanAttack;    // 第一个生产时间

    public override float MaxHealth => 300F;
    public override float AttackSpeed => 5F;

    // Start is called before the first frame update
    void Start()
    {
        SpanAttack = 8F;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 创建火苗
    /// </summary>
    public void CreateFire()
    {
        an.Play("Active");
        Invoke("SpanFire", 1.2F);
    }
    public void SpanFire()
    {
        GameObject.Instantiate<GameObject>(GI_GameManager.Instance.Conf.Fire, Vector3.zero, Quaternion.identity, GameObject.Find("Game/FireBox").transform)
            .GetComponent<Fire>()
            .FoodInit(transform.localPosition + transform.parent.localPosition + transform.Find("Food").localPosition);
    }

    /// <summary>
    /// 创建食物
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="level"></param>
    public override void InitFood(M_Grid grid, int level = 1)
    {
        base.InitFood(grid, level);

        InvokeRepeating("CreateFire", AttackSpeed, SpanAttack);
    }
}
