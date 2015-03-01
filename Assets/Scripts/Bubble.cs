using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bubble : MonoBehaviour 
{
	public delegate void BubbleEvent(bool theOne);
	public static event BubbleEvent OnPop;
	public  bool 	theOne = false;
	private float 	upSpeed = 1.4f;
	private float  	moveSpeed = 0;
	private float  	timer = 0f;
	private float   moveTime = 0f;
	private bool 	isPlaying = true;
	private int  	isMovingNum = 1; // 0 = no, 1 = left, 2 = right;


	void Awake()
	{
		//RisingFactsUIManager.OnPuased += new RisingFactsUIManager.PuaseEvent(OnPuased);
	}

	void OnDestroy()
	{
		//RisingFactsUIManager.OnPuased -= new RisingFactsUIManager.PuaseEvent(OnPuased);
	}

	void OnPuased(bool playing)
	{
		isPlaying = playing;
	}

	void Move()
	{
		if(timer < moveTime && isMovingNum == 1)
		{
			transform.position = new Vector3(transform.position.x -(moveSpeed), transform.position.y+(upSpeed), transform.position.z);
			timer += Time.deltaTime;
		}
		else if(timer < moveTime && isMovingNum == 2)
		{
			transform.position = new Vector3(transform.position.x +(moveSpeed), transform.position.y+(upSpeed), transform.position.z);
			timer += Time.deltaTime;
		}
		else
		{
			timer = 0;
			moveTime = Random.Range(1f,2f);
			moveSpeed = Random.Range(.5f,1f);
		
			if(isMovingNum == 1)
				isMovingNum =2;
			else if( isMovingNum == 2)
				isMovingNum = 1;
		}
	}
	void Update()
	{
	
		Move();
	}

	public void SetBubble(int bubbleID, int bubbleAnswer, bool correctChoice)
	{
		theOne = correctChoice;
		this.gameObject.GetComponentInChildren<Text>().text = bubbleAnswer.ToString();
	}

	public void OnClicked()
	{
		if(OnPop != null)
			OnPop(theOne);
	}
}
