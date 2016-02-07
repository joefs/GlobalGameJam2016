using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmashCamScript : MonoBehaviour {

	static List<GameObject> trackChars;

	static bool isInitialized = false;

	const float W_TO_H_RATIO = 16.0f/9.0f;

	float maxOrth = 10.0f;

	float minOrth = 3.0f;

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
	float highestHeight = 0.0f;
	float shortestLength = 0.0f;
	float lowestHeight = 0.0f;


	float initialOrthSize;


	float zoomSpeed = 2f;


	public static void AddTrackChar(GameObject pGO)
	{
		if(trackChars == null) trackChars = new List<GameObject>();
		trackChars.Add(pGO);
		isInitialized = true;
	}

	// Use this for initialization
	void Start () {
		initialOrthSize = camGO.GetComponent<Camera>().orthographicSize;
		if(trackChars == null) trackChars = new List<GameObject>();
		initialPos = camGO.transform.position;
		if(lLengthGO != null && rLengthGO != null)
		{
			longestLength = Mathf.Abs((lLengthGO.transform.position - rLengthGO.transform.position).x) + 4;
		}
		if(tHeightGO != null && bHeightGO != null)
		{
			highestHeight = Mathf.Abs((tHeightGO.transform.position-bHeightGO.transform.position).z) + 4;
		}

		shortestLength = longestLength/4.0f;
		lowestHeight = highestHeight/4.0f;

	}
	
	// Update is called once per frame
	void Update () {
		if(isInitialized)
		{
			//ChangeCamPosition();
			//AdjustCamZoom();
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
			if(Vector3.Magnitude(avgSum) > 10.0f) avgSum = avgSum.normalized * 10.0f;
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

	private void AdjustCamZoom()
	{
		// calc max player distance, x, y and determine the normalizing part of things
		// set zoom as interpolation between two floats (the top one being he initial and the bottom being something else)
		// give it a velocity so it eases


		Vector3 avg = Vector3.zero;
		for(int i =0; i< trackChars.Count; i++)
		{
			avg += trackChars[0].transform.position;
		}
		if(trackChars!=null && trackChars.Count!= 0) avg /= (float) trackChars.Count;



		float highPoint = avg.z; 
		float lowPoint = avg.z;
		float leftPoint = avg.x;
		float rightPoint = avg.x;

		Vector3 curPos;
		for(int i = 0; i< trackChars.Count; i++)
		{
			curPos = trackChars[i].transform.position;
			
			if(curPos.x>rightPoint)
			{
				rightPoint = curPos.x;
			}
			if(curPos.x < leftPoint)
			{
				leftPoint = curPos.x;
			}

			if(curPos.z>highPoint)
			{
				highPoint = curPos.z;
			}
			if(curPos.z < lowPoint)
			{
				lowPoint = curPos.z;
			}
			
		}


		float width = (rightPoint - leftPoint) + 2;

		float height = (highPoint - lowPoint)+4;

		float curRatio = width/height;

		float desiredOrthographicSize;

		if(curRatio > W_TO_H_RATIO)
		{
			//length is longer
			if(longestLength == 0) longestLength = 0.01f;
			desiredOrthographicSize = ((width)/longestLength) * (10 * (16.0f/9.0f) );
			//Debug.Log("A");
		}
		else
		{
			// width is longer
			if(highestHeight == 0) highestHeight = 0.01f;
			desiredOrthographicSize =  ((height +8)/highestHeight)* 10;
			//Debug.Log("B");
		}
			if(desiredOrthographicSize > maxOrth) desiredOrthographicSize = maxOrth;
			if(desiredOrthographicSize < minOrth) desiredOrthographicSize = minOrth;


		if(Mathf.Abs(camGO.GetComponent<Camera>().orthographicSize - desiredOrthographicSize) >1.0f)
		{
			if(desiredOrthographicSize > camGO.GetComponent<Camera>().orthographicSize )
			{
				camGO.GetComponent<Camera>().orthographicSize += zoomSpeed * Time.deltaTime;
			}
			else
			{

				camGO.GetComponent<Camera>().orthographicSize -= zoomSpeed * Time.deltaTime;
			}

			//Debug.Log(			desiredOrthographicSize);
		}

	}
}
