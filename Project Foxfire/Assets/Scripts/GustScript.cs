using UnityEngine;
using System.Collections;

public class GustScript : MonoBehaviour {

	const float DEFAULT_TIME = 1.0f;

	[SerializeField]
	Material mat;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Y))
		{
			GoTransparent();
		}
	}

	IEnumerator GoTransparentCo(float pTime)
	{
		float elapsedTime = 0.0f;
		while(mat.color != Color.clear && elapsedTime<pTime)
		{
			elapsedTime+= Time.deltaTime;
			GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.clear, elapsedTime/pTime );
			// this seemingly roundabout method targets the inidividuated copy of the shader
			yield return new WaitForEndOfFrame();
		}
		GetComponent<Renderer>().material.color = Color.clear;
	}

	public void GoTransparent(float pTime)
	{
		StartCoroutine(GoTransparentCo(pTime));
	}

	public void GoTransparent()
	{
		StartCoroutine(GoTransparentCo(DEFAULT_TIME));
	}

	void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().Respawn_COMMAND();

            Destroy(gameObject);
        }
    }

}
