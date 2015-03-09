using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlowingFactory : MonoBehaviour 
{

	public delegate void PlayesrFinished(bool correct);
	public static event PlayesrFinished OnFinised;

	public GameObject 	bubbleContainer;
	public GameObject 	spawnPosition;
	public GameObject   bubble;
	
	public bool 		canSpawn = true;
	public bool 		isBlowing = false;
	private int 		maxXScale = 0;
	private int 		minXScale = 0;
	private int 		maxYScale = 0;
	private int 		minYScale = 0;
	private int 		lastState = 0;
	public  int 		theAnswer = 0;
	public int 			bubbleCount = 0;
	
	
	void Awake()
	{
		BlowingFactsManager.OnUpdate += new BlowingFactsManager.GameUpdate(FactoryUpdate);
		Blowing.OnBlowing += new Blowing.BlowingUpdate(IsBlowing);
		MathGenerator.OnGetQuestion += new MathGenerator.QuestionOut(GetInfo);
	}
	
	
	void OnDestroy()
	{
		BlowingFactsManager.OnUpdate -= new BlowingFactsManager.GameUpdate(FactoryUpdate);
		Blowing.OnBlowing -= new Blowing.BlowingUpdate(IsBlowing);
		MathGenerator.OnGetQuestion -= new MathGenerator.QuestionOut(GetInfo);
	}
	
	private void IsBlowing(bool blowing)
	{
			isBlowing = blowing;
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
		if(canSpawn)
		{	
			if(isBlowing)
			{
				GameObject clone = Instantiate(bubble,new Vector3(Random.Range(0f,500.0f), 100,0), Quaternion.identity)as GameObject;
				clone.transform.SetParent(bubbleContainer.transform);
				bubbleCount++;
				clone.GetComponent<BlowingBubble>().SetBubble(bubbleCount,0,false);
				canSpawn = false;
				StartCoroutine("WaitToBubble");
			}
		}
	}

	public void IsFinished()
	{
		if(OnFinised != null)
		{
			if(bubbleCount == theAnswer)
				OnFinised(true);
			else
				OnFinised(false);
		}
	}
	private IEnumerator WaitToBubble()
	{
		yield return new WaitForSeconds(.05f);
		canSpawn = true;
	}
}
