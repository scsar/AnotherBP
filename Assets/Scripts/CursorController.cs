using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorController : MonoBehaviour {

	[SerializeField] private Transform tf_cursor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		CursorMoving();	
	}

	void CursorMoving()
	{
		float x = Input.mousePosition.x - (Screen.width / 2);
    	float y = Input.mousePosition.y - (Screen.height / 2);
    	tf_cursor.localPosition = new Vector2(x, y);
			
		// 마우스 가두기 (범위 지정)
        float tmp_cursorPosX = tf_cursor.localPosition.x;
        float tmp_cursorPosY = tf_cursor.localPosition.y;

        float min_width = -Screen.width / 2;
        float max_width = Screen.width / 2;
        float min_height = -Screen.height / 2;
        float max_height = Screen.height / 2;
        int padding = 20;	// 값은 자유

        tmp_cursorPosX = Mathf.Clamp(tmp_cursorPosX, min_width + padding, max_width - padding);
        tmp_cursorPosY = Mathf.Clamp(tmp_cursorPosY, min_height + padding, max_height - padding);
        
        tf_cursor.localPosition = new Vector2(tmp_cursorPosX, tmp_cursorPosY);
	}
}
