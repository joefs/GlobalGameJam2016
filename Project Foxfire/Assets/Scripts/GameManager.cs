using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager globalInstance;


	[SerializeField]
	GameObject m_p1SpawnLocation;

	[SerializeField]
	GameObject m_p2SpawnLocation;

	[SerializeField]
	GameObject m_pShrine;

	[SerializeField]
	Transform fairyPrefab;

	bool m_isShrineOpen;

	[SerializeField]
	Transform playerPrefab;


	PlayerController p1Contr;
	PlayerController p2Contr;

	[SerializeField]
	UnityEngine.UI.Text p1LevelLabel;

	[SerializeField]
	UnityEngine.UI.Text p2LevelLabel;
	


	// Use this for initialization
	void Start () {
		globalInstance = this;

		Transform t = (Transform)Instantiate(playerPrefab, m_p1SpawnLocation.transform.position, Quaternion.identity);
		t.gameObject.GetComponent<PlayerController>().m_playerNumber = (PlayerNumber)0;
		p1Contr = t.gameObject.GetComponent<PlayerController>();
		SmashCamScript.AddTrackChar(t.gameObject);



		t = (Transform)Instantiate(playerPrefab, m_p2SpawnLocation.transform.position, Quaternion.identity);
		t.gameObject.GetComponent<PlayerController>().m_playerNumber = (PlayerNumber)1;
		p2Contr = t.gameObject.GetComponent<PlayerController>();
		SmashCamScript.AddTrackChar(t.gameObject);

		m_isShrineOpen = false;

		fairies = new GameObject[6];
		SpawnFairies();
	
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if(Input.GetKeyUp(KeyCode.G))
		{
			m_isShrineOpen = !m_isShrineOpen;
			if(m_isShrineOpen)Debug.Log("Shrine Open");
			else Debug.Log("Shrine Open");
		}
		*/
	}


	public void OpenShrine()
	{
		m_isShrineOpen = true;
	}


	private void CloseShrine()
	{
		m_isShrineOpen = false;
	}

	public bool IsShrineOpen()
	{
		return m_isShrineOpen;
	}

	public void PrayAtShrine(PlayerController pPlayer)
	{
		pPlayer.LevelUp();
		if(pPlayer.m_playerNumber == PlayerNumber.P1 && p1LevelLabel!=null)
		{
			 p1LevelLabel.text = "P1 Lvl: " + pPlayer.GetLevel();
		}
		else if(pPlayer.m_playerNumber == PlayerNumber.P2 && p1LevelLabel!=null)
		{
			 p2LevelLabel.text = "P2 Lvl: " + pPlayer.GetLevel();
		}
		p1Contr.Respawn_COMMAND();
		p1Contr.ResetFairies();
		p2Contr.Respawn_COMMAND();
		p2Contr.ResetFairies();
		KillFairies();
		CloseShrine();
		SpawnFairies();
	}

	GameObject[] fairies;

	void SpawnFairies()
	{
		for( int i = 0; i< fairies.Length; i++)
		{
			Transform t = (Transform)Instantiate(fairyPrefab, Vector3.zero, Quaternion.identity);
			fairies[i] = t.gameObject;
			if(i<2)
			{
				fairies[i].GetComponent<FairyScript>().m_Color = FairyColor.BLUE;
				fairies[i].GetComponent<Renderer>().material.color = Color.blue;
			}
			else if(i<4)
			{
				fairies[i].GetComponent<Renderer>().material.color = Color.red;
				fairies[i].GetComponent<FairyScript>().m_Color = FairyColor.RED;
			}
			else
			{
				fairies[i].GetComponent<FairyScript>().m_Color = FairyColor.GREEN;
				fairies[i].GetComponent<Renderer>().material.color = Color.green;
			}
		}
	}

	void KillFairies()
	{
		for( int i = 0; i< fairies.Length; i++)
		{
			Destroy(fairies[i]);
		}
	}

}
