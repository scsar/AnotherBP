using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

	public Transform target;
	public float dist = 0.25f;
	public float height = 0.125f;

	private Transform tr;
	void Start ()
	{
		tr = GetComponent<Transform>();
	}
	
	void LateUpdate () 
	{
			CameraTurn();
	}

	void CameraTurn()
	{
		tr.position = new Vector3(target.position.x, target.position.y, -7);// - (Vector2. * dist) - (Vector2.up * height);
		tr.LookAt (target);
		
	}
}
