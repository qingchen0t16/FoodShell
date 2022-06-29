using Assets.Scripts.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSClientManager : MonoBehaviour
{
    public static FSClientManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FSClient.Instance.Open("192.168.31.84", 7788);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
