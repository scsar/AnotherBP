using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creature", menuName = "Scriptable Object/Creature", order = int.MaxValue)]
public class CreatureData : ScriptableObject
{
    [SerializeField]
    private string CreatureName;
    public string CName
    {
        get { return CreatureName; }
    }

    [SerializeField]
    private float MaxHp;
    private float CreatureHp;
    public float CHP
    {
        get { return CreatureHp; }
    }


    [SerializeField]
    private Sprite CreatureImage;
    public Sprite CImage
    {
        get { return CreatureImage; }
    }

    
    [SerializeField]
    private int CreatureCode;
    public int CCode
    {
        get { return CreatureCode; }
    }
}
