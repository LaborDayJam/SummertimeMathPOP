using UnityEngine;
using System.Collections;

public class EndMenuSaved : MonoBehaviour 
{
	private int timeSaved = 35;
	private int numCorrect = 10;
	
	public int SaveTime
	{
		get{return timeSaved;}
		set{timeSaved = value;}
	}

	public int NumOfCorrect
	{
		get{return numCorrect;}
		set{numCorrect = value;}
	}
	
}
