using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum PlayerNumber{P1, P2};

public enum Direction{UP, LEFT, DOWN, RIGHT};


public class PlayerController : MonoBehaviour {
	public static int WINNING_COUNT = 9;

	Vector3 m_velocity;
	[SerializeField]
	private float speed = 1.0f;

	private float speedMultiplier =1.0f;

	[SerializeField]
	private Direction facingDirection = Direction.LEFT;

	[SerializeField]
	private int tailCount = 1;
	[SerializeField]
	private int currentPower = 0;

	[SerializeField]
	private Transform firePrefab;


	[SerializeField]
	private Transform rockPrefab;

	[SerializeField]
	private Transform gustPrefab;

	[SerializeField]
	private Transform wavePrefab;

	[SerializeField]
	private Transform floodPrefab;


/*
	CooldownItem m_fireWeapon;
	CooldownItem m_earthWeapon;
	CooldownItem m_waterWeapon;
	CooldownItem m_windWeapon;
*/




	Dictionary<Direction, Vector3> m_directionVector;


	[SerializeField]
	private PlayerNumber m_playerNumber;
	// Use this for initialization
	void Start ()
	{
		m_velocity = Vector3.zero;

		windFunc = Gust;// ONLY ONE SET BECAUSE IT LEVEL 1
		m_directionVector = new Dictionary<Direction, Vector3>();
		m_directionVector.Add(Direction.UP, new Vector3(0,0,1));
		m_directionVector.Add(Direction.DOWN, new Vector3(0,0,-1));
		m_directionVector.Add(Direction.LEFT, new Vector3(-1,0,0));
		m_directionVector.Add(Direction.RIGHT, new Vector3(1,0,0));






	}
	
	// Update is called once per frame
	void Update ()
	{
		HandleInput();
		Move();
		UpdateDisplay();
		m_velocity = Vector3.zero;
	}

	void HandleInput()
	{
		Vector3 unitVelo = Vector2.zero;
		if(m_playerNumber == PlayerNumber.P1)
		{
			if (Input.GetKey(KeyCode.I)) {unitVelo.z+=1; facingDirection = Direction.UP; }
			else if (Input.GetKey(KeyCode.J)) {unitVelo.x-=1; facingDirection = Direction.LEFT; }
			else if (Input.GetKey(KeyCode.K)) {unitVelo.z-=1; facingDirection = Direction.DOWN; }
			else if (Input.GetKey(KeyCode.L)) {unitVelo.x+=1; facingDirection = Direction.RIGHT; }


			if(Input.GetKeyUp(KeyCode.Alpha7)) UseElementWind();
			else if(Input.GetKeyUp(KeyCode.Alpha8)) UseElementFire();
			else if(Input.GetKeyUp(KeyCode.Alpha9)) UseElementEarth();
			else if(Input.GetKeyUp(KeyCode.Alpha0)) UseElementWater();


		}
		else if(m_playerNumber == PlayerNumber.P2)
		{
			if (Input.GetKey(KeyCode.W)) {unitVelo.z+=1; facingDirection = Direction.UP; }
			else if (Input.GetKey(KeyCode.A)) {unitVelo.x-=1; facingDirection = Direction.LEFT; }
			else if (Input.GetKey(KeyCode.S)) {unitVelo.z-=1; facingDirection = Direction.DOWN; }
			else if (Input.GetKey(KeyCode.D)) {unitVelo.x+=1; facingDirection = Direction.RIGHT; }

			if(Input.GetKeyUp(KeyCode.Alpha1)) UseElementWind();
			else if(Input.GetKeyUp(KeyCode.Alpha2)) UseElementFire();
			else if(Input.GetKeyUp(KeyCode.Alpha3)) UseElementEarth();
			else if(Input.GetKeyUp(KeyCode.Alpha4)) UseElementWater();

		}
		if(IsDirectionFreeOfObstacles(m_directionVector[facingDirection]) && m_isPinned == false)
		{
			m_velocity = (unitVelo.normalized * speed * speedMultiplier) + m_pushVec;	
		}
		else
		{
			m_velocity = Vector3.zero;
		}

		if(Input.GetKeyUp(KeyCode.Space) )LevelUp();

	}

