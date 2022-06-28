using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameingConf",menuName = "GameingConf")]
public class GI_Conf : ScriptableObject
{
    [Tooltip("火苗")]
    public GameObject Fire;
    [Tooltip("食物")]
    public List<GameObject> Foods;
    [Tooltip("老鼠")]
    public List<GameObject> Mouses;
    [Tooltip("食物卡片")]
    public List<Sprite> FoodsCard;
    [Tooltip("子弹")]
    public List<GameObject> Bullets;
}
