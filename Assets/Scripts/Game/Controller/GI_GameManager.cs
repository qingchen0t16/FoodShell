using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GI_GameManager : MonoBehaviour
{
    public static GI_GameManager Instance;
    public GI_Conf Conf { get; private set; }   // Τ¤ΦΖΜε¶Ρ
    public GameObject FoodBox;
    public GameObject TopSpanBox;

    private void Awake()
    {
        Instance = this;

        Conf = Resources.Load<GI_Conf>("Game/GameingConf");
        FoodBox = GameObject.Find("Game/FoodBox");
        TopSpanBox = GameObject.Find("UI/TopSpanBox");
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
