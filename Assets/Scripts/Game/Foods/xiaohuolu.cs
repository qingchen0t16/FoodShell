using Assets.Scripts.Game.Foods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class xiaohuolu : FoodBase
{
    private float SpanAttack;    // ��һ������ʱ��

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
    /// ��������
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
    /// ��ʼ��ʳ��
    /// </summary>
    protected override void _initFood(int level)
    {
        InvokeRepeating("CreateFire", AttackSpeed, SpanAttack);
    }
}
