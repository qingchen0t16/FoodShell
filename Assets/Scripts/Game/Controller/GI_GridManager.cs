using Assets.Scripts.Enum;
using Assets.Scripts.Model;
using System.Collections.Generic;
using UnityEngine;

public class GI_GridManager : MonoBehaviour
{
    public static GI_GridManager Instance;

    private GameObject GridBox; // ����
    private List<M_Grid> gridList;  // �����б�

    private void Awake()
    {
        Instance = this;
        GridBox = GetComponent<GameObject>();
        CreateGrids();  // ��������
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
            Debug.Log(GetGridPointByMouse().Point);*/
    }

    /// <summary>
    /// ͨ��Y��(����)��ȡ��ʼ����
    /// </summary>
    /// <param name="verticalNum"></param>
    /// <returns></returns>
    public M_Grid GetGridByVerticalNum(int verticalNum)
    {
        for (int i = 0; i < gridList.Count; i++)
            if (gridList[i].Point == new Vector2(8, verticalNum))
                return gridList[i];
        return null;
    }

    /// <summary>
    /// ����Grid
    /// </summary>
    private void CreateGrids()
    {
        gridList = new List<M_Grid>();
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 6; j++)
                gridList.Add(new(new(i, j),transform.position +  new Vector3(1F * i, 1.08F * j), GridStatu.main));
    }

    /// <summary>
    /// ͨ�����������ȡ���������
    /// </summary>
    /// <returns></returns>
    public M_Grid GetGridPointByWorldPos(Vector2 pos)
    {
        float dis = 99999999;
        M_Grid grid = null;
        for (int i = 0; i < gridList.Count; i++)
        {
            float temp = Vector2.Distance(pos, gridList[i].Position);
            if (temp < dis)
            {
                dis = temp;
                grid = gridList[i];
            }
        }
        return grid;
    }

    /// <summary>
    /// ��ȡ��������λ
    /// </summary>
    /// <returns></returns>
    public M_Grid GetGridPointByMouse()
    {
        return GetGridPointByWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
