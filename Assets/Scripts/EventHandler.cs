using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour {
	string scenename;
	[SerializeField]
	PlayerController playerController;
	void Start () 
	{
		if (playerController != null)
		{
			playerController.OnDialogueEndEvent += Handler;
		}

	}
	
	void OnDestroy () 
	{
		if (playerController != null)
		{
			playerController.OnDialogueEndEvent -= Handler;
		}
	}

	void Handler()
	{
		scenename = SceneManager.GetActiveScene().name;

		
	}

}
