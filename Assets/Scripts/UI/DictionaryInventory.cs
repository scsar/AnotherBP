using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryInventory : MonoBehaviour
{
    // 아이템의 아이템 코드와 데이터 값을 저장하는 딕셔너리
    private static Dictionary<int, ItemData> DInventory = new Dictionary<int, ItemData>();
    public GameObject panelPrefeb;
    public GameObject itemContent;
    
    // 아이템 코드를 입력받았을때 해당 아이템 데이터를 반환 받을수있게 한다.
    public ItemData GetDictionary(int ICode)
    {
        return DInventory[ICode];
        UpdateDictionary(ICode);
    }

    public bool isContain(int ICode)
    {
        return DInventory.ContainsKey(ICode);
    }

    public void setDictionary(int ICode, ItemData itemData)
    {
        itemData.Gain = true;
        itemData.ICount += 1;
        DInventory.Add(ICode, itemData);
        UpdateDictionary(ICode);
    }

    // TODO
    
    void UpdateDictionary(int ICode)
    {
        if (DInventory != null)
        {
            GameObject panel = Instantiate(panelPrefeb);
            panel.transform.parent = itemContent.transform;
            panel.GetComponent<Panel>().UpdatePanel(DInventory[ICode]);
        }
    }
}
