using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item", order = int.MaxValue)]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private string itemName;
    public string iName
    {
        get { return itemName; }
    }
    [SerializeField]
    private Sprite itemIcon;
    public Sprite iIcon
    {
        get { return itemIcon; }
    }
    [SerializeField]
    private int itemCode;
    public int iCode
    {
        get { return itemCode; }
    }
    [SerializeField]
    private string itemExplain;
    public string iExplain
    {
        get { return itemExplain; }
    }

    private bool isGain = false;
    public bool Gain
    {
        get { return isGain; }
        set { isGain = value;}
    }

    private int itemCount = 0;
    public int ICount
    {
        get { return itemCount; }
        set { itemCount = value; }
    }
}
