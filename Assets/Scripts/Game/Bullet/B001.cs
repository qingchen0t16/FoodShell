using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B001 : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator an;
    private bool active;


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
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!active && coll.tag == "Mouse") {
            an.Play("Active");
            active = true;

            rb.velocity = Vector2.zero;

           Invoke("Destroy",0.3F);
        }
    }

    /// <summary>
    /// Ïú»Ù
    /// </summary>
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
