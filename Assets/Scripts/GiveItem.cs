using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
    private bool active;
    public GameObject giveWeaphon;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        giveWeaphon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && active)
        {
            giveWeaphon.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            active = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        active = false;
        giveWeaphon.SetActive(false);
    }
}
