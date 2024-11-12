using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 키보드 연속입력을 감지하기 위한 클래스
class DoubleKeyPressDetection
{
    public KeyCode Key { get; private set; }

    // 한번 눌렀을경우
    public bool SinglePressed { get; private set; }

    // 두번눌렀을경우
    public bool DoublePressed { get; private set; }

    private bool  doublePressDetected;
    private float doublePressThreshold;
    private float lastKeyDownTime;


    // 입력 감지를 위한 초기설정함수, 키 값과 입력 유지값을 받아 설정한다. 기본값은 0.3초
    public DoubleKeyPressDetection(KeyCode key, float threshold = 0.3f)
    {
        this.Key = key;
        SinglePressed = false;
        DoublePressed = false;
        doublePressDetected = false;
        doublePressThreshold = threshold;
        lastKeyDownTime = 0f;
    }

    public void ChangeKey(KeyCode key)
    {
        this.Key = key;
    }
    public void ChangeThreshold(float seconds)
    {
        doublePressThreshold = seconds > 0f ? seconds : 0f;
    }

    // 키 값을 업데이트 하기위한 호출함수 monobehavior 클래스에서 호출된다.
    public void Update()
    {
        // 키를 입력받았을때 유지시간 보다 마지막으로 키를 누른시간이 작다면
        if (Input.GetKeyDown(Key))
        {
            // 연속 입력 판정을 활성화 하고
            doublePressDetected =
                (Time.time - lastKeyDownTime < doublePressThreshold);

            // 현재 시간을 마지막으로 키를 누른 시간으로 설정한다.
            lastKeyDownTime = Time.time;
        }

        // 키를 눌렀을때 연속 입력 판정 변수가 true라면
        if (Input.GetKey(Key))
        {
            if (doublePressDetected)
                DoublePressed = true;
            else
                SinglePressed = true;
        }
        else
        {
            // 아니라면 모둔 판정함수를 초기값으로 돌린다.
            doublePressDetected = false;
            DoublePressed = false;
            SinglePressed = false;
        }
    }

    // 실질적인 행동을 수행할 Invoke 구독 함수 위의 단일 입력 혹은 연속입력이 감지되었을때, 코드에서 작성한 부분을 수행한다.
    public void UpdateAction(Action singlePressAction, Action doublePressAction)
    {
        if(SinglePressed) singlePressAction?.Invoke();
        if(DoublePressed) doublePressAction?.Invoke();
    }
}

