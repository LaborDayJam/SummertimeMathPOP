using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RisingFactsManager : MonoBehaviour 
{
	#region RisisngFactManager Variables
	public delegate void GameUpdate(int state);
	public static event GameUpdate OnUpdate;
	public static event GameUpdate OnReset;

	public delegate void RoundUpdate(float time);
	public static event RoundUpdate OnPlaying;



	public GameObject       roundObject;
	public Text				roundText;
	private BubbleFactory   bubbleFactory;
	private GameSettings 	gameSettings;
	private EndMenuSaved    endMenu;
	private MathGenerator	mathGen;

	private float 			currTimer = 0f;
	private float 			timer = 0;
	private float			maxRoundTime = 10.5f;
	private float			maxWaitTime = 3f;
	private int 			numofrounds = 11;
	private bool 			answered = false;
	private bool 			isBoy = false;
	private bool 			hasReset = false;
	public int 				gameState = 0; // 0 = pre game, 1 = in game, 2 = post game, 3 = game paused;
	public int 				prevState = -1;
	public int 				playerLevel = 1;
	public int 				roundNum = 1;
	public int 				lastRound = 0;
	public int 				timeSaved = 0;
	public int 				numCorrecct = 0;


	#endregion

	#region Unity Functions
	void Awake()
	{
		gameSettings = GameSettings.instance;

		Bubble.OnPop += new Bubble.BubbleEvent(AnsweredStatus);
		GameManager.OnUpdate += new GameManager.GlobalUpdate(RisingUpdate);
	}
	
	void OnDestroy()
	{
		Bubble.OnPop -= new Bubble.BubbleEvent(AnsweredStatus);	
		GameManager.OnUpdate -= new GameManager.GlobalUpdate(RisingUpdate);
	}
	void Start()
	{
		playerLevel = gameSettings.PlayerLevel;
		bubbleFactory = GameObject.FindGameObjectWithTag("RisingFactory").GetComponent<BubbleFactory>();
		endMenu = GameObject.FindGameObjectWithTag("EndMenu").GetComponent<EndMenuSaved>();
		mathGen  = GameObject.FindGameObjectWithTag("MathGen").GetComponent<MathGenerator>();
		mathGen.SetQuestions();
		endMenu.SaveTime = 0;
		endMenu.NumOfCorrect = 0;
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
			mathGen.GetQuestions();
		
		if(currTimer < maxRoundTime && !answered)
		{
			currTimer += Time.deltaTime;
			bubbleFactory.canSpawn = true;

			if(OnPlaying != null)
				OnPlaying( maxRoundTime-currTimer);
		}
		else if(answered)
		{
			if(currTimer > 0 && !hasReset)
			{
				bubbleFactory.canSpawn = false;
				//play annimation
				//play Sound;
				ResetGame();
			
				
			}
			else
			{
				if(!hasReset)
					ResetGame();


				Debug.Log("Outta time");
			}
		}
		else 
		{
			Debug.Log("Resetting Game");
			if(!hasReset)
				ResetGame();
		}
	}

	
	private void AnsweredStatus(bool correct)
	{
		answered = true;
		
		if(correct)
		{
			timeSaved += (int)(maxRoundTime - currTimer);
			numCorrecct += 1;			
		}
	}

	private void EndGame()
	{
		if(lastRound != roundNum)
		{
			roundNum++;
			if(roundNum < numofrounds)
			{
				currTimer = 0;
				answered = false;
				hasReset = false;
				gameState = 0;
				lastRound = roundNum;
			}
			else
			{
				endMenu.NumOfCorrect = numCorrecct;
				endMenu.SaveTime = timeSaved;
				GameManager.instance.CurrentState = 4;
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
			if(bubble.GetComponent<Bubble>().theOne)
			{
				bubble.GetComponent<Bubble>().enabled = false;
				bubble.transform.localPosition = Vector3.zero;
				StartCoroutine(WaitToPop(bubble));
			}
			else 
				Destroy(bubble);
		}
		Debug.Log("Destroyed the bubbles");
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
