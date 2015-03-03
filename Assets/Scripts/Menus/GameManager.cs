using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	#region GameManager Variables
	public delegate void GlobalUpdate();
	public static event GlobalUpdate OnUpdate;

	public static GameManager 	instance;
	public GameObject[] 		stateObjects; // 0 = menus; 1 = risingfacts game; 2 = hidden facts game;

	private int 				gamesState = 0;
	private int 				lastState = -1;

	#endregion
	#region Getters/Setters Functions
	public int CurrentState
	{
		get{return gamesState;}
		set{gamesState = value;}
	}
	#endregion
	#region Unity Functions
	void Awake()
	{
		if(instance == null)
			instance = this;
		else 
			Destroy(gameObject);
	}	
	
	// Update is called once per frame
	void Update () 
	{
		if(lastState != gamesState)
		{
			SwitchStates();
			lastState = gamesState;
		}
		if(OnUpdate != null)
			OnUpdate();
	}
	#endregion
	#region Class Functions
	private void SwitchStates()
	{
		switch(gamesState)
		{
		case 0:

			for (int i = 0; i < stateObjects.Length; i++)
				stateObjects[i].SetActive(false);

			stateObjects[0].SetActive(true);
			break;
		case 1:
			
			for (int i = 0; i < stateObjects.Length; i++)
				stateObjects[i].SetActive(false);
			
			stateObjects[1].SetActive(true);
			break;
		case 2:
			
			for (int i = 0; i < stateObjects.Length; i++)
				stateObjects[i].SetActive(false);
			
			stateObjects[2].SetActive(true);
			break;
		}
	}
	#endregion
}
