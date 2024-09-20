using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [HideInInspector]
    public WeaphonData curWeaphon;

    // 현재 인벤토리를 위치를 표시하는 index
    private int invNum;
    public int curNum
    {
        get { return invNum; }
    }
    // 현재 인벤토리 위치를 표시하기위한 이미지 게임오브젝트
    private GameObject curInventory;
    // 각 공간의 위치를 저장하기위한 게임오브젝트
    private GameObject[] inventories;
    private bool isThrow;
    // 현재 공간에 아이템이 존재하면 Thorw함수를 실행할수있게 하는 getset 프로퍼티
    public bool gsisThrow
    {
        get 
        {
            return isThrow;
        }
        set
        {
            isThrow = value;
        }
    }
    void Awake()
    {
        // 비어있으므로 시작할때는 던질수없게 false 로 초기화
        isThrow = false;
        inventories = new GameObject[4];
        curInventory = transform.GetChild(4).gameObject;
        for (int i = 0; i < 4; i++)
        {
            // 인벤토리의 각 공간을 매핑 시켜준다.
            inventories[i] = transform.GetChild(i).gameObject;
        }
        // 이후 현재 공간에있는 index UI를 비활성화 한다.
        for (int i = 0; i < inventories.Length; i++)
            inventories[i].transform.GetChild(1).gameObject.SetActive(false);

        SelectInventory(0); 
    }

    void Update()
    {
        // 각 숫자키를 눌렀을때, 인벤토리를 선택하는 invNum변수를 변경한다.
        if (Input.GetKeyDown(KeyCode.Alpha1))
            invNum = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            invNum = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            invNum = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            invNum = 3;

        SelectInventory(invNum);      
    }

    // 인벤토리 선택 함수
    void SelectInventory(int invNum)
    {
        // 현재 선택되어있는 위치와 선택하고자 하는 오브젝트의 위치가 다를경우에만 위치를 갱신한다.
        if (curInventory.transform.position != inventories[invNum].transform.position)
            curInventory.transform.position = inventories[invNum].transform.position;
        
        // 현재 공간에 저장되어있는 무기 데이터를 변수를 새로 생성해 저장
        WeaphonData selectWeaphon = inventories[invNum].transform.GetChild(0).GetComponent<WeaphonInv>().setWeaphon;
        // 이후 현재 무기 데이터에 넘겨준다.
        curWeaphon = selectWeaphon;
    }

    public void UpdateInventory(WeaphonData weaphonData)
    {
        // isThrow가 false일때(무기 획득)
        if (!isThrow)
        {
            // 아이템 획득시 가방에 빈공간 or 중복값 있는지 검사
            for (int i = 0; i < inventories.Length; i++)
            {
                // 인벤토리 창 변수
                WeaphonInv Inv = inventories[i].transform.GetChild(0).GetComponent<WeaphonInv>();
                // 인벤토리 인덱스 변수
                GameObject extra = inventories[i].transform.GetChild(1).gameObject;
                // 인벤토리가 비어있지 않은경우
                if (Inv.fill)
                {
                    // 인벤토리에 해당 아이템이 존재하는경우 (서로의 WeaphonId가 일치하는 경우)
                    if (Inv.getWId == weaphonData.Wcode)
                    {
                        // index UI를 활성화 하고 + 1을 수행한다.
                        extra.SetActive(true);
                        Inv.gsweaphonindex += 1;
                        // 이후 텍스트 갱신 후 함수 종료
                        extra.GetComponent<TextMeshProUGUI>().text = Inv.gsweaphonindex.ToString();    
                        return;
                    }
                    // 만약 같은 무기가 아닌경우 다음 공간과 비교한다.
                    continue;
                }
                else
                {
                    // 공간이있다면 그곳을 true로 변경하고 함수 반환
                    Inv.setWeaphon = weaphonData;
                    Inv.fill = true;
                    extra.SetActive(true);
                    Inv.gsweaphonindex += 1;
                    extra.GetComponent<TextMeshProUGUI>().text = Inv.gsweaphonindex.ToString();    
                    return;
                }
            }
            // 공간이 없다면 메세지 출력
            Debug.Log("가방 공간부족");
        }
        else // isThrow가 true일때, (무기 사용)
        {
            // 무기 데이터를 가져온후 인덱스를 감소시킨다.
            WeaphonInv inv = inventories[invNum].transform.GetChild(0).GetComponent<WeaphonInv>();
            inv.gsweaphonindex -= 1;
            GameObject extra = inventories[invNum].transform.GetChild(1).gameObject;
            extra.GetComponent<TextMeshProUGUI>().text = inv.gsweaphonindex.ToString();
            if (inv.gsweaphonindex <= 0)
                extra.SetActive(false);
            isThrow = false;
        }
    }
}
