using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B001 : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(Vector2.right * 300F);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
