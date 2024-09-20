using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject dictionaryInventory;

    private GameObject itemInventory, weaphonInventory, extraInventory;
    private bool isActive;
    private ItemData itemData;
    // 아이템 값과 개수를 저장하는 배열

    public static GameManager Instance = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        isActive = false;
        itemInventory = dictionaryInventory.transform.GetChild(0).gameObject;
        weaphonInventory = dictionaryInventory.transform.GetChild(1).gameObject;
        extraInventory = dictionaryInventory.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isActive = !isActive;
            dictionaryInventory.SetActive(isActive);
            ActiveItemInventory();
        }
    }

    public void ActiveItemInventory()
    {
        itemInventory.SetActive(isActive);
        weaphonInventory.SetActive(!isActive);
        extraInventory.SetActive(!isActive);
    }

    public void ActiveWeaphonInventory()
    {
        weaphonInventory.SetActive(isActive);
        itemInventory.SetActive(!isActive);
        extraInventory.SetActive(!isActive);
    }

    public void ActiveExtraInventory()
    {
        extraInventory.SetActive(isActive);
        itemInventory.SetActive(!isActive);
        weaphonInventory.SetActive(!isActive);
    }

}
