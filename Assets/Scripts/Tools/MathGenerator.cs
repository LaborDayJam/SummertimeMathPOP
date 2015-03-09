using UnityEngine;
using System.Collections;

public class MathGenerator : MonoBehaviour 
{
	public static MathGenerator instance;
	
	public delegate void QuestionOut(int x, int y, int answer);
	public static event QuestionOut OnGetQuestion;

	private enum MathType{ADD,SUBTRACT,MULTIPLY,DIVIDE,MIXED};
	private int []	 	x,y,answer;
	private int 		level = 0;
	private int 		questionCount = 0;
	private int 		maxNum = 0;
	private int 		minNum = 0;

	void Awake()
	{
		if(instance != null)
			Destroy(gameObject);

		instance = this;
	}

	// Use this for initialization
	void CheckLevel () 
	{
		level 			= GameSettings.instance.PlayerLevel;
		questionCount 	= 10;
		x 				= new int[questionCount];
		y 				= new int[questionCount];
		answer 			= new int[questionCount];
	
		if(level <= 3)
		{
			maxNum = 3;
			minNum = 0;
		}
		else if (level > 3 && level <= 6)
		{
			maxNum = 5;
			minNum = 2;
		}
		else if (level > 6 && level <= 10)
		{
			maxNum = 7;
			minNum = 4;
		}
		else if(level > 10 && level <= 15)
		{
			maxNum = 8;
			minNum = 6;
		}
		else if( level > 15 && level <= 18)
		{
			maxNum = 10;
			minNum = 8;
		}
		else 		
		{
			maxNum = 12;
			minNum = 8;
		} 
	}

	void OnDestroy()
	{

	}

	public void GetQuestions()
	{
		questionCount -= 1;
		
		if(OnGetQuestion != null)
			OnGetQuestion(x[questionCount],y[questionCount],answer[questionCount]);
	}

	public void SetQuestions()
	{
		CheckLevel();
		
		for( int i = 0; i < answer.Length ; i++)
		{
			x[i] = Random.Range(minNum,maxNum);
			y[i] = Random.Range(minNum,maxNum);
			answer[i] = x[i] * y[i];
			Debug.Log (x[i].ToString() + " x " + y[i].ToString() + " = " + answer[i].ToString());
		}

	}
	



}
