using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 구조체 인스펙터 창에 띄우기
public struct TalkData
{
	public string name;	// 해당대화 캐릭터 명
	public string[] contexts; // 대화 내용
}

public class Dialogue : MonoBehaviour 
{
	// 대화를 구분할 이벤트 명
	[SerializeField] private string eventname = null;
	public string geteventname
	{
		get
		{
			return eventname;
		}
	}
	// TalkData에 대한 배열
	[SerializeField] private TalkData[] talkDatas = null;

	public TalkData[] GetObjectDialogue()
	{
		return DialogueParse.GetDialogue(eventname);
	}

	public static TalkData[] GetObjectDialogue(string eventname)
	{
		return DialogueParse.GetDialogue(eventname);
	}


}


