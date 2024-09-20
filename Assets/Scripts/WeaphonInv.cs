using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 인벤토리에 들어가있는 무기 정보를 저장하고 이미지를 갱신하기 위한 스크립트
public class WeaphonInv : MonoBehaviour
{
    public Sprite nullImage;
    private Image img;
    private int weaphonindex = 0;
    private bool canThrow = false;

    // 현재 보유하고있는 무기개수를 보여주는 getset프로퍼티
    // 무기개수가 0이되면 해당공간을 비운다.
    public int gsweaphonindex
    {
        get{ return weaphonindex;}
        set
        {
            weaphonindex = value;
            if (weaphonindex <= 0)
            {
                weaphonindex = 0;
                fill = false;
            }
        }
    }
    private WeaphonData weaphon;

    // 무기정보를 갱신하는 getset프로퍼티
    public WeaphonData setWeaphon
    {
        get{ return weaphon; }
        set
        {
            weaphon = value;
        }
    }

    // 무기종류를 구분하기위한, WeaphonId의 getset 프로퍼티
    public int getWId
    {
        get
        {
            return weaphon.Wcode;
        }
    }

    private bool isFilled = false;
    // 현재 인벤토리의 공간이 비어있는지 확인하기위한, getset프로퍼티
    public bool fill
    {
        get { return isFilled; }
        set 
        { 
            isFilled = value; 
            if (isFilled)
            {
                SwapImage();
                canThrow = true;
            }
            else
            {
                ResetImage();
                canThrow = false;
            }
        }
    }

    void SwapImage()
    {
        img.sprite = weaphon.Wicon;
    }

    void ResetImage()
    {
        img.sprite = nullImage;
    }

    void Awake()
    {
        img = GetComponent<Image>();    
        fill = false;
    }
}
