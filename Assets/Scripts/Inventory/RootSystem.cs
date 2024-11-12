using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefeb;
    [SerializeField]
    private GameObject weaphonPrefeb;

    [SerializeField]
    private List<ItemData> itemList;

    [SerializeField]
    private List<WeaphonData> weaphonList;

    private Dictionary<int, ItemData> itemDictionary = new Dictionary<int, ItemData>();

    private Dictionary<int, WeaphonData> weaphonDictionary = new Dictionary<int, WeaphonData>();

    void Awake()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemDictionary.Add(itemList[i].iCode, itemList[i]);
        }
        for (int i = 0; i < weaphonList.Count; i++)
        {
            weaphonDictionary.Add(weaphonList[i].Wcode, weaphonList[i]);
        }
    }

    // 고려해야할것
    // 몇개는 장비로, 몇개는 아이템으로 생성해야하는데
    // 이를 어떤기준으로 나눌것인지.

    // itemcategory 와 weaphoncategory에는 각각 넣고자 하는 아이템의 식별코드를 작성한다.
    public void RandomRoot(int itemMaxvalue, List<int> itemcategory, List<int> weaphoncategory, Transform target)
    {
        int value = Random.Range(1, itemMaxvalue + 1);
        if (value < 0)
            value = 0;

        Debug.Log(value);
        int wvalue;
        // 아이템 코드값을 받아와서 해당값으로 아이템에 대한 데이터를 가져온다.
        if (itemcategory.Count < 0)
        {
            wvalue = value % 3;
        }
        else
        {
            wvalue = value;
        }

        if (itemcategory.Count > 0)
        {
            for (int i = 0; i < value - wvalue; i++)
            {
                int itemidx = Random.Range(0, itemcategory.Count);
                itemPrefeb.GetComponent<Item>().itemData = itemDictionary[itemcategory[itemidx]];
                GameObject item = Instantiate(itemPrefeb);
                item.transform.position = target.position;
                StartCoroutine(WaitForGenerate(item));
                item.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
            }
        }
        
        if (weaphoncategory.Count > 0)
        {
            for (int i = 0; i < wvalue; i++)
            {
                int weaphonidx = Random.Range(0, weaphoncategory.Count);
                weaphonPrefeb.GetComponent<Weaphon>().weaphonData = weaphonDictionary[weaphoncategory[weaphonidx]];
                GameObject weaphon = Instantiate(weaphonPrefeb);
                weaphon.transform.position = target.position;
                StartCoroutine(WaitForGenerate(weaphon));
                weaphon.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 7f), ForceMode2D.Impulse);
            }
        }
    }

    // 오브젝트가 생성된 직후에는 해당기능을 통해, 획득할수없게 하고. 이후 잠깐의 시간이 지난후 획득하수있게 설정
    IEnumerator WaitForGenerate(GameObject obj)
    {
        obj.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        obj.GetComponent<CircleCollider2D>().enabled = true;
    }

    public void GiveWeaphon(int WCode)
    {
        for (int i = 0; i < 10; i++)
        {
            weaphonPrefeb.GetComponent<Weaphon>().weaphonData = weaphonDictionary[WCode];
            GameObject weaphon = Instantiate(weaphonPrefeb);
            weaphon.transform.position = GameObject.FindWithTag("Player").transform.position;
            StartCoroutine(WaitForGenerate(weaphon));
            weaphon.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 7f), ForceMode2D.Impulse);
        }
    }
}
