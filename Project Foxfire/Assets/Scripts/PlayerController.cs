using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		m_velocity = unitVelo.normalized * speed * speedMultiplier;

		if(Input.GetKeyUp(KeyCode.Space) )LevelUp();

	}

	void Move()
	{
		gameObject.transform.position += m_velocity * Time.deltaTime;
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
		Debug.Log(speedMultiplier);
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
			Debug.Log("GUST");

			Transform t = (Transform)Instantiate(gustPrefab, gameObject.transform.position - m_directionVector[facingDirection], Quaternion.identity);
			GameObject currentGust = t.gameObject;
		}

		void InvisibleGust()
		{
			Debug.Log("INVISIBLE GUST");

			Transform t = (Transform)Instantiate(gustPrefab, gameObject.transform.position - m_directionVector[facingDirection], Quaternion.identity);
			GameObject currentGust = t.gameObject;
			currentGust.GetComponent<GustScript>().GoTransparent();
		}

	void UseElementFire()
	{
		if(fireFunc!=null) fireFunc.Invoke();
		else Debug.Log("NO FIRE FUNCTION");
	}



		void Fire()
		{
			Debug.Log("FIRE");

			Transform t = (Transform)Instantiate(firePrefab, gameObject.transform.position + m_directionVector[facingDirection], Quaternion.identity);
			GameObject currentFireball = t.gameObject;
			currentFireball.GetComponent<FireballScript>().Launch(m_directionVector[facingDirection], 1.0f);
		}

		void Fireball()
		{
			Debug.Log("FIREBALL");
			Transform t = (Transform)Instantiate(firePrefab, gameObject.transform.position + m_directionVector[facingDirection], Quaternion.identity);
			GameObject currentFireball = t.gameObject;
			currentFireball.GetComponent<FireballScript>().Launch(m_directionVector[facingDirection], 2.0f, 20f);
		}

	void UseElementEarth()
	{
		if(earthFunc!=null)earthFunc.Invoke();
		else Debug.Log("NO EARTH FUNCTION");
	}	

		void Grow()
		{
			Debug.Log("GROW");
			Transform t = (Transform)Instantiate(rockPrefab, gameObject.transform.position + m_directionVector[facingDirection], Quaternion.identity);
			GameObject currentTree = t.gameObject;
			currentTree.GetComponent<RockScript>().ConvertToTree();
		}

		void Rock()
		{
			Debug.Log("ROCK");
			Transform t = (Transform)Instantiate(rockPrefab, gameObject.transform.position + m_directionVector[facingDirection], Quaternion.identity);
			GameObject currentRock = t.gameObject;
			currentRock.GetComponent<RockScript>().ConvertToRock();
		}

	void UseElementWater()
	{
		if(waterFunc!=null)waterFunc.Invoke();
		else Debug.Log("NO WATER FUNCTION");
	}

		void Wave()
		{
			Debug.Log("WAVE");

			Transform t = (Transform)Instantiate(wavePrefab, gameObject.transform.position + m_directionVector[facingDirection], Quaternion.identity);
			GameObject currentWave = t.gameObject;
			currentWave.GetComponent<WaveScript>().Launch(m_directionVector[facingDirection], 3.0f);
			currentWave.GetComponent<WaveScript>().Launch(-1*m_directionVector[facingDirection], 3.0f);
		}

		void Flood()
		{
			Debug.Log("FLOOD");

			Transform t = (Transform)Instantiate(wavePrefab, gameObject.transform.position + m_directionVector[facingDirection], Quaternion.identity);
			GameObject currentWave = t.gameObject;
			currentWave.GetComponent<WaveScript>().Launch(m_directionVector[facingDirection], 3.0f);
			t = (Transform)Instantiate(wavePrefab, gameObject.transform.position + m_directionVector[facingDirection], Quaternion.identity);
			currentWave = t.gameObject;
			currentWave.GetComponent<WaveScript>().Launch(-1*m_directionVector[facingDirection], 3.0f);
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
			Debug.Log("ACCESSED");
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
    }

}
