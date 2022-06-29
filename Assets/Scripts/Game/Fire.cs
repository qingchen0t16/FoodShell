using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fire : MonoBehaviour
{
    private float tFirePosY = 0;    // 目标Y
    private bool activate = false;  // 是否激活
    public bool Activate { 
        get => activate;
        set {
            activate = value;
            if (activate)
            {
                StopAllCoroutines();    // 清除所有携程
                GI_PlayManager.Instance.FireNum += 25;
                Vector3 temp = GI_CardBarCtrl.Instance.GetFileTextPos();
                Activate_FlyAn(new Vector3(temp.x, temp.y, 0));
            }
        }
    }
    private bool isSky = false; // 来自天空

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
            return;
        if (GI_PlayManager.Instance.AutoCollect)    // 自动收集
        {
            if (isSky)
                transform.localPosition = new Vector3(transform.localPosition.x, tFirePosY, 0);
            Activate = true;
        }
        if (transform.localPosition.y <= tFirePosY && isSky && tFirePosY != -1) //  tFirePosY != -1是防止二次调用
        {
            tFirePosY = -1;
            if (!activate)
                Invoke("Destroy", 5);
        }
        if(isSky)
            transform.Translate(Vector3.down * Time.deltaTime);
    }

    /// <summary>
    /// 销毁自身
    /// </summary>
    private void Destroy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 天空阳光初始化
    /// </summary>
    public void SkyInit(float[] cFirePos, float tFirePosY)
    {
        isSky = true;
        this.tFirePosY = tFirePosY; // 目标位置
        transform.localPosition = new Vector3(cFirePos[0], cFirePos[1],0);  // 移动自身到初始位置
    }

    /// <summary>
    /// 创建位置
    /// </summary>
    /// <param name="cFirePos"></param>
    public void FoodInit(Vector2 cFirePos) {
        isSky = false;
        transform.localPosition = cFirePos;
        StartCoroutine(Card_DoJump());
    }
    public IEnumerator Card_DoJump()
    {
        Vector3 startPos = transform.position;
        int temp = Random.Range(0, 2) == 1 ? 1 : -1;
        float speed = 0;
        while (transform.position.y <= startPos.y + 1)
        {
            yield return new WaitForSeconds(0.001F);
            speed += 0.0005F;
            transform.Translate(new Vector3(0.01F * temp, 0.05F + speed, 0));
        }
        while (transform.position.y >= startPos.y - 0.3F)
        {
            yield return new WaitForSeconds(0.001F);
            speed += 0.0005F;
            transform.Translate(new Vector3(0.01F * temp, -0.05F - speed, 0));
        }
        if (!activate)
            Invoke("Destroy", 5);
    }

    /// <summary>
    /// 鼠标进入
    /// </summary>
    private void OnMouseEnter()
    {
        CollectFire();
    }

    /// <summary>
    /// 阳光被收集
    /// </summary>
    public void CollectFire()
    {
        if (activate)
            return;
        //AudioManager.Instance.PlayEFAudio(GIManager.Instance.gameSource.AuFireHover);
        Activate = true;
    }

    /// <summary>
    /// 飞向卡片栏
    /// </summary>
    private void Activate_FlyAn(Vector3 pos)
    {
        StartCoroutine(Activate_DoFly(pos));
    }
    private IEnumerator Activate_DoFly(Vector3 pos)
    {
        Vector3 direction = (pos - transform.position).normalized * 2.2F;
        while (Vector3.Distance(pos, transform.position) > 15F)
        {
            yield return new WaitForSeconds(0.005F);
            transform.Translate(direction);
        }
        Fire_Destroy();
    }

    /// <summary>
    /// 火苗销毁
    /// </summary>
    public void Fire_Destroy() {
        Destroy(gameObject);
    }
}
