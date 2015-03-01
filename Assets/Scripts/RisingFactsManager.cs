using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RisingFactsManager : MonoBehaviour 
{
	#region RisisngFactManager Variables
	public delegate void GameUpdate(int state);
	public static event GameUpdate OnUpdate;
	public static event GameUpdate OnReset;

	public delegate void RoundUpdate(int round, int score, float time, bool boy);
	public static event RoundUpdate OnPlaying;



	public GameObject       roundObject;
	public GameObject 		startObject;
	public Text				roundText;
	private GameSettings 	gameSettings;

	private float 			currTimer = 0f;
	private float 			timer = 0;
	private float			maxRoundTime = 10.5f;
	private float			maxWaitTime = 3f;
	private int 			numofrounds = 11;
	private bool 			answered = false;
	private bool			isBoy = false;
	public int 				gameState = 0; // 0 = pre game, 1 = in game, 2 = post game, 3 = game paused;
	public int 				playerLevel = 1;
	public int 				round = 1;
	public int 				lastRound = 0;
	public int 				score = 0;


	#endregion

	#region Unity Functions
	void Awake()
	{
		gameSettings = GameSettings.instance;

		Bubble.OnPop += new Bubble.BubbleEvent(AnsweredStatus);
		GameManager.OnUpdate += new GameManager.GlobalUpdate(RisingUpdate);
		Time.timeScale = 1;

	}
	
	void OnDestroy()
	{
		Bubble.OnPop -= new Bubble.BubbleEvent(AnsweredStatus);	
		GameManager.OnUpdate -= new GameManager.GlobalUpdate(RisingUpdate);
	}
	void Start()
	{
		playerLevel = 1;//gameSettings.PlayerLevel;
	}

	void RisingUpdate()
	{
		if(lastRound != round)
		{
			StateSwitch();
			lastRound = round;
		}
	
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
			case 2 :{PostGame();}break;
			case 3 :{EndGame();}break;
			case 4 :{PausedGame();}break;
		}
	}
	
	private void PreGame()
	{
		roundObject.SetActive(true);
		roundText.text = "Round " + round.ToString();
		startObject.SetActive(true);
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
				OnPlaying(playerLevel, score, maxRoundTime-currTimer,isBoy);
		}
		else if(answered)
		{
			if(currTimer > 0)
			{
				GameObject[] bubbles = GameObject.FindGameObjectsWithTag("bubbles");
				
				// find bubbles here goin 
				foreach(GameObject bubble in bubbles)
				{
					if(bubble.GetComponent<Bubble>().theOne)
					{
						bubble.GetComponent<Bubble>().enabled = false;
						bubble.transform.localPosition = Vector3.zero;
						StartCoroutine(WaitToPop(bubble));
					}
					else 
						Destroy(bubble);
				}
				
				
			}
			else
			{
				ResetGame();
			}
		}
		else 
		{
			ResetGame();
		}
	}
	
	private void PostGame()
	{
		
		Debug.Log("increasing round");
		if(lastRound != round)
			round++;
		
		lastRound = round;
		gameState = 3;
	}
	
	private void AnsweredStatus(bool correct)
	{
		answered = true;
	
		if(correct)
		{
			score+=5;
		}
	}

	private void EndGame()
	{
		if(round < numofrounds)
		{
			Debug.Log("Waiting to start new round");
			currTimer = 0;
			gameState = 0;
			lastRound = 0;
			answered = false;
		}
		else
		{
			Debug.Log("end game here");
		}
	}
	
	
	private void PausedGame(){}
	


	private void ResetGame()
	{
		GameObject[] bubbles = GameObject.FindGameObjectsWithTag("bubbles");
		
		// find bubbles here goin 
		foreach(GameObject bubble in bubbles)
		{
			Destroy(bubble);
		}
		StartCoroutine("WaitToEnd", 2f);
	}

	

	IEnumerator WaitToPop(GameObject bubble)
	{
		yield return new WaitForSeconds(2);
		Destroy(bubble);
		gameState =2;
	}

	IEnumerator WaitToEnd(float time)
	{
		yield return new WaitForSeconds(time);
		gameState =2;
	}
	IEnumerator WaitForStart(float time)
	{
		yield return new WaitForSeconds(time);
	}
	IEnumerator WaitForRound(float time)
	{
		Debug.Log("Entered");
		yield return new WaitForSeconds(time);
		Debug.Log("Exited");
		roundObject.SetActive(false);
		startObject.SetActive(false);
		gameState = 1;
	}


	#endregion
}
