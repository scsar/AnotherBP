using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Panel : MonoBehaviour
{
    private ItemData item = null;
    public void UpdatePanel(ItemData itemdata)
    {
        if (item == null)
            item = itemdata;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.iName;
        transform.GetChild(1).GetComponent<Image>().sprite = item.iIcon;
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.ICount.ToString();
    }

    void FixedUpdate()
    {
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.ICount.ToString();
    }
}
