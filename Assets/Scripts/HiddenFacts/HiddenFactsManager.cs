using UnityEngine;
using System.Collections;

public class HiddenFactsManager : MonoBehaviour 
{

	public delegate void GameUpdate(int state);
	public static event GameUpdate OnUpdate;
	public static event GameUpdate OnReset;
	
	public delegate void RoundUpdate(int round, int score, float time, bool boy);
	public static event RoundUpdate OnPlaying;
	
	
	// 
	void Awake()
	{
	
	}
	
	
	void OnDestroy()
	{
		
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
