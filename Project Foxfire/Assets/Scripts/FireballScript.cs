using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour {

	Vector3 velocity = Vector3.zero;

	Vector3 initialPos;

	float maxDist;

	// Use this for initialization
	void Start ()
	{
		initialPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		gameObject.transform.position += velocity * Time.deltaTime;
		if(Vector3.Distance(gameObject.transform.position, initialPos) > maxDist) Kill();

	}

	public void Launch(Vector3 pVec, float pSpeed, float newMaxDist = 3.0f)
	{
		velocity = pVec *  pSpeed;
		maxDist = newMaxDist;
	}


    void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.tag == "Obstacle")
        {
            Kill();
        }
        else if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().Respawn_COMMAND();

            Destroy(gameObject);
        }
    }

	void Kill()
	{
		Destroy(gameObject);
	}

	void FixedUpdate()
	{
		GameObject go = WhatsThere(5.0f);
		if(go!= null&&(go.tag=="Flood"||go.tag=="Wave"))
		{
			Kill();
		}
	}


    GameObject WhatsThere(float pDist)
    {
    	GameObject retVal = null;
	    RaycastHit hit;
        Ray ray = new Ray(gameObject.transform.position + (velocity*Time.deltaTime), Quaternion.Euler(0,90,0 ) * velocity );// origin, dir
        //Debug.Log("Ray is " + velocity);
        Debug.DrawLine(gameObject.transform.position + (velocity*Time.deltaTime), gameObject.transform.position + (velocity.normalized * pDist), Color.green, 0.1f, false);
	    if (Physics.Raycast(ray, out hit))
	    {
	        if (hit.collider != null )
	        {
	        	//Debug.Log("An object was hit and it was: " + hit.collider.gameObject);
	        	 if(Vector3.Distance(gameObject.transform.position, hit.collider.gameObject.transform.position) < pDist)retVal = hit.collider.gameObject;
	        }
	    }
	    return retVal;
    }


}
