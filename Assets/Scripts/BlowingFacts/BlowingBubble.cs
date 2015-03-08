using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlowingBubble : MonoBehaviour 
{
	private float 	upSpeed = 1.4f;
	private float  	moveSpeed = 0;
	private float  	timer = 0f;
	private float   moveTime = 0f;
	private bool 	isPlaying = true;
	private int  	isMovingNum = 1; // 0 = no, 1 = left, 2 = right;

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

		this.gameObject.GetComponentInChildren<Text>().text = bubbleID.ToString();
	}

}
