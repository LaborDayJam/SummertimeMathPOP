using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HiddenFactsUI : MonoBehaviour {

	public delegate void AnswerOut(int answer);
	public static event  AnswerOut OnRecieved;
	
	public delegate void PuaseEvent(bool isPuased);
	public static event PuaseEvent OnPuased;
	
	public Text equationText;
	public Text timeText;
	public Text levelText;
	public Text correctText;
	public Text totalSavedTimeText;


	
	// Use this for initialization
	void Awake () 
	{
		MathGenerator.OnGetQuestion += new MathGenerator.QuestionOut(QuestionIn);
		HiddenFactsManager.OnPlaying += new HiddenFactsManager.RoundUpdate(UIUpdate);
		HiddenFactsManager.OnCorrect += new HiddenFactsManager.ScoreUpdate(UpdateScore);
	}

	void OnDestroy()
	{
		MathGenerator.OnGetQuestion -= new MathGenerator.QuestionOut(QuestionIn);
		HiddenFactsManager.OnPlaying -= new HiddenFactsManager.RoundUpdate(UIUpdate);
		HiddenFactsManager.OnCorrect -= new HiddenFactsManager.ScoreUpdate(UpdateScore);
	}

	void Start()
	{
		levelText.text = GameSettings.instance.PlayerLevel.ToString();
	}
	
	// Update is called once per frame
	void UIUpdate (float time) 
	{
		int timer = (int)time;
		timeText.text = timer.ToString();
	}
	
	void UpdateScore(int timeSaved, int numCorrect)
	{
		totalSavedTimeText.text = timeSaved.ToString();
		correctText.text = numCorrect.ToString();
	}
	
	void QuestionIn(int x, int y, int answers)
	{
		equationText.text = x.ToString() + " x " + y.ToString() + " = ?";
		
		if(OnRecieved != null)
			OnRecieved(answers);
	}
}