using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
	#region Class Variables
	public static MenuManager instance;
	public GameObject[]		menuPanels;
	
	private LevelUpController levelUp;
	private GameSettings	gameSettings;
	private int 			menuNum = 0;  // 0 = name, 1 = mainbutons, 2 = gamemodes, 3 = options
	private int 			lastMenu = -1;
	#endregion
	#region Getter/Setter Functions
	public int MenuState
	{
		get{return menuNum;}
		set{menuNum = value;}
	}

	#endregion
	#region Unity Functions
	void Awake()
	{
		if(instance == null)
			instance = this;
		else 
			Destroy(gameObject);

		GameManager.OnUpdate += new GameManager.GlobalUpdate(MenuUpdates);
	}

	void OnDestroy()
	{
		GameManager.OnUpdate -= new GameManager.GlobalUpdate(MenuUpdates);

	}
	// Use this for initialization
	void Start () 
	{
		gameSettings = GameSettings.instance;
		levelUp = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelUpController>();

	}

	void MenuUpdates()
	{
		if(lastMenu != menuNum)
		{
			SwitchPanels();
			lastMenu = menuNum;
		}
	}
	#endregion
	#region Class Functions
	private void SwitchPanels()
	{
		switch(menuNum)
		{
		case 0:
			for(int i = 0; i < menuPanels.Length-1; i++)
			{
				menuPanels[i].SetActive(false);
			}
			menuPanels[0].SetActive(true);
			levelUp.UpdateLevel();
			break;
		case 1:
			for(int i = 0; i < menuPanels.Length-1; i++)
			{
				menuPanels[i].SetActive(false);
			}
				menuPanels[1].SetActive(true);
			break;
		case 2:
			for(int i = 0; i < menuPanels.Length-1; i++)
			{
				menuPanels[i].SetActive(false);
			}
			menuPanels[2].SetActive(true);
			break;
		case 3:
			for(int i = 0; i < menuPanels.Length-1; i++)
			{
				menuPanels[i].SetActive(false);
			}
			menuPanels[3].SetActive(true);

			break;
		}
	}
	#endregion
}
