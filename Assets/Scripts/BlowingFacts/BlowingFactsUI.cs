using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlowingFactsUI : MonoBehaviour 
{
	public delegate void AnswerOut(int answer);
	public static event  AnswerOut OnRecieved;
	
	public delegate void PuaseEvent(bool isPuased);
	public static event PuaseEvent OnPuased;
	
	public Text equationText;
	public Text timeText;
	public Text correctText;
	public Text savedTimeText;

	// Use this for initialization
	void Awake () 
	{
		MathGenerator.OnGetQuestion += new MathGenerator.QuestionOut(QuestionIn);
		BlowingFactsManager.OnPlaying += new BlowingFactsManager.RoundUpdate(UIUpdate);		
		BlowingFactsManager.OnCorrect += new BlowingFactsManager.ScoreUpdate(UpdateScore);
	}

	void OnDestroy()
	{
		MathGenerator.OnGetQuestion -= new MathGenerator.QuestionOut(QuestionIn);
		BlowingFactsManager.OnPlaying -= new BlowingFactsManager.RoundUpdate(UIUpdate);
		BlowingFactsManager.OnCorrect -= new BlowingFactsManager.ScoreUpdate(UpdateScore);
	}

	// Update is called once per frame
	void UIUpdate (float time) 
	{
		int timer = (int)time;
		timeText.text = timer.ToString();		
	}
	
	void UpdateScore(int savedTime, int numCorrect)
	{
		savedTimeText.text = savedTime.ToString();
		correctText.text = numCorrect.ToString();
	}
	
	void QuestionIn(int x, int y, int answers)
	{
		equationText.text = x.ToString() + " x " + y.ToString() + " = ?";
		
		if(OnRecieved != null)
			OnRecieved(answers);
	}
}
