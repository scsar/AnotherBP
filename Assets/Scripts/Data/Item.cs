using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    void Awake()
    {
        itemData.ICount = 0;
    }


    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.iIcon;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
