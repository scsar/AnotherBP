using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Summon : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target; 
    [SerializeField]
    private float stopDistance;
    [SerializeField]
    private float targetUpdateInterval = 1.0f; // 목표 갱신 주기
    private float timeSinceLastUpdate = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        UpdateTarget();
    }

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        // 일정 시간마다 목표를 갱신하여 가장 가까운 몬스터를 추적
        if (timeSinceLastUpdate >= targetUpdateInterval)
        {
            UpdateTarget();
            timeSinceLastUpdate = 0f;
        }

        // 현재 목표가 있을 때만 NavMeshAgent의 목적지를 설정
        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(target.position, transform.position);

            if (distanceToTarget < stopDistance)
            {
                agent.destination = target.position;
            }
        }
    }

    void FixedUpdate()
    {
        if (target != null)
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
    }

    // 가까운 몬스터를 찾아서 target으로 설정하는 함수
    private void UpdateTarget()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (GameObject monster in monsters)
        {
            float distance = Vector2.Distance(transform.position, monster.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = monster.transform;
            }
        }

        target = closestTarget;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Creature creature = other.GetComponent<Creature>();
        if (creature != null)
        {
            creature.C_hp -= 2f;
            Destroy(gameObject); // 충돌 시 소환수 파괴
        }
    }
}
