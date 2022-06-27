using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager_Sky : MonoBehaviour
{
    private GameObject Prefab_Fire; // ����Ԥ����

    private float cFirePosY = 340F; // ��ջ��紴��λ��Y
    private float[] cFirePosX = { -134F, 343F };    // ����λ��X
    private float[] tFirePosY = { 159, -147F};    // ����Ŀ�������ַY

    // Start is called before the first frame update
    void Start()
    {
        Prefab_Fire = Resources.Load<GameObject>("Game/Prefab/Fire");

        InvokeRepeating("SkyCreateFire", 0, 3);
    }

     // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �����������
    /// </summary>
    public void SkyCreateFire() {
        GameObject.Instantiate<GameObject>(GI_GameManager.Instance.Conf.Fire, Vector3.zero, Quaternion.identity, GameObject.Find("Game/FireBox").transform)
            .GetComponent<Fire>()
            .SkyInit(new float[] { Random.Range(cFirePosX[0], cFirePosX[1]), cFirePosY }, Random.Range(tFirePosY[0], tFirePosY[1]));
    }
}
