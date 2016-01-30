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
    }

	void Kill()
	{
		Destroy(gameObject);
	}
}
