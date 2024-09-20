using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaphon : MonoBehaviour
{
    // 해당무기의 정보를 저장할 데이터 변수
    public WeaphonData weaphonData;

    Rigidbody2D _wRigidbody;

    void Awake()
    {
        _wRigidbody = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sprite = weaphonData.Wicon;
    }

    public void UpdateWeaphon()
    {
        // 무기 데이터를 현재 인벤토리에 선택되어있는 무기의 정보로 갱신한다. 
        weaphonData = GameObject.FindWithTag("Inventory").GetComponent<Inventory>().curWeaphon;
        // 무기의 스프라이트(이미지)를 변경한다.
        GetComponent<SpriteRenderer>().sprite = weaphonData.Wicon;
    }

    // 무기가 벽, 몬스터에 충돌했을때 5초후 무기가 파괴 된다.
    IEnumerator WeaphonHit()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 충돌했을떄, 파괴하면서 획득 처리
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Monster") && collision.gameObject.CompareTag("Field"))
        {
            // 몬스터 or 맵에 부딪칠 경우
            StartCoroutine(WeaphonHit());
        }
    }

    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.x > 1)
        {
            _wRigidbody.rotation -= 5.0f;
        }
    }
}
