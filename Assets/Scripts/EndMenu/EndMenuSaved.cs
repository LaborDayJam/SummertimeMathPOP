using UnityEngine;
using System.Collections;

public class EndMenuSaved : MonoBehaviour 
{
	private int timeSaved = 0;
	private int numCorrect = 0;
	
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
