using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Creature : MonoBehaviour
{
    public CreatureData creature;
    
    private NavMeshAgent agent;
    private Transform target; 
    [SerializeField]
    private float stopDistance;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GetComponent<SpriteRenderer>().sprite = creature.CImage;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        

    }

    // Update is called once per frame
    void Update()
    {

        if (Vector2.Distance(target.position, transform.position) < stopDistance)
        {
            agent.destination = target.transform.position;
        }
    }
}
