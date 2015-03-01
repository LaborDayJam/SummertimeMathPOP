using UnityEngine;
using System.Collections;

public class MainUpdate : MonoBehaviour 
{
	public delegate void TheUpdate();
	public static event TheUpdate OnUpdate;

	void Update()
	{
		if(OnUpdate!=null)
		{
			OnUpdate(); 	// one update to rule them all!
		}
	}
}
