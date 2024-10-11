using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weaphon", menuName = "Scriptable Object/Weaphon", order = int.MaxValue)]
public class WeaphonData : ScriptableObject
{
    // 무기 데이터를 규정하는 ScriptableObject
    [SerializeField]
    private string weaphonName;
    public string Wname
    {
        get { return weaphonName; }
    }
    [SerializeField]
    private Sprite weaphonIcon;
    public Sprite Wicon
    {
        get { return weaphonIcon; }
    }
    [SerializeField]
    private int weaphonDamage;
    public int Wdamage
    {
        get { return weaphonDamage; }
    }
    [SerializeField]
    private int weaphonCode;
    public int Wcode
    {
        get { return weaphonCode; }
    }

    [SerializeField]
    private string weaphonGrade;
    public string Wgrade
    {
        get { return weaphonGrade; }
    }
}
