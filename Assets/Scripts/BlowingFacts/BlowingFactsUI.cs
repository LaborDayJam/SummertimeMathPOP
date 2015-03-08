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
	public Text levelText;
	public Text correctText;
	
	private GameSettings gameSettings;	
	private int score;
	
	// Use this for initialization
	void Awake () 
	{
		MathGenerator.OnGetQuestion += new MathGenerator.QuestionOut(QuestionIn);
		BlowingFactsManager.OnPlaying += new BlowingFactsManager.RoundUpdate(UIUpdate);		
	}
	void Start()
	{
		gameSettings = GameSettings.instance;
	}
	void OnDestroy()
	{
		MathGenerator.OnGetQuestion -= new MathGenerator.QuestionOut(QuestionIn);
		BlowingFactsManager.OnPlaying -= new BlowingFactsManager.RoundUpdate(UIUpdate);
	
	}
	
	// Update is called once per frame
	void UIUpdate (float time) 
	{
		int timer = (int)time;
		timeText.text = timer.ToString();
		levelText.text = gameSettings.PlayerLevel.ToString();
		
	}
	
	void UpdateScore(bool right)
	{
		score += 1;
		correctText.text = score.ToString();

	}
	
	void QuestionIn(int x, int y, int answers)
	{
		equationText.text = x.ToString() + " x " + y.ToString() + " = ?";
		
		if(OnRecieved != null)
			OnRecieved(answers);
	}
}
