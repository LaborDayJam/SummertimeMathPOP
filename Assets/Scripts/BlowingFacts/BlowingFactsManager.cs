using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlowingFactsManager : MonoBehaviour 
{
	#region BlowingFactsManager Variables
	public delegate void GameUpdate(int state);
	public static event GameUpdate OnUpdate;
	public static event GameUpdate OnReset;
	
	public delegate void RoundUpdate(float time);
	public static event RoundUpdate OnPlaying;
	
	
	
	public GameObject       roundObject;
	public GameObject		youWin;
	public GameObject       youLose;
	public Text				roundText;
	private BlowingFactory  bubbleFactory;
	private GameSettings 	gameSettings;
	
	private float 			currTimer = 0f;
	private float 			timer = 0;
	private float			maxRoundTime = 10.5f;
	private float			maxWaitTime = 3f;
	private int 			numofrounds = 11;
	private bool 			answered = false;
	private bool 			isRight = false;
	private bool 			hasReset = false;
	public int 				gameState = 0; // 0 = pre game, 1 = in game, 2 = post game, 3 = game paused;
	public int 				prevState = -1;
	public int 				playerLevel = 1;
	public int 				roundNum = 1;
	public int 				lastRound = 0;
	public int 				score = 0;
	
	
	#endregion
	
	#region Unity Functions
	void Awake()
	{
		gameSettings = GameSettings.instance;
		GameManager.OnUpdate += new GameManager.GlobalUpdate(RisingUpdate);
		BlowingFactory.OnFinised += new BlowingFactory.PlayesrFinished(AnsweredStatus);
	}
	
	void OnDestroy()
	{
		GameManager.OnUpdate -= new GameManager.GlobalUpdate(RisingUpdate);
		BlowingFactory.OnFinised -= new BlowingFactory.PlayesrFinished(AnsweredStatus);
	}
	void Start()
	{
		playerLevel = gameSettings.PlayerLevel;
		bubbleFactory = GameObject.FindGameObjectWithTag("BlowingFactory").GetComponent<BlowingFactory>();	
	}
	
	void RisingUpdate()
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
			MathGenerator.instance.GetQuestions();
			bubbleFactory.canSpawn = true;
		}
		if(currTimer < maxRoundTime && !answered)
		{
			currTimer += Time.deltaTime;

			
			if(OnPlaying != null)
				OnPlaying( maxRoundTime-currTimer);
		}
		else if(answered)// change this to if done and bubbleCount is < theAnswer
		{
			if(currTimer <= 10 && isRight && !hasReset)
			{
				score++;
				youWin.SetActive(true);
				bubbleFactory.canSpawn = false;
				ResetGame();
				
				
			}
			else
			{
				bubbleFactory.canSpawn = false;
				youLose.SetActive(true);
				if(!hasReset)
					ResetGame();
			}
		}
		else 
		{
			bubbleFactory.canSpawn = false;
			if(bubbleFactory.bubbleCount > bubbleFactory.theAnswer)
				youLose.SetActive(true);
			else if(bubbleFactory.bubbleCount == bubbleFactory.theAnswer)
				youWin.SetActive(true);
			else 
				youLose.SetActive(true);

			if(!hasReset)
				ResetGame();
		}
	}
	
	
	public void AnsweredStatus(bool correct)
	{
		answered = true;
		isRight = correct;
	}
	
	private void EndGame()
	{
		if(lastRound != roundNum)
		{
			roundNum++;
			if(roundNum < numofrounds)
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
		Debug.Log("Resetting Game");
		GameObject[] bubbles = GameObject.FindGameObjectsWithTag("Bubble");
		hasReset = true;
		// find bubbles here goin 
		foreach(GameObject bubble in bubbles)
		{
			Destroy(bubble);
		}
		Debug.Log("Destroyed the bubbles");
		StartCoroutine(WaitToPop());
	}
	
	IEnumerator WaitToPop()
	{
		yield return new WaitForSeconds(2);
		youWin.SetActive(false);
		youLose.SetActive(false);
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
