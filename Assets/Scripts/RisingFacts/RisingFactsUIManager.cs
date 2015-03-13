using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RisingFactsUIManager : MonoBehaviour
{
	public delegate void AnswerOut(int answer);
	public static event  AnswerOut OnRecieved;

	public delegate void PuaseEvent(bool isPuased);
	public static event PuaseEvent OnPuased;

	public Text equationText;
	public Text timeText;
	public Text correctText;
	public Text timeSavedText;
	


	// Use this for initialization
	void Awake () 
	{
		MathGenerator.OnGetQuestion += new MathGenerator.QuestionOut(QuestionIn);
		RisingFactsManager.OnPlaying += new RisingFactsManager.RoundUpdate(UIUpdate);
		RisingFactsManager.OnCorrect += new RisingFactsManager.ScoreUpdate(UpdateScore);
	}
	
	void OnDestroy()
	{
		MathGenerator.OnGetQuestion -= new MathGenerator.QuestionOut(QuestionIn);
		RisingFactsManager.OnPlaying -= new RisingFactsManager.RoundUpdate(UIUpdate);
		RisingFactsManager.OnCorrect -= new RisingFactsManager.ScoreUpdate(UpdateScore);
	}

	// Update is called once per frame
	void UIUpdate (float time) 
	{
		int timer = (int)time;
		timeText.text = timer.ToString();
	}
	
	void UpdateScore(int timeSaved, int numCorreect)
	{
		timeSavedText.text = timeSaved.ToString();
		correctText.text = numCorreect.ToString();
	}

	void QuestionIn(int x, int y, int answers)
	{
		equationText.text = x.ToString() + " x " + y.ToString() + " = ?";
		if(OnRecieved != null)
			OnRecieved(answers);
	}
}
