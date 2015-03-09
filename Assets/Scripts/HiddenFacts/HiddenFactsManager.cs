using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HiddenFactsManager : MonoBehaviour 
{

	public delegate void GameUpdate(int state);
	public static event GameUpdate OnUpdate;
	public static event GameUpdate OnReset;
	
	public delegate void RoundUpdate(float time);
	public static event RoundUpdate OnPlaying;

	public delegate void ScoreUpdate(int totalSavedTime, int totalNumCorrect);
	public static event ScoreUpdate OnCorrect;

	public GameObject 		roundObject;
	public Text 			roundText;

	private HiddenFactory	hiddenFactory;
	private MathGenerator	mathGen;
	private EndMenuSaved    endMenu;

	private float 			maxWaitTime = 3;
	private float 			maxRoundTime = 10.5f;
	private float 			currTimer = 0;
	private bool 			answered = false;
	private bool 			hasReset = false;
	private int 			playerLevel = 1;
	private int				numOfRounds = 10;
	private int				gameState = 0;
	private int 			prevState = -1;
	private int 			roundNum = 1;
	private int 			lastRound = 0;
	private int 			savedTime = 0;
	private int 			numCorrect = 0;
	
	#region Unity Functions
	void Awake()
	{
		HiddenBubble.OnPop += new HiddenBubble.BubbleEvent(AnsweredStatus);
		GameManager.OnUpdate += new GameManager.GlobalUpdate(HiddenUpdate);
	}
	
	void OnDestroy()
	{
		HiddenBubble.OnPop -= new HiddenBubble.BubbleEvent(AnsweredStatus);	
		GameManager.OnUpdate -= new GameManager.GlobalUpdate(HiddenUpdate);
	}

	void Start()
	{
		hiddenFactory = GameObject.FindGameObjectWithTag("HiddenFactory").GetComponent<HiddenFactory>();
		mathGen = GameObject.FindGameObjectWithTag("MathGen").GetComponent<MathGenerator>();
		endMenu = GameObject.FindGameObjectWithTag("EndMenu").GetComponent<EndMenuSaved>();
		endMenu.NumOfCorrect = 0;
		endMenu.SaveTime = 0;
		mathGen.SetQuestions();
	}
	
	void HiddenUpdate()
	{
		if(prevState != gameState)
		{
			prevState = gameState;
			StateSwitch();
		}
		
		if(gameState == 1)
			StateSwitch();
		
		if(OnUpdate != null)
			OnUpdate(gameState);
	}
	#endregion
	
	#region Class Functions
	private void StateSwitch()
	{
		switch(gameState)
		{
			case 0 :{PreGame();}break;
			case 1 :{RunningGame();}break;
			case 2 :{EndGame();}break;
			case 3 :{PausedGame();}break;
		}
	}
	
	private void PreGame()
	{
		roundObject.SetActive(true);
		roundText.text = "Round " + roundNum.ToString();
		lastRound = 0;
		StartCoroutine(WaitForRound(maxWaitTime));
	}
	
	private void RunningGame()
	{
		if(currTimer == 0)
		{
			mathGen.GetQuestions();
			hiddenFactory.canSpawn = true;
		}
		if(currTimer < maxRoundTime && !answered)
		{
			currTimer += Time.deltaTime;
			
			if(OnPlaying != null)
				OnPlaying(maxRoundTime - currTimer);
		}
		else if(answered)
		{
			hiddenFactory.canSpawn = false;

			if(currTimer > 0 && !hasReset)
			{
				ResetGame();
			}
			else
			{
				if(!hasReset)
					ResetGame();
			}
		}
		else 
		{
			if(!hasReset)
				ResetGame();
		}
	}

	private void AnsweredStatus(bool correct)
	{
		answered = true;
		if(correct)
		{
			savedTime += (int)(maxRoundTime - currTimer);
			numCorrect += 1;

			if(OnCorrect != null)
				OnCorrect(savedTime,numCorrect);
		}
	}
	
	private void EndGame()
	{
		if(lastRound != roundNum)
		{
			roundNum++;
			if(roundNum <= numOfRounds)
			{
				currTimer = 0;
				answered = false;
				hasReset = false;
				gameState = 0;
				lastRound = roundNum;
			}
			else
			{
				endMenu.SaveTime = savedTime;
				endMenu.NumOfCorrect = numCorrect;
				GameManager.instance.CurrentState = 4;
			}
		}
	}
	
	private void PausedGame(){}

	private void ResetGame()
	{
		GameObject[] bubbles = GameObject.FindGameObjectsWithTag("Bubble");
		foreach(GameObject bubble in bubbles)
		{
			if(bubble.GetComponent<HiddenBubble>().theOne)
			{
				bubble.GetComponent<HiddenBubble>().enabled = false;
				bubble.transform.localPosition = Vector3.zero;
				StartCoroutine(WaitToPop(bubble));
			}
			else 
				Destroy(bubble);
		}
		hasReset = true;
	}

	IEnumerator WaitToPop(GameObject bubble)
	{
		yield return new WaitForSeconds(2);
		Destroy(bubble);
		gameState = 2;
	}
	
	IEnumerator WaitForRound(float time)
	{
		yield return new WaitForSeconds(time);
		roundObject.SetActive(false);
		gameState = 1;
	}
	
	
	#endregion

}
