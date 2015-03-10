using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndMenuManager : MonoBehaviour 
{
	private EndMenuSaved endMenu;

	private int menuCount = 0;
	private int lastCount = -1;
	private int totalTime = 0;
	private int numCorrect = 0;
	private int totalPoints = 0;
	private bool calculated = false;
	private float gradeMultipler = 0;
	private string letterGrade = "";

	public Text numRightText;
	public Text savedTimeText;
	public Text gradeText;
	public Text totalPointsText;
	public Text multiplierText;


	// Use this for initialization
	void Start () 
	{
		endMenu = GameObject.FindGameObjectWithTag("EndMenu").GetComponent<EndMenuSaved>();
		totalTime = endMenu.SaveTime;
		numCorrect = endMenu.NumOfCorrect;
		GradeGame();
	}
	
	// Update is called once per frame
	void Update () 
	{
		savedTimeText.text = totalTime.ToString();
		numRightText.text = numCorrect.ToString();
		totalPointsText.text = totalPoints.ToString();
		RunLoop();
	}

	private void RunLoop()
	{
		if(lastCount != menuCount)
		{
			lastCount = menuCount;
			switch(menuCount)
			{
			case 0:StartCoroutine(WaitToStart()); 	break;
			case 1:StartCoroutine(CalNumCorrect());	break;
			case 2:StartCoroutine(CalTime());		break;
			case 3:StartCoroutine(CalGrade());		break;
			case 4:StartCoroutine(CalMultiplier());	break;
			}
		}
	}

	private void GradeGame()
	{
		if(numCorrect == 10 && totalTime >= 30)
		{
			gradeMultipler = 5;
			letterGrade = "A";
		}
		else if(numCorrect >= 7 && totalTime >= 20)
		{
			gradeMultipler = 3;
			letterGrade = "B";
		}
		else if(numCorrect >= 5 && totalTime >= 10)
		{
			gradeMultipler = 1.5f;
			letterGrade = "C";
		}
		else if(numCorrect >= 3 && totalTime > 0)
		{
			gradeMultipler = .5f;
			letterGrade = "D";
		}
		else
		{
			gradeMultipler = 0;
			letterGrade = "F";
		}
	}

	public void OnDone()
	{

	}

	public void OnRestart()
	{

	}

	private IEnumerator CalTime()
	{

		while(totalTime > 0)
		{
			totalTime--;
			totalPoints++;
			menuCount= 3;
			yield return null;
		}
		yield return new WaitForSeconds(1f);
	}

	private IEnumerator CalNumCorrect()
	{
		while(numCorrect > 0)
		{
			numCorrect--;
			totalPoints++;
			yield return null;
		}
		menuCount = 2;
		yield return new WaitForSeconds(1);
	}

	private IEnumerator CalGrade()
	{
		yield return new WaitForSeconds(1);
		gradeText.text = letterGrade;
		multiplierText.text = "X" + gradeMultipler.ToString();
		yield return new WaitForSeconds(1);
		menuCount = 4;
	}

	private IEnumerator CalMultiplier()
	{
		multiplierText.text = "";
		if(gradeMultipler != 0)
			totalPoints = (int)(totalPoints * gradeMultipler);
		yield return new WaitForSeconds(1);
			
	}
	private IEnumerator WaitToStart()
	{
		yield return new WaitForSeconds(2);
		menuCount = 1;
	}
}
