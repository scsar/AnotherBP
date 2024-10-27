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
    private float summonDuration = 5f;
    private float summonInterval = 1f; 
    [SerializeField]
    private GameObject SummonCreature;

    private bool guardActive;
    private bool returnActive;

    void Awake()
    {
        _wRigidbody = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sprite = weaphonData.Wicon;
        playerThrow = false;
        guardActive = false;
        returnActive = false;
    }

    public void UpdateWeaphon()
    {
        // 무기 데이터를 현재 인벤토리에 선택되어있는 무기의 정보로 갱신한다. 
        weaphonData = GameObject.FindWithTag("Inventory").GetComponent<Inventory>().curWeaphon;
        damage = weaphonData.Wdamage;
        // 무기의 스프라이트(이미지)를 변경한다.
        GetComponent<SpriteRenderer>().sprite = weaphonData.Wicon;

        if (weaphonData.WAbilityCode == (int)WeaphonAbility.Guard)
        {
            guardActive = true;
            StartCoroutine(A_Guard());
        }
        if (weaphonData.WAbilityCode == (int)WeaphonAbility.Return)
        {
            StartCoroutine(A_Return());
        }

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
            Ability(other.gameObject.GetComponent<Creature>());

            Vector2 direction = (transform.position - other.transform.position).normalized;
            _wRigidbody.AddForce(direction * 10, ForceMode2D.Impulse);
            _wRigidbody.velocity = Vector2.zero;
        }

        if (other.gameObject.CompareTag("Projectile") && guardActive)
        {
            Destroy(other.gameObject);
        }
    }

    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.x > 1)
        {
            _wRigidbody.rotation -= 5.0f;
        }


        if (returnActive)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, 30 * Time.deltaTime);

            float distanceToPlayer = Mathf.Abs(Vector2.Distance(player.position, transform.position));

            if ( distanceToPlayer > 5)
            {
                gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            }
            else 
            {
                gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
            }

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
    void A_Summon(float power)
    {
        StartCoroutine(SummonRoutine(power));
    }

    private IEnumerator SummonRoutine(float power)
    {
        GameObject summon = Instantiate(SummonEffectPrefab, transform.position, Quaternion.identity);
        float timer = 0;
        summonDuration *= power;
        Destroy(summon, summonDuration);
        // summonDuration 동안 summonInterval 간격으로 소환수를 생성
        while (timer <= summonDuration)
        {
            GameObject summonCreature = Instantiate(SummonCreature, summon.transform.position, Quaternion.identity);
            summonCreature.transform.SetParent(summon.transform, false);
            summonCreature.transform.position = summon.transform.position;
            yield return new WaitForSeconds(summonInterval);
            timer += summonInterval;
        }
    }
    void A_FishingStaff(){}

    IEnumerator A_Guard()
    {
        yield return new WaitForSeconds(0.1f);
        _wRigidbody.velocity = Vector2.zero;
        _wRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        float timer = 0f;
        while (timer < 5f)
        {
            timer += Time.deltaTime;
            transform.Rotate(0f, 0f, 10f);
            yield return null;
        }
        _wRigidbody.constraints = RigidbodyConstraints2D.None;
        guardActive = false;
    }
    IEnumerator A_Return()
    {
        yield return new WaitForSeconds(2f);
        returnActive = true;        
    }

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
                A_Summon(A_power);
                break;
            case WeaphonAbility.FishingStaff :
                A_FishingStaff();
                break;
        }
    }
}
