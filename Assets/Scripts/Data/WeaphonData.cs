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
    private float weaphonDamage = 0;
    public float Wdamage
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
    private int weaphonGrade;
    public int Wgrade
    {
        get { return weaphonGrade; }
    }

    [SerializeField]
    private int AbilityCode;
    public int WAbilityCode
    {
        get { return AbilityCode; }
    }
}
