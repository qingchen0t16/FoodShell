using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GI_CardBarCtrl : MonoBehaviour
{
    public static GI_CardBarCtrl Instance;

    private Text fireNumText;

    private void Awake()
    {
        Instance = this;

        fireNumText = transform.Find("FireNumText").GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="fireNum"></param>
    public void SetFireNumText(int fireNum)
        => fireNumText.text = fireNum.ToString();

    /// <summary>
    /// ��ȡ�����ı�λ��
    /// </summary>
    /// <returns></returns>
    public Vector3 GetFileTextPos()
        => transform.position;
}
