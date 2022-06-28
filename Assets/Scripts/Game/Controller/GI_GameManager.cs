using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GI_GameManager : MonoBehaviour
{
    public static GI_GameManager Instance;
    public GI_Conf Conf { get; private set; }   // Τ¤ΦΖΜε¶Ρ
    public GameObject FoodBox;
    public GameObject TopSpanBox;
    public GameObject BulletBox;
    public GameObject MousesBox;

    private void Awake()
    {
        Instance = this;

        Conf = Resources.Load<GI_Conf>("Game/GameingConf");
        FoodBox = GameObject.Find("Game/FoodBox");
        TopSpanBox = GameObject.Find("UI/TopSpanBox");
        BulletBox = GameObject.Find("Game/BulletBox");
        MousesBox = GameObject.Find("Game/MousesBox");
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject
            .Instantiate(Conf.Mouses[0], Vector3.zero, Quaternion.identity, MousesBox.transform)
            .GetComponent<MouseBase>()
            .Init((int)GI_GridManager.Instance.GetGridPointByMouse().Point.y);
        }
    }
}
