using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HiddenFactory : MonoBehaviour 
{
	public GameObject[] hiddenSpots;
	public GameObject	bubbleContainer;
	public Button		hiddenBubble;
	public bool 		canSpawn = true;

	private int[] 		otherNumbers;
	private int 		maxBubbleCount = 12;
	private int 		theAnswer = 0;
	private int 		answerBubble = 0;
	private int 		bubbleCount = 0;


	void Awake()
	{
		HiddenFactsManager.OnUpdate += new HiddenFactsManager.GameUpdate(FactoryUpdate);
		MathGenerator.OnGetQuestion += new MathGenerator.QuestionOut(GetInfo);
	}

	void OnDestroy()
	{
		HiddenFactsManager.OnUpdate -= new HiddenFactsManager.GameUpdate(FactoryUpdate);
		MathGenerator.OnGetQuestion -= new MathGenerator.QuestionOut(GetInfo);	
	}

	private void OnPause(bool playing)
	{
		canSpawn = playing;
	}

	private void GetInfo(int x, int y, int answered)
	{
		theAnswer = answered;
		answerBubble = Random.Range(0,12);
	}

	private void FactoryUpdate(int gameState)
	{
		switch(gameState)
		{
			case 0:
			{
				canSpawn = true;
				bubbleCount = 0;
			}
				break;
			case 1:
			{
				SpawnBubbles();
			}
				break;
		}
	}

	private void SpawnBubbles()
	{
		if(canSpawn)
		{
			if(bubbleCount < maxBubbleCount)
			{
				Button clone = Instantiate(hiddenBubble, hiddenSpots[bubbleCount].transform.position, Quaternion.identity) as Button;
				clone.transform.SetParent(bubbleContainer.transform);

				if(bubbleCount == answerBubble)
				{
					clone.GetComponent<HiddenBubble>().SetBubble(bubbleCount, theAnswer, true);
				}
				else
				{
					int theNum = theAnswer;
					while(theNum == theAnswer)
					{
						theNum = Random.Range (GameSettings.instance.PlayerLevel - 1, (GameSettings.instance.PlayerLevel * 5));
					}
					clone.GetComponent<HiddenBubble>().SetBubble(bubbleCount, theNum, false);
				}
				bubbleCount++;
			}
		}
	}
}