	void Move()
	{
		gameObject.transform.position += m_velocity * Time.deltaTime;
	}


	bool IsDirectionFreeOfObstacles(Vector3 pRayDir)
	{
		bool retVal = true;
		Vector3 sourcePoint = gameObject.transform.position+ (m_directionVector[facingDirection]/2.0f);
        Ray ray = new Ray(sourcePoint, m_directionVector[facingDirection]);// origin, dir
        RaycastHit hit;

        float fractionalDist = speed/15.0f;

       // Debug.DrawLine(sourcePoint, gameObject.transform.position+ (m_directionVector[facingDirection].normalized * fractionalDist), Color.green, 0.1f, false);
        if (Physics.Raycast(ray, out hit, 100)  && hit.rigidbody!= null && hit.rigidbody.gameObject.tag=="Obstacle" && Vector3.Distance(sourcePoint, hit.rigidbody.gameObject.transform.position) < fractionalDist)
        {
        	//Debug.Log("OBSTACLE IN THAT Direction");
        	retVal = false;
        }
		return retVal;
	}

	void UpdateDisplay()
	{
		//Change to animation Cycle based on direction
		ChangeToAnimState(facingDirection);
	}

	void ChangeToAnimState(Direction pDirection)
	{
		// change to directional animation track and pause if not moving
	}

	void CyclePowers()
	{
		currentPower = (currentPower + 1) % tailCount;
	}

	void LevelUp()
	{
		tailCount++;
		if(tailCount==WINNING_COUNT) Debug.Log("YOU WIN");//END GAME

		speedMultiplier = Mathf.Lerp( 1,0.5f, (tailCount-1)/8.0f);
		//Debug.Log(speedMultiplier);
		switch(tailCount)
		{
			case 2:
				windFunc = InvisibleGust;
				break;
			case 3:
				fireFunc = Fire;
				break;
			case 4:
				fireFunc = Fireball;
				break;
			case 5:
				earthFunc = Grow;
				break;
			case 6:
				earthFunc = Rock;
				break;
			case 7:
				waterFunc = Wave;
				break;
			case 8:
				waterFunc = Flood;
				break;
		}

	}

	System.Action windFunc = null;
	System.Action fireFunc = null;
	System.Action earthFunc = null;
	System.Action waterFunc = null;

	void UseElementWind()
	{
		windFunc.Invoke();
	}


		void Gust()
		{
			//Debug.Log("GUST");

			GameObject go = WhatsThere(gameObject.transform.position, (Direction)((((int)facingDirection)+2)%4), 2.0f);
			if(go== null||(go.tag!="Gust"&&go.tag!="Obstacle"))
			{
				Transform t = (Transform)Instantiate(gustPrefab, gameObject.transform.position - m_directionVector[facingDirection], Quaternion.identity);
				GameObject currentGust = t.gameObject;
				Debug.Log(go);
			}
			else
			{
				Debug.Log("FAILED TO PLACE");
			}
		}

		void InvisibleGust()
		{
			//Debug.Log("INVISIBLE GUST");
			GameObject go = WhatsThere(gameObject.transform.position, (Direction)((((int)facingDirection)+2)%4), 2.0f);
			if(go== null||(go.tag!="Gust"&&go.tag!="Obstacle"))
			{
				Transform t = (Transform)Instantiate(gustPrefab, gameObject.transform.position - m_directionVector[facingDirection], Quaternion.identity);
				GameObject currentGust = t.gameObject;
				currentGust.GetComponent<GustScript>().GoTransparent();
			}
			else
			{
				Debug.Log("FAILED TO PLACE");
			}
		}

	void UseElementFire()
	{
		if(fireFunc!=null) fireFunc.Invoke();
		else Debug.Log("NO FIRE FUNCTION");
	}



		void Fire()
		{
			//Debug.Log("FIRE");

			Transform t = (Transform)Instantiate(firePrefab, gameObject.transform.position + m_directionVector[facingDirection], Quaternion.identity);
			GameObject currentFireball = t.gameObject;
			currentFireball.GetComponent<FireballScript>().Launch(m_directionVector[facingDirection], 10.0f, 4.0f);
		}

