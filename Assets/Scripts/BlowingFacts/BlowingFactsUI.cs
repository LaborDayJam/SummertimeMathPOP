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
		Bubble.OnPop += new Bubble.BubbleEvent(UpdateScore);
		
	}
	void Start()
	{
		gameSettings = GameSettings.instance;
	}
	void OnDestroy()
	{
		MathGenerator.OnGetQuestion -= new MathGenerator.QuestionOut(QuestionIn);
		BlowingFactsManager.OnPlaying -= new BlowingFactsManager.RoundUpdate(UIUpdate);
		Bubble.OnPop -= new Bubble.BubbleEvent(UpdateScore);
	}
	
	// Update is called once per frame
	void UIUpdate (float time) 
	{
		int timer = (int)time;
		timeText.text = timer.ToString();
		levelText.text = gameSettings.PlayerLevel.ToString();
		
	}
	
	void UpdateScore(bool theOne)
	{
		if(theOne)
			score += 5 * gameSettings.PlayerLevel;
		
		correctText.text = score.ToString();
	}
	
	void QuestionIn(int x, int y, int answers)
	{
		equationText.text = x.ToString() + " x " + y.ToString() + " = ?";
		
		if(OnRecieved != null)
			OnRecieved(answers);
	}
}
