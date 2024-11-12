using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    // 이벤트 관리를위한 핸들러
    public delegate void DialogueEndHandler();
    public event DialogueEndHandler OnDialogueEndEvent;

    [SerializeField] private GameObject talkImage, nameImage;
    [SerializeField] private TalkData[] talkDatas;
    private TextMeshProUGUI talkText, nameText;
    [HideInInspector]
    public bool isTalk;
    private bool isNpc;
    private GameObject talkNpc;
    private string typeText = "";
    [HideInInspector] 
    public string ename;

    
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private DictionaryInventory dictionaryInventory;

    [SerializeField]
    private GameObject Staminabar;
    [SerializeField]
    private GameObject Hpbar;

    private bool isDead;


    // 상호작용할 정보를 저장할 게임 오브젝트
    private GameObject InteractionObject = null;

    private float _stamina;
    public float Stamina
    {
        get { return _stamina;}
        set 
        { 
            _stamina = value;
            if (_stamina > 70f)
            {
                _stamina = 70f;
            }
            if (_stamina < 0)
            {
                _stamina = 0;
            }
            Staminabar.GetComponent<Image>().fillAmount = _stamina/70f;
        }

    }
    public bool StaminaEmpty
    {
        get { return _stamina <= 0; }
    }
    private float _hp;
    public float Hp
    {
        get { return _hp; }
        set 
        {
            _hp = value; 
            Hpbar.GetComponent<Image>().fillAmount = _hp/100f;
            if (_hp <= 0)
            {
                _hp = 0;
                isDead = true;
                SceneManager.LoadScene(0);
            }
        }
    }
    void Awake()
    {
        Stamina = 70f;
        Hp = 100f;
        isDead = false;

        isTalk = false;
        isNpc = false;
        talkImage.SetActive(false);
		nameImage.SetActive(false);
		talkText = talkImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		nameText = nameImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // 상호작용할 오브젝트가 존재하고 상호작용 키를 눌렀을때
        if (InteractionObject != null && Input.GetKeyDown(KeyCode.E))
        {
            // 어떤 오브젝트인지는 상관하지않고, InterAction함수를 수행한다.
            InteractionObject.GetComponent<IInteraction>().InterAction();
        }

        if (Input.GetKeyDown(KeyCode.E) && isNpc)
        {
            Inter(talkNpc);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 접촉한 오브젝트가 무기인경우 인벤토리에 해당 무기의 데이터를 저장한다.
        if (collision.gameObject.CompareTag("Weaphon"))
            inventory.UpdateInventory(collision.gameObject.GetComponent<Weaphon>().weaphonData);
    }

    // Trigger오브젝트와 접촉했을때 (상호작용이 가능한 오브젝트와 접촉했을때.)
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Item") && collider.gameObject.GetComponent<Item>() != null)
        {
            InteractionObject = collider.gameObject;
            // 아이템 태그를 가지고 있지만, 상호작용하는 오브젝트가 아닌경우
            // item스크립트가 있는지 검사하고 있다면 코드 실행
            if (InteractionObject != null && InteractionObject.GetComponent<Item>() != null)
            {
                // 딕셔너리 인벤토리에 값 전달.
                UpdateItemData(InteractionObject.GetComponent<Item>().itemData);
                InteractionObject = null;
            }
        }
    }


    void OnTriggerStay2D(Collider2D collider)
    {
        // 아이템(상자, ) 이면
        if (collider.gameObject.CompareTag("Item"))
        {
            // 해당오브젝트의 정보를 저장
            InteractionObject = collider.gameObject;
        }
        if (collider.gameObject.CompareTag("NPC") && !isTalk)
        {
            isNpc = true;
            talkNpc = collider.gameObject;
            // Inter(collider.gameObject);
        }
    }

    void OnTriggerExit2D (Collider2D collider)
    {
        // 아이템과 접촉해있지 않은경우 다시 해당값을 해제한다.
        InteractionObject = null;
        
        isNpc = false;
        talkNpc = null;
    }

    void UpdateItemData(ItemData itemdata)
    {   
        // 딕셔너리에 해당값을 추가한다.
        // 아이템을 획득한적이 있는 경우
        if (dictionaryInventory.isContain(itemdata.iCode))
        {
            dictionaryInventory.GetDictionary(itemdata.iCode).ICount += 1;
        }
        else
        {
            // 아이템을 최초획득한 경우
            dictionaryInventory.setDictionary(itemdata.iCode, itemdata);
        }

    }

    public void Inter(GameObject obj)
    {
        talkDatas = obj.GetComponent<Dialogue>().GetObjectDialogue();
        if (talkDatas != null)
        {
            ename = obj.transform.GetComponent<Dialogue>().geteventname;
            StartCoroutine(ShowNextDialogue());
        }
    }

    IEnumerator ShowNextDialogue()
	{
		isTalk = true;
		talkImage.SetActive(true);
		nameImage.SetActive(true);
		yield return new WaitForSeconds(0.001f);
		for (int i = 0; i < talkDatas.Length; i++)
		{
			nameText.text = talkDatas[i].name;
			foreach (string context in talkDatas[i].contexts)
			{
                // 타이핑 효과음을 위한 오디오 설정
				// audio.Play();
				yield return new WaitForSeconds(0.05f);
				for (int j = 0; j < context.Length; j++)
				{
					typeText += context[j];
					talkText.text = typeText;
					yield return null;
				}
				typeText = "";
				// audio.Stop();
				yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
			}
		}
		talkImage.SetActive(false);
		nameImage.SetActive(false);
		isTalk = false;
		OnDialogueEnd();
	}

	void OnDialogueEnd()
	{
		// 이벤트 트리거
		if (OnDialogueEndEvent != null)
		{
			OnDialogueEndEvent();
		}
	}


}
