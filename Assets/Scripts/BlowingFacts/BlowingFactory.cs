﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlowingFactory : MonoBehaviour 
{
	public GameObject 	bubbleContainer;
	public GameObject 	spawnPosition;
	public GameObject   bubble;
	
	public bool 		canSpawn = true;
	public bool 		onFinished = false;
	private int 		maxXScale = 0;
	private int 		minXScale = 0;
	private int 		maxYScale = 0;
	private int 		minYScale = 0;
	private int 		lastState = 0;
	private int 		theAnswer = 0;
	public int 			bubbleCount = 0;
	
	
	void Awake()
	{
		BlowingFactsManager.OnUpdate += new BlowingFactsManager.GameUpdate(FactoryUpdate);
		RisingFactsUIManager.OnPuased += new RisingFactsUIManager.PuaseEvent(OnPaused);
		MathGenerator.OnGetQuestion += new MathGenerator.QuestionOut(GetInfo);
	}
	
	
	void OnDestroy()
	{
		BlowingFactsManager.OnUpdate -= new BlowingFactsManager.GameUpdate(FactoryUpdate);
		MathGenerator.OnGetQuestion -= new MathGenerator.QuestionOut(GetInfo);
	}
	
	
	private void GetInfo(int x, int y, int answer)
	{
		theAnswer = answer;
	}
	private void FactoryUpdate(int state)
	{
		if(lastState != state || state == 1)
		{
			SwitchState();
			lastState = state;
		}
	}
	
	private void OnPaused(bool playing)
	{
		canSpawn = playing;
	}
	private void SwitchState()
	{
		switch(lastState)
		{
		case 0:
		{
			bubbleCount = 0;
			canSpawn = true;
		}break;
		case 1:
		{
			SpawnBubbles();
		}break;
			
		}
	}
	
	private void SpawnBubbles()
	{
		if(canSpawn || bubbleCount < theAnswer || !onFinished)
		{	
			GameObject clone = Instantiate(bubble, spawnPosition.transform.position, Quaternion.identity)as GameObject;
			clone.transform.SetParent(bubbleContainer.transform);
			bubbleCount++;
		}
		else if(bubbleCount > theAnswer && !onFinished)
		{
			// they made to many bubbles reset the game here.
		}
		else if(bubbleCount < theAnswer && onFinished)
		{
			// they got it right reset the game here.
		}
	}
}
