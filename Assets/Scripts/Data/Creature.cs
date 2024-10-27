using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
    public CreatureData creature;
    
    private NavMeshAgent agent;
    private Transform target; 
    [SerializeField]
    private float stopDistance;

    private bool isDead;
    private float hp;
    public float C_hp
    {
        get { return hp;}
        set 
        {
            hp = value;

            if (hp <= 0)
            {
                hp = 0;
                isDead = true;
            }
        }
    }

    public GameObject HP_prefeb;
    private GameObject Hp_bar;
    private Image Cur_HP;
    private RectTransform Hp_barRT;
    private Transform canvas;

    public GameObject bloodEffect;
    

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
        hp = creature.CMaxhp;
        Hp_bar = Instantiate(HP_prefeb, canvas.transform);
        Hp_barRT = Hp_bar.GetComponent<RectTransform>();
        Cur_HP = Hp_bar.transform.GetChild(0).GetComponent<Image>();


        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GetComponent<SpriteRenderer>().sprite = creature.CImage;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();     

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 0.7f, 0));
        Hp_barRT.position = _hpBarPos;

        Cur_HP.fillAmount = hp/creature.CMaxhp;

        if (Vector2.Distance(target.position, transform.position) < stopDistance)
        {
            agent.destination = target.transform.position;
        }
    }

    void FixedUpdate()
    {
        if (target.position.x - transform.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weaphon") && other.GetComponent<Weaphon>().playerThrow)
        {
            Weaphon weaphon = other.gameObject.GetComponent<Weaphon>();
            C_hp -= weaphon.damage;

            weaphon.playerThrow = false;
        }
    }

    public IEnumerator Df_blood(float power)
    {
        for (int i = 0; i < 5; i++ )
        {
            GameObject bl_Effect = Instantiate(bloodEffect);
            bl_Effect.transform.position = transform.position;
            Destroy(bl_Effect, 1f);
            C_hp -= 0.5f * power;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
