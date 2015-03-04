using UnityEngine;
using System.Collections;

public class LevelUpController : MonoBehaviour
{

	public delegate void LevelUp();
	public static event  LevelUp OnLeveledUp;

	private GameSettings gameSettings;

	private int[] levelGoals;
	private int levelScore; 
	private int nextLevel;



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
	void UpdateLevel () 
	{
		if(levelScore != gameSettings.PlayerPoints)
		{
			levelScore = gameSettings.PlayerPoints;	// refreseh the score so that we have what we need;

			if(levelScore > levelGoals[nextLevel])
			{
				gameSettings.PlayerLevel += 1;	// level up the player
				levelScore = levelScore - levelGoals[nextLevel-1]; // resets score back to 0 with remainder

				nextLevel++;

				if(OnLeveledUp != null)
					OnLeveledUp();
			}
		}
	}

	void SetLevelGoals()
	{
		for(int i = 0; i < 20; i++)
			levelGoals[i] = (int)((((i+1) * 1000)*.25f) + (50 * (i+1)));
	}
}
