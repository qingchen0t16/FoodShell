using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GI_PlayManager : MonoBehaviour
{
    public static GI_PlayManager Instance;

    private UnityAction fireNumUpdateListener;    // 阳光监听委托

    private GI_CardBar_Card selectedCard;   // 当前选中卡片
    public GI_CardBar_Card SelectedCard {
        get => selectedCard;
        set {
            if (selectedCard == value)
                return;
            if(selectedCard != null)
                selectedCard.CreateingFood = false;
            selectedCard = value;
        }
    }

    private int fireNum = 0;    // 火苗数量
    public int FireNum
    {
        get => fireNum;
        set
        {
            fireNum = value;
            GI_CardBarCtrl.Instance.SetFireNumText(fireNum);
            if(fireNumUpdateListener != null)
                fireNumUpdateListener();
        }
    }
    public bool AutoCollect;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FireNum = 100;
        AutoCollect = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 添加火苗更新检测
    /// </summary>
    /// <param name="action"></param>
    public void AddFireNumUpdateListener(UnityAction action) {
        fireNumUpdateListener += action;
    }
}
