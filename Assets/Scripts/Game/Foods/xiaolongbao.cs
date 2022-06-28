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

    private BulletType bulletType;  // �ӵ�����
    private Vector3 createBulletLocalPos;    // �ӵ���ʼλ��

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
    /// ��ʼ��ʳ��
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="level"></param>
    public override void InitFood(M_Grid grid, int level = 1)
    {
        base.InitFood(grid, level);

        InvokeRepeating("Attack", 0, 0.1F);
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Attack() {
        if (isAttack == false)    // û�п�ʼ����
            return;

        MouseBase mouse = GI_MouseManager.Instance.GetScopeMouse(grid, transform.position);

        if (mouse == null ||    // ǰ��û������
            (mouse.Grid.Point.x == 8 && 
            Vector2.Distance(mouse.transform.position,mouse.Grid.Position) > 1.5F))  // ���������ر�Զ
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
    /// ����CD
    /// </summary>
    private void CDEnter()
    {
        //  ������ȴ
        StartCoroutine(CalCD());
    }
    /// <summary>
    /// ��ȴʱ�����
    /// </summary>
    /// <returns></returns>
    private IEnumerator CalCD()
    {
        yield return new WaitForSeconds(AttackSpeed);
        isAttack = true;
    }
}
