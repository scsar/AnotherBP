using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField]
    private Sprite[] Fishes;

    private float damage = 2f;


    // Start is called before the first frame update
    void Start()
    {
        int randnum = Random.Range(0, Fishes.Length -1);
        GetComponent<SpriteRenderer>().sprite = Fishes[randnum];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Creature>())
        {
            other.GetComponent<Creature>().C_hp -= damage;
            Destroy(gameObject, 0.5f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            collision.gameObject.GetComponent<PlayerController>().Hp += 1f;
            Destroy(gameObject);
        }
    }
}