public class Move : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private Vector2 dir = Vector2.zero;
    [SerializeField]
    private GameObject ThrowPreFeb;

    private bool isSliding = false;
    private bool isStoped = true;

    private float moveSpeed = 150f;
    private float JumpForce = 10f;
    private bool isGrounded = true;

    private PlayerController playerController;

    Inventory inventory;

    // 입력할 특정키를 저장할 배열 keys
    private DoubleKeyPressDetection[] keys;

    private Collider2D collider1;
    private Collider2D collider2;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        playerController = GetComponent<PlayerController>();
        
        keys = new[]
        {
            new DoubleKeyPressDetection(KeyCode.A),
            new DoubleKeyPressDetection(KeyCode.D),
        };


        Collider2D[] colliders = GetComponents<Collider2D>();
        collider1 = colliders[0];
        collider2 = colliders[1];
        collider2.enabled = false;
    }

    // TODO
    // 스테미나 모두 소진시 스테미나를 소비하는 모든행동 수행할수 없게 수정
    void Update()
    {
        if (!playerController.StaminaEmpty && !playerController.isTalk)
        {
            // 마우스 왼쪽클릭을 눌렀을때, 현재 선택 되어있는 인벤토리에 아이템이 비어있지않다면,
            if (Input.GetMouseButtonDown(0) && inventory.transform.GetChild(inventory.curNum).GetChild(0).GetComponent<WeaphonInv>().fill)
            {
                Throw();
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            // 슬라이딩중 값이 변경되는것을 방지하기위한 if 문
            if (!isSliding)
            {
                // 키보드 연속입력 처리구문(Action함수를 사용해서 익명함수로 처리)
                for (int i = 0; i < keys.Length; i++)
                {
                    keys[i].Update();
                }

                keys[0].UpdateAction(() => moveSpeed = 150 , () => moveSpeed = 250);
                keys[1].UpdateAction(() => moveSpeed = 150 , () => moveSpeed = 250);
            }
        }
        else
        {
            moveSpeed = 150;
        }

        if (isSliding || moveSpeed <= 150 || dir.x == 0)
        {
            animator.SetBool("isRun", false);
        }
        else if (!isSliding && moveSpeed > 150)
        {
            animator.SetBool("isRun", true);
        }
        

        if (isGrounded)
        {
            animator.SetBool("isFalling", false);
        }
        if (!isGrounded)
        {
            animator.SetBool("isFalling", true);
        }
        

    }
    void FixedUpdate()
    {
        PMove();
        // 슬라이딩중이 아니고 뛰고있지않을떄.
        if (!isSliding && playerController.Stamina != 70f)
        {
            if ( moveSpeed <= 150 || isStoped)
                playerController.Stamina += 1f;
        }
        if (!isStoped)
            playerController.Stamina -= 0.5f;
    }

    void PMove()
    {
        // 오른쪽을 눌르고 있을때, 슬라이딩을 수행하고 뗄때 종료한다.
        if (Input.GetMouseButton(1) && !isSliding)
        {
            isSliding = true;
            animator.SetBool("isSliding",true);
            animator.SetTrigger("Tsliding");
            StartCoroutine(Sliding());
        }
        if (!Input.GetMouseButton(1))
        {
            animator.SetTrigger("noSliding");
            isSliding = false;
        }

        float h =  Input.GetAxisRaw("Horizontal");

        isStoped = h == 0 ? true : false;

        dir.x = h;
        dir.Normalize();
        rigidbody2D.velocity = new Vector2(dir.x * moveSpeed * Time.deltaTime, rigidbody2D.velocity.y);

        if (dir.x > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        else if (dir.x < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }

        if (isSliding || moveSpeed < 150 || dir.x == 0)
        {
            animator.SetBool("isWalk", false);
        }
        else if (!isSliding && moveSpeed <= 150)
        {
            animator.SetBool("isWalk", true);
        }
        
    }

    IEnumerator Sliding()
    {
        collider1.enabled = false;
        collider2.enabled = true;
        float originalspeed = moveSpeed;
        moveSpeed = 500f;
        while (moveSpeed > originalspeed * 0.1f)
        {
            playerController.Stamina -= 1f;
            if (playerController.StaminaEmpty || !Input.GetMouseButton(1))
            {
                break;
            }
            moveSpeed *= 0.7f;
            yield return new WaitForSeconds(0.3f);
        }
        moveSpeed = originalspeed;
        isSliding = false;
        animator.SetBool("isSliding", false);
        collider1.enabled = true;
        collider2.enabled = false;
    }

    void Jump()
    {
        animator.SetTrigger("Jump");
        isGrounded = false;
        playerController.Stamina -= 3f;
        rigidbody2D.AddForce(new Vector2(0, JumpForce) , ForceMode2D.Impulse);
    }

    void Throw()
    {
        playerController.Stamina -= 5f;
        // 마우스 위치 가져오기
        float x = Input.mousePosition.x - (Screen.width / 2);
        float y = Input.mousePosition.y - (Screen.height / 2);

        // 가져온 위치를 방향 벡터로 전환
        Vector2 dir = new Vector2(x, y);
        dir.Normalize();

        // 던질 아이템을 생성
        GameObject throwPrefeb = Instantiate(ThrowPreFeb);
        throwPrefeb.GetComponent<Weaphon>().playerThrow = true;
        // 생성후 Weaphon 스크립트에서 해당 아이템의 정보를 가져온다. (이미지, 데미지)
        throwPrefeb.GetComponent<Weaphon>().UpdateWeaphon();
        if (dir.x > 0)
        {
            throwPrefeb.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
        }
        else
        {
            throwPrefeb.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y);
        }
        dir.y += 0.1f;
        throwPrefeb.GetComponent<Rigidbody2D>().velocity = dir * 20;
        throwPrefeb.GetComponent<Rigidbody2D>().rotation += 1.0f;
        animator.SetTrigger("Throw");

        // 던진 무기의 정보에 접근한다.
        WeaphonData curWeaphon = throwPrefeb.GetComponent<Weaphon>().weaphonData;
        
        // 인벤토리에서 던진무기의 정보를 갱신한다. (개수를 감소시키고 0이되면 인벤토리에서 제거한다.)
        inventory.gsisThrow = true;
        inventory.UpdateInventory(curWeaphon);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Field"))
        {
            isGrounded = true;
        }
    }
}
