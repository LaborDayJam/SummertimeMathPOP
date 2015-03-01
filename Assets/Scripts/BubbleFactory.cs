using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BubbleFactory : MonoBehaviour 
{
	public GameObject 	bubbleContainer;
	public GameObject 	spawnPosition;
	public Button[]	  	bubbleButtons;

	private float 		spawnTime = .5f;
	private float 		timer = 0f;
	private int[] 		otherNumbers;
	private int 		maxBubbleCount = 5;
	private int 		maxXScale = 0;
	private int 		minXScale = 0;
	private int 		maxYScale = 0;
	private int 		minYScale = 0;
	private int 		lastState = 0;
	private int 		theAnswer = 0;
	private int 		answerBubble = 0;
	private int 		bubbleCount = 0;


	void Awake()
	{
		RisingFactsManager.OnUpdate += new RisingFactsManager.GameUpdate(FactoryUpdate);
		MathGenerator.OnGetQuestion += new MathGenerator.QuestionOut(GetInfo);
	}


	void OnDestroy()
	{
		RisingFactsManager.OnUpdate -= new RisingFactsManager.GameUpdate(FactoryUpdate);
		MathGenerator.OnGetQuestion -= new MathGenerator.QuestionOut(GetInfo);
	}


	private void GetInfo(int x, int y, int answer)
	{
		theAnswer = answer;
		answerBubble = Random.Range(0,5);
	}
	private void FactoryUpdate(int state)
	{
		if(lastState != state)
		{

			lastState = state;
		}
		SwitchState();
	}

	private void SwitchState()
	{
		switch(lastState)
		{
			case 0:
			{
				bubbleCount = 0;
			}break;
			case 1:
			{
				SpawnBubbles();
			}break;
	
		}
	}

	private void SpawnBubbles()
	{
	
		if(timer < spawnTime)
			timer += Time.deltaTime;
		else if(bubbleCount < maxBubbleCount && theAnswer != 0 )
		{
			Button clone = Instantiate(bubbleButtons[0], spawnPosition.transform.position, Quaternion.identity)as Button;
			clone.transform.SetParent(bubbleContainer.transform);
			if(bubbleCount == answerBubble)
			{
				clone.GetComponent<Bubble>().SetBubble(bubbleCount,theAnswer,true);
			}
			else 
			{
				int theNum = theAnswer;
				while(theNum == theAnswer)
				{
					theNum = Random.Range(GameSettings.instance.PlayerLevel - 1, (GameSettings.instance.PlayerLevel * 5)*5);
				}
				clone.GetComponent<Bubble>().SetBubble(bubbleCount, theNum, false);
			}
			bubbleCount++;
			timer = 0;
		}
		else
		{
			answerBubble = Random.Range(0,6);
			theAnswer = 0;
		}
	}
}

