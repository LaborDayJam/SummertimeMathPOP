using UnityEngine;
using System.Collections;

public class WandController : MonoBehaviour 
{
	private Vector3 startPosition;
	private Vector3 endinPosition;
	private float 	pushTime ;
	private bool 	isPlaying = true;

	IEnumerator Start()
	{
		pushTime  = 1f;

		startPosition = transform.position;
		float theMove = startPosition.x + (Screen.width/2);
		endinPosition = new Vector3(theMove, startPosition.y, startPosition.z);

		while (true) 
		{
			yield return StartCoroutine(MoveObject(transform, startPosition, endinPosition, pushTime));
			yield return StartCoroutine(MoveObject(transform, endinPosition, startPosition, pushTime));
		}
	}
	
	IEnumerator MoveObject (Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
	{
		if(isPlaying)
		{
			float currTime = 0.0f;
			float rate = 1.0f / time;
			while (currTime < 1.0f) 
			{
				currTime += Time.deltaTime * rate;
				thisTransform.position = Vector3.Lerp(startPos, endPos, currTime);
				yield return null; 
			}
		}
		else
			yield return null;
	}
}
