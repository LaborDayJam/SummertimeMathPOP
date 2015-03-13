using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUpController : MonoBehaviour
{

	public delegate void LevelUp();
	public static event  LevelUp OnLeveledUp;
	
	public GameObject 		levelBar;
	public Text       		levelPointsText;
	public Text		  		playerLevel;
	public Text				totalPointsText;
	private GameSettings 	gameSettings;
	private int[] 			levelGoals;
	private int 			levelScore; 
	private int 			nextLevel = 1;



	// Use this for initialization
	void Awake () 
	{
		levelGoals = new int[20];
	}

	void Start()
	{
		gameSettings = GameSettings.instance;
		SetLevelGoals();
		UpdateLevel();
	
	}
	void OnDestroy()
	{

	}
	
	// Update is called once per frame
	public void UpdateLevel () 
	{
		
		levelScore = gameSettings.PlayerPoints;	// refreseh the score so that we have what we need;

		if(levelScore > levelGoals[nextLevel-1])
		{
			gameSettings.PlayerLevel += 1;	// level up the player
			levelScore = levelScore - levelGoals[nextLevel-1]; // resets score back to 0 with remainder

			nextLevel++;

			if(OnLeveledUp != null)
				OnLeveledUp();
		}
		else if(levelScore == 0 && gameSettings.PlayerLevel == 0)
			gameSettings.PlayerLevel = 1;
		
		SetBar();
	}
	void SetBar()
	{
		playerLevel.text = gameSettings.PlayerLevel.ToString();
		levelPointsText.text = levelScore + "/" + levelGoals[nextLevel-1];
		totalPointsText.text = gameSettings.PlayerPoints.ToString();
		float percent = (levelScore * 100) /levelGoals[nextLevel-1];
		levelBar.transform.localScale = new Vector3(percent/100,1,1);
	}
	void SetLevelGoals()
	{
		for(int i = 0; i < 20; i++)
			levelGoals[i] = (int)((((i+1) * 1000)*.25f) + (50 * (i+1)));
	}
}
