using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Bar : MonoBehaviour
{
    private GameObject Canvas;

    // Start is called before the first frame update
    void Awake()
    {
        Canvas = GameObject.Find("Canvas");
        // transform.SetParent(Canvas.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
