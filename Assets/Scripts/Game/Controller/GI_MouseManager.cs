using Assets.Scripts.Enum;
using Assets.Scripts.Game.Foods;
using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GI_MouseManager : MonoBehaviour
{
    public static GI_MouseManager Instance;

    private List<MouseBase> mousesList; // ȫ�������б�
    private Vector2 mouseLi;
    private int currOrderNum = 0;

    public int CurrOrderNum
    {
        get => currOrderNum; set
        {
            currOrderNum = value;
            if (currOrderNum > 50)
                currOrderNum = 0;
        }
    }

    private void Awake()
    {
        Instance = this;

        mousesList = new List<MouseBase>();
    }

    public void CreateMouse(int row, MousesType mouseType) {
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="args"></param>
    public void AddMouses(params MouseBase[] args) {
        foreach (MouseBase item in args)
            mousesList.Add(item.SetSorting(CurrOrderNum++));
    }

    /// <summary>
    /// �Ƴ�����
    /// </summary>
    /// <param name="args"></param>
    public void RemoveMouses(params MouseBase[] args) {
        foreach (MouseBase item in args)
            mousesList.Remove(item);
    }

    /// <summary>
    /// ��ȡ��Χ�ڵ�����
    /// </summary>
    /// <returns></returns>
    public MouseBase GetScopeMouse(M_Grid grid,Vector3 pos,MM_ScopeType type = MM_ScopeType.InFrontTheRecent) {
        MouseBase mouse = null;
        switch (type)
        {
            case MM_ScopeType.InFrontTheRecent: // ǰ���������������
                float dis = 99999;
                foreach (MouseBase item in mousesList)
                {
                    if (item.Grid.Point.y == grid.Point.y && Vector2.Distance(pos, item.transform.position) < dis)
                    {
                        dis = Vector2.Distance(pos, item.transform.position);
                        mouse = item;
                    }
                }
                break;
        }
        return mouse;
    }
}
