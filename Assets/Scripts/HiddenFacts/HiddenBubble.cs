using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HiddenBubble : MonoBehaviour 
{
	
	public delegate void BubbleEvent(bool theOne);
	public static event BubbleEvent OnPop;
	public  bool 	theOne = false;
	private bool 	isPlaying = true;
	
	void Awake()
	{
		HiddenFactsUI.OnPuased += new HiddenFactsUI.PuaseEvent(OnPuased);
	}
	
	void OnDestroy()
	{
		HiddenFactsUI.OnPuased -= new HiddenFactsUI.PuaseEvent(OnPuased);
	}
	
	void OnPuased(bool playing)
	{
		isPlaying = playing;
	}
	void Update()
	{
	
	}

	public void SetBubble(int bubbleID, int bubbleAnswer, bool correctChoice)
	{
		theOne = correctChoice;
		this.gameObject.GetComponentInChildren<Text>().text = bubbleAnswer.ToString();
	}
	
	public void OnClicked()
	{
		if(OnPop != null)
			OnPop(theOne);
	}
}