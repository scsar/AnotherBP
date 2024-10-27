using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaphonAbility
{
    None,
    Blood,
    Explosion,
    Lightning,
    Summon,
    FishingStaff,
    Guard,
    Return
};

public enum Rarity
{
    N = 1,
    R = 2,
    SR = 3
}

public class Weaphon : MonoBehaviour
{
    // 해당무기의 정보를 저장할 데이터 변수
    public WeaphonData weaphonData;

    [HideInInspector]
    public float damage;

    [HideInInspector]
    public bool playerThrow;

    Rigidbody2D _wRigidbody;

    [SerializeField]
    private GameObject explosionEffectPrefab;
    private float explosionRadius = 3.0f;
    private float explosionForce = 1.5f;

    [SerializeField]
    private GameObject lightningEffectPrefab;
    private float chainRadius = 5.0f;
    private float maxchain;
    private float lightningDamage;

    [SerializeField]
    private GameObject SummonEffectPrefab;

    void Awake()
    {
        _wRigidbody = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sprite = weaphonData.Wicon;

        damage = weaphonData.Wdamage;
        playerThrow = false;
    }

    public void UpdateWeaphon()
    {
        // 무기 데이터를 현재 인벤토리에 선택되어있는 무기의 정보로 갱신한다. 
        weaphonData = GameObject.FindWithTag("Inventory").GetComponent<Inventory>().curWeaphon;
        // 무기의 스프라이트(이미지)를 변경한다.
        GetComponent<SpriteRenderer>().sprite = weaphonData.Wicon;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 충돌했을떄, 파괴하면서 획득 처리
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Monster") && playerThrow)
        {
            playerThrow = false;
            Ability(other.gameObject.GetComponent<Creature>());

            Vector2 direction = (transform.position - other.transform.position).normalized;
            _wRigidbody.AddForce(direction * 10, ForceMode2D.Impulse);
            _wRigidbody.velocity = Vector2.zero;

        }
    }

    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.x > 1)
        {
            _wRigidbody.rotation -= 5.0f;
        }
    }
    

    private float power;
    float SetGrade()
    {    
        Rarity rare = (Rarity)weaphonData.Wgrade;

        switch(rare)
        {
            case Rarity.N :
                power = 1;
                break;
            case Rarity.R :
                power = 2;
                break;
            case Rarity.SR :
                power = 3;
                break;
        }    
        return power;
    }


    void A_Explosion(float power)
    {
        // 완드류는 무기 투척데ㅣ지는 동일하고 마법값만 증가
        float exDamage = 5 * power;
        GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        explosion.transform.localScale *= power;

        Destroy(explosion, 1f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in colliders)
        {
            Creature creature = null;
            if (hit.isTrigger)
            {
                creature = hit.GetComponent<Creature>();
            }
            if (creature != null)
            {
                creature.C_hp -= exDamage;
                creature = null;
            }

            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (rb.position - new Vector2(transform.position.x, transform.position.y)).normalized;
                rb.AddForce(direction * explosionForce * power, ForceMode2D.Impulse);
            }
        }
    }
    void A_Lightning(float power, Transform target)
    {
        maxchain = 3 * power;
        lightningDamage = 1f * power;
        StartCoroutine(ChainLightning(target, maxchain));
    }

    IEnumerator ChainLightning(Transform target, float maxchain)
    {
        if (maxchain < 0 || target == null)
        {
            yield break;
        }

        GameObject lightning = Instantiate(lightningEffectPrefab, target.position, Quaternion.identity);
        Destroy(lightning, 1f);

        Creature creature = target.GetComponent<Creature>();
        if (creature != null)
        {
            creature.C_hp -= lightningDamage;
        }
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(target.position, chainRadius);
        Transform nextTarget = null;

        foreach (Collider2D collider in nearbyEnemies)
        {
            // 자신을 제외한 다음 대상 찾기
            if (collider.transform != target && collider.GetComponent<Creature>() != null)
            {
                nextTarget = collider.transform;
                break;
            }
        }

        // 다음 타겟이 있다면 재귀적으로 ChainLightning 호출
        if (nextTarget != null)
        {
            yield return new WaitForSeconds(0.2f); // 연쇄 딜레이
            StartCoroutine(ChainLightning(nextTarget, maxchain - 1));
        }

    }
    void A_Summon()
    {
        GameObject Summon = Instantiate(SummonEffectPrefab, transform.position, Quaternion.identity);
        Destroy(Summon , 5f);
    }
    void A_FishingStaff(){}
    void A_Guard(){}
    void A_Return(){}

    void Ability(Creature creature)
    {
        WeaphonAbility Code = (WeaphonAbility)weaphonData.WAbilityCode;
        float A_power = SetGrade();
        switch(Code)
        {
            case WeaphonAbility.None :
                break;
            case WeaphonAbility.Blood :
                StartCoroutine(creature.Df_blood(A_power));
                break;
            case WeaphonAbility.Explosion :
                A_Explosion(A_power);
                break;
            case WeaphonAbility.Lightning :
                A_Lightning(A_power, creature.transform);
                break;
            case WeaphonAbility.Summon :
                A_Summon();
                break;
            case WeaphonAbility.FishingStaff :
                A_FishingStaff();
                break;
            case WeaphonAbility.Guard :
                A_Guard();
                break;
            case WeaphonAbility.Return :
                A_Return();
                break;
        }
    }

}
