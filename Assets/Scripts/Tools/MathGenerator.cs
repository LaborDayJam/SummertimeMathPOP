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
	void Start () 
	{
		level 			= GameSettings.instance.PlayerLevel;
		questionCount 	= 10;
		x 				= new int[questionCount];
		y 				= new int[questionCount];
		answer 			= new int[questionCount];
		maxNum 			= (level)+1;
		minNum 			= level;

		 
		SetQuestions(); 
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

	private void SetQuestions()
	{
		for( int i = 0; i < answer.Length ; i++)
		{
			x[i] = Random.Range(minNum,maxNum);
			y[i] = Random.Range(minNum,maxNum);



			answer[i] = x[i] * y[i];
			Debug.Log (x[i].ToString() + " x " + y[i].ToString() + " = " + answer[i].ToString());
		}

	}
	



}
