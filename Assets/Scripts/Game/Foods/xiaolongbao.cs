using Assets.Scripts.Enum;
using Assets.Scripts.Game.Foods;
using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xiaolongbao : FoodBase
{
    public override float MaxHealth => 300F;
    public override float AttackSpeed => 1.5F;

    private BulletType bulletType;  // 子弹类型
    private Vector3 createBulletLocalPos;    // 子弹初始位置

    // Start is called before the first frame update
    void Start()
    {
        bulletType = BulletType.B001;
        createBulletLocalPos = new Vector3(33, 18, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 初始化食物
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="level"></param>
    public override void InitFood(M_Grid grid, int level = 1)
    {
        base.InitFood(grid, level);

        InvokeRepeating("Attack", 0, 0.1F);
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void Attack() {
        if (isAttack == false)    // 没有开始攻击
            return;

        MouseBase mouse = GI_MouseManager.Instance.GetScopeMouse(grid, transform.position);

        if (mouse == null ||    // 前方没有老鼠
            (mouse.Grid.Point.x == 8 && 
            Vector2.Distance(mouse.transform.position,mouse.Grid.Position) > 1.5F))  // 距离网格特别远
            return;

        an.Play("Active", 0, 0);
        Invoke("CreateBullet",0.7F);
        isAttack = false;
        CDEnter();
    }
    public void CreateBullet() {
        GameObject
            .Instantiate(GI_GameManager.Instance.Conf.Bullets[(int)bulletType], Vector3.zero, Quaternion.identity, GI_GameManager.Instance.BulletBox.transform)
            .GetComponent<B001>()
            .Init(transform.localPosition + createBulletLocalPos);
    }
    /// <summary>
    /// 进入CD
    /// </summary>
    private void CDEnter()
    {
        //  计算冷却
        StartCoroutine(CalCD());
    }
    /// <summary>
    /// 冷却时间计算
    /// </summary>
    /// <returns></returns>
    private IEnumerator CalCD()
    {
        yield return new WaitForSeconds(AttackSpeed);
        isAttack = true;
    }
}
