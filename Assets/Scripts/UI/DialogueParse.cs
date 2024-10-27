using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParse : MonoBehaviour 
{

	[SerializeField] private TextAsset csvFile = null;
	private static Dictionary<string, TalkData[]> DialogueDictionary = new Dictionary<string, TalkData[]>();

	public static TalkData[] GetDialogue(string eventname)
	{
		return DialogueDictionary[eventname];
	}


	private void Awake()
	{
		SetTalkDictionary();
	}


	public void SetTalkDictionary()
	{
		// csv파일을 잘라서 넣음
		string csvText = csvFile.text.Substring(0, csvFile.text.Length - 1);
		// csv파일을 줄단위로 분할해서 배열에 넣음
		string[] rows = csvText.Split(new char[] { '\n' });

		for (int i = 1; i < rows.Length; i++)
		{
			string[] rowValues = rows[i].Split(new char[] { ',' });

			// 유효한 이벤트가 나올때까지.
			if (rowValues[0].Trim() == "" || rowValues[0].Trim() == "end")
				continue;

			// 유효이벤트를 찾으면 TalkData 구조체를 원소로 가지는 리스트 생성
			// why List? 캐릭터가 한번에 몇개의 대사를 칠지 알수없으므로 자유롭게 저장하기위해 리스트로 받아 형변환
			List<TalkData> talkDataList = new List<TalkData>();
			// 이벤트 이름 가져오기
			string eventname = rowValues[0];
		
			// 값이 end가 아닌경우 대화 데이터를 집어넣는다.
			while(rowValues[0].Trim() != "end")
			{
				List<string> contextList = new List<string>();

				TalkData talkData; //구조체
				talkData.name = rowValues[1]; // 구조체 내부에 이름 저장
				do	// 구조체 하나를 생성
				{
				// 대사를 string으로 변환해서 리스트에 추가
					contextList.Add(rowValues[2].ToString());
					if(++i < rows.Length)
						rowValues = rows[i].Split(new char[] { ',' });
					else
						break;
				}while(rowValues[1] == "" && rowValues[0] != "end");

				talkData.contexts = contextList.ToArray();
				talkDataList.Add(talkData);
			} // end while
			DialogueDictionary.Add(eventname, talkDataList.ToArray());
		} // end for
	} // end
}