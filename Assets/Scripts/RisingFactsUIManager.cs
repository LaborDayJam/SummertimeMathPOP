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
	public Text nameText;
	public Text timeText;
	public Text roundText;
	public Text scoreText;

	// Use this for initialization
	void Awake () 
	{
		MathGenerator.OnGetQuestion += new MathGenerator.QuestionOut(QuestionIn);
		RisingFactsManager.OnPlaying += new RisingFactsManager.RoundUpdate(UIUpdate);
	}

	void OnDestroy()
	{
		MathGenerator.OnGetQuestion -= new MathGenerator.QuestionOut(QuestionIn);
		RisingFactsManager.OnPlaying -= new RisingFactsManager.RoundUpdate(UIUpdate);
	}

	// Update is called once per frame
	void UIUpdate (int round, int score, float time, bool boy) 
	{
		int timer = (int)time;
		timeText.text = timer.ToString();
		roundText.text = round.ToString();
		scoreText.text = score.ToString();
	}

	void QuestionIn(int x, int y, int answers)
	{
		equationText.text = x.ToString() + " x " + y.ToString() + " = ?";

		if(OnRecieved != null)
			OnRecieved(answers);
	}
}