		void Fireball()
		{
			//Debug.Log("FIREBALL");
			Transform t = (Transform)Instantiate(firePrefab, gameObject.transform.position + m_directionVector[facingDirection], Quaternion.identity);
			GameObject currentFireball = t.gameObject;
			currentFireball.GetComponent<FireballScript>().Launch(m_directionVector[facingDirection], 25.0f, 40f);
		}

	void UseElementEarth()
	{
		if(earthFunc!=null)earthFunc.Invoke();
		else Debug.Log("NO EARTH FUNCTION");
	}	

		void Grow()
		{



			//Debug.Log("GROW");

			GameObject go = WhatsThere(gameObject.transform.position, facingDirection, 2.0f);
			if(go== null||(go.tag!="Gust"&&go.tag!="Obstacle"))
			{
				Transform t = (Transform)Instantiate(rockPrefab, gameObject.transform.position + m_directionVector[facingDirection], Quaternion.identity);
				GameObject currentTree = t.gameObject;
				currentTree.GetComponent<RockScript>().ConvertToTree();
			}
			else
			{
				Debug.Log("FAILED TO PLACE");
			}
		}

		void Rock()
		{
			Debug.Log("ROCK");

			GameObject go = WhatsThere(gameObject.transform.position, facingDirection, 2.0f);
			if(go== null||(go.tag!="Gust"&&go.tag!="Obstacle"))
			{
				Transform t = (Transform)Instantiate(rockPrefab, gameObject.transform.position + m_directionVector[facingDirection], Quaternion.identity);
				GameObject currentRock = t.gameObject;
				currentRock.GetComponent<RockScript>().ConvertToRock();
			}
			else
			{
				Debug.Log("FAILED TO PLACE");
			}
		}

	void UseElementWater()
	{
		if(waterFunc!=null)waterFunc.Invoke();
		else Debug.Log("NO WATER FUNCTION");
	}

		void Wave()
		{
			//Debug.Log("WAVE");
			for(int i = 0; i<4; i++)
			{
				Transform t = (Transform)Instantiate(wavePrefab, gameObject.transform.position + m_directionVector[facingDirection], Quaternion.Euler(0, 90*(1+(int)facingDirection), 0));
				GameObject currentWave = t.gameObject;
				currentWave.GetComponent<WaveScript>().Launch(m_directionVector[facingDirection], 3.0f);
			}

		}

		void Flood()
		{
			//Debug.Log("FLOOD");
			Transform t = (Transform)Instantiate(floodPrefab, gameObject.transform.position, Quaternion.identity);
			GameObject currentFlood = t.gameObject;
			currentFlood.GetComponent<FloodScript>().RegisterOwner(gameObject);
		}


	void Respawn()
	{

	}

	public void Respawn_COMMAND()
	{
		Debug.Log("CHARACTER SHOULD BE SENT BACK");
	}

