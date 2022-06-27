using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameingConf",menuName = "GameingConf")]
public class GI_Conf : ScriptableObject
{
    [Tooltip("����")]
    public GameObject Fire;
    [Tooltip("ʳ��")]
    public List<GameObject> Foods;
    [Tooltip("ʳ�￨Ƭ")]
    public List<Sprite> FoodsCard;
}
