using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour 
{
	#region Class Variables
	public static GameSettings instance;
	public bool 	hasPlayed = false;

	private int 	playerLevel = 0;
	private int 	playerPoints = 0;
	private int 	currentDifficulty = 1;
	#endregion
	#region Getter/Setter Functions
	public int PlayerLevel 
	{
		get{return playerLevel;}
		set{playerLevel = value;}
	}
	public int PlayerPoints
	{
		get{return playerPoints;}
		set{playerPoints = value;}
	}
	public int DifficultyLevel
	{
		get{return currentDifficulty;}
		set{currentDifficulty = value;}
	}
	#endregion
	#region Unity Functions
	// Use this for initialization
	void Awake () 
	{
		if(instance == null)
			instance = this;
		else 
			Destroy(gameObject);
	
		playerPoints = PlayerPrefs.GetInt("PlayerPoints");
	}

	void OnDestroy()
	{
		SaveGame();
	}
	#endregion
	#region Class Functions
	public void SaveGame()
	{
		PlayerPrefs.SetInt("PlayerPoints", playerPoints);
	}
	#endregion
}
