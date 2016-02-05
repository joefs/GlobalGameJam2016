using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmashCamScript : MonoBehaviour {

	static List<GameObject> trackChars;

	static bool isInitialized = false;

	const float L_TO_W_RATIO = 16.0f/9.0f;

	Vector3 initialPos;

	[SerializeField]
	GameObject camGO;

	float moveSpeed = 0.5f;

	[SerializeField]
	GameObject lLengthGO;

	[SerializeField]
	GameObject rLengthGO;

	[SerializeField]
	GameObject tHeightGO;

	[SerializeField]
	GameObject bHeightGO;


	float longestLength = 0.0f;
	float highestheight = 0.0f;


	public static void AddTrackChar(GameObject pGO)
	{
		if(trackChars == null) trackChars = new List<GameObject>();
		trackChars.Add(pGO);
		isInitialized = true;
	}

	// Use this for initialization
	void Start () {
		if(trackChars == null) trackChars = new List<GameObject>();
		initialPos = camGO.transform.position;
		if(lLengthGO != null && rLengthGO != null)
		{
			longestLength = Mathf.Abs((lLengthGO.transform.position - rLengthGO.transform.position).x);
		}
		if(tHeightGO != null && bHeightGO != null)
		{
			highestheight = Mathf.Abs((tHeightGO.transform.position-bHeightGO.transform.position).x);
		}

		// calc max player distance, x, y and determine the normalizing part of things
		// set zoom as interpolation between two floats (the top one being he initial and the bottom being something else)
		// give it a velocity so it eases

	}
	
	// Update is called once per frame
	void Update () {
		if(isInitialized)
		{
			ChangeCamPosition();
		}
	}

	private void ChangeCamPosition()
	{
		Vector3 posSum = Vector3.zero;
		Vector3 avgSum = Vector3.zero;
		for(int i = 0; i< trackChars.Count; i++)
		{
			posSum += trackChars[i].transform.position;
		}
		if(trackChars.Count != 0)
		{
			avgSum = posSum/((float)trackChars.Count);
		}

		//Debug.Log(Vector3.Distance(camGO.transform.position, avgSum));

		if(Vector3.Distance(camGO.transform.position - initialPos, avgSum) > 1.0)
		{
			Vector3 diff = avgSum - (camGO.transform.position - initialPos);
			Vector3 moveVec = diff.normalized * Time.deltaTime * 7;
			moveVec = new Vector3(moveVec.x, 0, moveVec.z);
			camGO.transform.position +=  moveVec;
		}
	}
}