	void OnCollisionExit(Collision collisionInfo) {
		if(collisionInfo.gameObject.tag == "Obstacle" || collisionInfo.gameObject.tag == "Player")
		{
			//Debug.Log("ACCESSED");
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
    }

    void OnCollisionEnter(Collision collisionInfo) {
		if(collisionInfo.gameObject.tag == "Obstacle")
		{
			if(!m_isPinned)
			{
				//Debug.LogError("INTERSECTED " + " with"+collisionInfo.gameObject.name + " @"+ Time.frameCount);
				Vector3 diff = collisionInfo.gameObject.transform.position - gameObject.transform.position;
				// push out

				//Debug.Log("AT FIRST: " + gameObject.transform.position);
				if( Mathf.Abs(diff.x) > Mathf.Abs(diff.z) )
				{
					gameObject.transform.position += new Vector3(0.5f,0,0) * ((diff.x<=0)? 1: -1) ; 
				}
				else
				{
					gameObject.transform.position += new Vector3(0,0,0.5f) * ((diff.z<=0)? 1: -1) ;
				}
				//Debug.Log("AT LAST: " + gameObject.transform.position);
			}
			if(m_isBeingPushed)
			{
				m_isPinned = true;
			}
		}
    }


	void OnTriggerEnter (Collider col)
    {
		if(col.gameObject.tag == "Flood" && col.gameObject.GetComponent<FloodScript>().IsOwnedBy(gameObject) == false)
		{
			//Debug.LogError("FLOOD HIT SOMEONE");
			COMMAND_ImpartPush(col.gameObject.transform.position, 3.0f, 3.0f);
		}
		else if( col.gameObject.tag == "Wave" )
		{
			//Debug.LogError("Wave HIT SOMEONE");
			COMMAND_ImpartCardinalPush (col.gameObject.transform.position, 1.0f, 8.0f);
		}
    }

    private bool m_isBeingPushed = false;
    private bool m_isPinned = false;

    Vector3 m_pushVec;

    public void COMMAND_ImpartPush(Vector3 pSource, float pTime, float pPushSpeed)
    {
    	if(m_isBeingPushed==false)
    	{
    		Func<float, float> pPushFunc =(float r)=>{ return Mathf.Sin(r * Mathf.PI);};
    		StartCoroutine(ImpartPush(pSource,pPushFunc, pTime, pPushSpeed));
    	}
    }

    private IEnumerator ImpartPush(Vector3 pSource, Func<float, float> pPushFunc, float pTime, float pPushSpeed)
    {
    	float elapsedTime = 0.0f;
    	m_isBeingPushed = true;
    	while(elapsedTime<pTime)
    	{
    		elapsedTime+= Time.deltaTime;
    		Vector3 diff = gameObject.transform.position - pSource;
    		m_pushVec = diff.normalized * pPushSpeed * pPushFunc(elapsedTime/pTime);
    		yield return new WaitForEndOfFrame();
    	}
    	m_pushVec = Vector3.zero;
    	m_isBeingPushed = false;
    	m_isPinned = false;
    }

    public void COMMAND_ImpartCardinalPush(Vector3 pSource, float pTime, float pPushSpeed)
    {
    	if(m_isBeingPushed==false)
    	{
    		Func<float, float> pPushFunc =(float r)=>{ return Mathf.Sin(r * Mathf.PI);};
    		StartCoroutine(ImpartCardinalPush(pSource,pPushFunc, pTime, pPushSpeed));
    	}
    }

    private IEnumerator ImpartCardinalPush(Vector3 pSource, Func<float, float> pPushFunc, float pTime, float pPushSpeed)
    {
    	float elapsedTime = 0.0f;
    	m_isBeingPushed = true;
    	while(elapsedTime<pTime)
    	{
    		elapsedTime+= Time.deltaTime;
    		Vector3 diff =  pSource-gameObject.transform.position;


				if( Mathf.Abs(diff.x) > Mathf.Abs(diff.z) )
				{
					diff= new Vector3(0.5f,0,0) * ((diff.x<=0)? 1: -1) ; 
				}
				else
				{
					diff= new Vector3(0,0,0.5f) * ((diff.z<=0)? 1: -1) ;
				}



    		m_pushVec = diff.normalized * pPushSpeed * pPushFunc(elapsedTime/pTime);
    		yield return new WaitForEndOfFrame();
    	}
    	m_pushVec = Vector3.zero;
    	m_isBeingPushed = false;
    	m_isPinned = false;
    }    

    GameObject WhatsThere(Vector3 pSource, Direction pDir, float pDist)
    {
    	GameObject retVal = null;
	    RaycastHit hit;
        Ray ray = new Ray(pSource, m_directionVector[pDir]);// origin, dir
	    if (Physics.Raycast(ray, out hit))
	    {
	        if (hit.collider != null && Vector3.Distance(gameObject.transform.position, hit.collider.gameObject.transform.position) < pDist) retVal = hit.collider.gameObject;
	    }
	    return retVal;
    }

}

[System.Serializable]
public class CooldownItem
{
	int m_currentStacks;
	int m_maxStacks;
	float m_loadTime;
	bool CanFire()
	{
		if(m_currentStacks>0) return true;
		return false;
	}
	void Recharge()
	{
		if(m_currentStacks<m_maxStacks)m_currentStacks++;
		else m_currentStacks = m_maxStacks;
	}
	void Discharge()
	{
		m_currentStacks--;
		if(m_currentStacks<0) m_currentStacks = 0;
	}

	void Fire( System.Action pAction)
	{
		if(CanFire())
		{
			Discharge();
			pAction.Invoke();
		}
	}
}