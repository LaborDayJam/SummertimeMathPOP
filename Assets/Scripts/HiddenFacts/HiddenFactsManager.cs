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

	public GameObject 		roundObject;
	public Text 			roundText;
	private GameSettings 	gameSettings;
	private HiddenFactory	hiddenFactory;

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
	private int 			score = 0;
	
	
	#region Unity Functions
	void Awake()
	{
		gameSettings = GameSettings.instance;
		
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
		playerLevel = gameSettings.PlayerLevel;
		hiddenFactory = GameObject.FindGameObjectWithTag("HiddenFactory").GetComponent<HiddenFactory>();
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
			MathGenerator.instance.GetQuestions();
		
		if(currTimer < maxRoundTime && !answered)
		{
			currTimer += Time.deltaTime;
			
			if(OnPlaying != null)
				OnPlaying(maxRoundTime - currTimer);
		}
		else if(answered)
		{
			if(currTimer > 0 && !hasReset)
			{
				hiddenFactory.canSpawn = false;
				//play annimation
				//play Sound;
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
			score += 5 * playerLevel;
	}
	
	private void EndGame()
	{
		if(lastRound != roundNum)
		{
			roundNum++;
			if(roundNum <= numOfRounds)
			{
				Debug.Log("Waiting to start new round");
				currTimer = 0;
				answered = false;
				hasReset = false;
				gameState = 0;
				lastRound = roundNum;
			}
			else
			{
				Debug.Log("end game here");
			}
		}
	}
	
	private void PausedGame(){}
	
	
	
	private void ResetGame()
	{
		GameObject[] bubbles = GameObject.FindGameObjectsWithTag("Bubble");
		hasReset = true;
		// find bubbles here goin 
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
