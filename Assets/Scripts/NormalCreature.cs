using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCreature : MonoBehaviour
{
    public Transform target;

    private bool isAttack;

    private float attackdistance = 1f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= attackdistance && !isAttack)
        {
            isAttack = true;
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(2f);
        isAttack = false;
    }

    void OnTriggerenter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isAttack)
        {
            target.GetComponent<PlayerController>().Hp -= 2f;
        }
    }
}
