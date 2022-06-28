using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B001 : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator an;
    private bool active;
    private float damage;   // ������

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(Vector2.right * 300F);
        active = false;
        damage = 20F;
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public void Init(Vector3 localPos)
    {
        transform.localPosition = localPos; // �ƶ���ָ��λ��
        active = false;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="coll"></param>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!active && coll.tag == "Mouse") {
            an.Play("Active");
            active = true;

            rb.velocity = Vector2.zero;
            coll.GetComponent<MouseBase>().Damage(damage);

           Invoke("Destroy",0.3F);
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
