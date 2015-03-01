using UnityEngine;
using System.Collections;

public class MainPanel : MonoBehaviour 
{
	#region Class Variables
	private GameSettings 	gameSettings;
	private MenuManager 	menuManager;
	#endregion
	#region Unity Functions
	// Use this for initialization
	void Start () 
	{
		menuManager = MenuManager.instance;
		gameSettings = GameSettings.instance;
	}
	#endregion
	#region Event Functions
	public void OnStartClicked()
	{
		menuManager.MenuState =1;
	}

	public void OnOptionsClicked()
	{
		menuManager.MenuState = 2;
	}

	public void OnExitClicked()
	{
		gameSettings.SaveGame();
	}
	#endregion
}
