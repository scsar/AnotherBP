using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInteraction
{
    Rigidbody2D _pRigidbody = null;
    public void InterAction()
    {
        // TODO
        // 상호작용시 사다리에 붙어있을수 있게 하고, 위아래(w,s) 입력시 위아래로 움직일수있도록 한다.

        // 1) 일시적으로 플레이어 Rigidbody에 접근해서 중력끄고 조작.
        // rigidbody 컴포넌트 접근
        // 특정 키를 입력받았을때, 플레이어의 velocity조정 (이값은 Labber의 Update()를 통해서 컨트롤 )
        // 2) 중력끄는것 없이 velocity.y 값만을 조정해서, 위아래 움직임 구현.

       _pRigidbody = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_pRigidbody != null)
        {
            _pRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            if (Input.GetKey(KeyCode.W))
            {
                _pRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                _pRigidbody.velocity = new Vector2(_pRigidbody.velocity.x, 200f * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _pRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                _pRigidbody.velocity = new Vector2(_pRigidbody.velocity.x, -200f * Time.deltaTime);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") & _pRigidbody != null)
        {
            _pRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _pRigidbody = null;
        }
    }
}
