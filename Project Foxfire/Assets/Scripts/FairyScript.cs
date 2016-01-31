using UnityEngine;
using System.Collections;

public enum FairyColor{BLUE, GREEN, RED};


public class FairyScript : MonoBehaviour {

	System.Action movementMethod;

	[SerializeField]
	float upperZ;
	[SerializeField]
	float lowerZ;
	[SerializeField]
	float upperX;
	[SerializeField]
	float lowerX;

	float initialY;

	public FairyColor m_Color;

	float initialDifferential;

	float multiplier;


	// Use this for initialization
	void Start () {
		initialY = gameObject.transform.position.y;
		movementMethod = FairyMoveA;
		initialDifferential = Random.Range(-1.0f,1.0f);
		multiplier = Random.Range(-4.0f,4.0f);

	}
	
	// Update is called once per frame
	void Update () {
		if(movementMethod!=null) movementMethod.Invoke();
	}


	void FairyMoveA()
	{
		float f = (Time.frameCount / 600.0f) + initialDifferential;

		float newX = Mathf.Lerp(lowerX,upperX,Mathf.PerlinNoise(initialDifferential,f));


		float newZ = Mathf.Lerp(lowerZ,upperZ,Mathf.PerlinNoise(multiplier*f,f)) ;

		gameObject.transform.position = new Vector3 (newX, initialY,newZ);
	}
}
