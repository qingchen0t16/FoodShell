using Assets.Scripts.Enum;
using Assets.Scripts.Model;
using UnityEngine;

public abstract class MouseBase : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Animator an;
    protected M_Grid grid;      // ������������
    protected abstract float Speed { get; }      // �����ٶ�
    protected abstract float AttackVal { get; }     // ���󹥻���
    protected abstract float MaxHealth { get; }    // ��������
    protected float health; // ��ǰ����
    protected bool isDisability;    // �Ƿ��Ѫ
    public float Health
    {
        get => health;
        protected set
        {
            health = value;
            if (health <= 0)
                Statu = MouseStatu.Dead;
            else if (health < (MaxHealth / 2) && !isDisability)
            {
                isDisability = true;
                Statu = Statu;  // ��Ҫˢ��״̬
            }
        }
    }

    public M_Grid Grid { get => grid; } // ������������

    protected MouseStatu statu;   // ����ǰ״̬
    public MouseStatu Statu
    {
        get => statu;
        protected set
        {
            statu = value;
            switch (statu)
            {
                case MouseStatu.Main:
                    an.speed = 0;
                    an.Play("Main", 0, 0);
                    break;
                case MouseStatu.Walk:
                    an.speed = 1;
                    an.Play(health < MaxHealth / 2 ? "Disabled" : "Main", 0, isDisability ? an.GetAnimatorTransitionInfo(0).normalizedTime : 0);
                    break;
                case MouseStatu.Eat:
                    an.speed = 1;
                    an.Play((health < MaxHealth / 2 ? "Disabled" : "") + "Eat", 0, isDisability ? an.GetAnimatorTransitionInfo(0).normalizedTime : 0);
                    break;
                case MouseStatu.Dead:
                    an.speed = 1;
                    GI_MouseManager.Instance.RemoveMouses(this);
                    an.Play("Dead",0,0);
                    Invoke("Dead", 1F);
                    break;
            }
        }
    }

    protected virtual void Awake()
    {
        an = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GI_MouseManager.Instance.AddMouses(this);    // ��������
        health = MaxHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="val"></param>
    public MouseBase SetSorting(int val) {
        sr.sortingOrder = val;
        return this;
    }

    /// <summary>
    /// ��ʼ������
    /// </summary>
    public virtual void Init(int line) {
        GetGridByVerticalNum(line);
    }

    /// <summary>
    /// �ܵ��˺�
    /// </summary>
    /// <param name="val"></param>
    public virtual void Damage(float val)
    {
        Health -= val;
    }

    /// <summary>
    /// ����
    /// </summary>
    public virtual void Dead()
    {
        StopAllCoroutines();
        CancelInvoke();
        Destroy(gameObject);
    }

    /// <summary>
    /// �ƶ�������
    /// </summary>
    /// <param name="vNum"></param>
    public void GetGridByVerticalNum(int vNum)
    {
        grid = GI_GridManager.Instance.GetGridByVerticalNum(vNum);
        transform.position = new Vector3(grid.Position.x, grid.Position.y, 0);
    }
}
