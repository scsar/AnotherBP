using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public CreatureData creature;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = creature.CImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
