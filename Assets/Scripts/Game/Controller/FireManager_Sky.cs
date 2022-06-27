using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager_Sky : MonoBehaviour
{
    private GameObject Prefab_Fire; // 火苗预制体

    private float cFirePosY = 340F; // 天空火苗创建位置Y
    private float[] cFirePosX = { -134F, 343F };    // 创建位置X
    private float[] tFirePosY = { 159, -147F};    // 掉落目标随机地址Y

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
    /// 天空生成阳光
    /// </summary>
    public void SkyCreateFire() {
        GameObject.Instantiate<GameObject>(GI_GameManager.Instance.Conf.Fire, Vector3.zero, Quaternion.identity, GameObject.Find("Game/FireBox").transform)
            .GetComponent<Fire>()
            .SkyInit(new float[] { Random.Range(cFirePosX[0], cFirePosX[1]), cFirePosY }, Random.Range(tFirePosY[0], tFirePosY[1]));
    }
}
