using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModePanel : MonoBehaviour 
{
	#region Class Variables
	public Text				diffNumText;
	public Text				gameModeText;

	private GameSettings 	gameSettings;
	private MenuManager  	menuManager;
	private string 			risingFacts = "Rising Facts";
	private string 			hiddenFacts = "Hidden Facts";
	private string 			blowingFacts = "Blowing Facts";

	private int 			modeNum = 0; // 0 = rising facts; 1 = hidden facts;
	private int 			diffNum  = 1; // this num can never be higher than playerLevel;
	#endregion
	#region Unity Functions
	void Start()
	{
		gameSettings = GameSettings.instance;
		menuManager = MenuManager.instance;

		diffNum = gameSettings.PlayerLevel;
		diffNumText.text = diffNum.ToString();
	}
	#endregion
	#region Class Functions
	private void SetModeName()
	{
	
		if(modeNum > 2)
			modeNum = 0;
		else if(modeNum < 0)
			modeNum = 2;
		
		
		switch(modeNum)
		{
			case 0:
				gameModeText.text = risingFacts;
				break;
			case 1:
				gameModeText.text = hiddenFacts;
				break;
			case 2:
				gameModeText.text = blowingFacts;
			break;
		}
	}
	
	private void SetDifficultyNumber()
	{
		if(diffNum > gameSettings.PlayerLevel)
			diffNum = 1;
		else if(diffNum <= 0)
			diffNum = gameSettings.PlayerLevel;
			
			
		diffNumText.text = diffNum.ToString();
	}
	#endregion
	#region Event Functions
	public void OnBackClicked()
	{
		diffNum = gameSettings.PlayerLevel;
		menuManager.MenuState = 1;
	}

	public void OnStartClicked()
	{
		 gameSettings.DifficultyLevel = diffNum;
		 GameManager.instance.CurrentState = modeNum+1;
	}

	public void LeftModeClicked()
	{
		modeNum--;
		SetModeName();
	}

	public void RightModeClicked()
	{
		modeNum++;
		SetModeName();	
	}
	public void LeftDiffClicked()
	{
		diffNum --;
		SetDifficultyNumber();	
	}

	public void RightDiffClicked()
	{
		diffNum++;
		SetDifficultyNumber();	
	}
	#endregion
}
