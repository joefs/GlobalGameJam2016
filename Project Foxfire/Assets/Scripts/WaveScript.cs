using UnityEngine;
using System.Collections;

public class WaveScript : MonoBehaviour {

	Vector3 velocity = Vector3.zero;

	Vector3 initialPos;

	float maxDist;

	// Use this for initialization
	void Start () {
		initialPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		gameObject.transform.position += velocity * Time.deltaTime;
		if(Vector3.Distance(gameObject.transform.position, initialPos) > maxDist) Kill();

	}



	public void Launch(Vector3 pVec, float pSpeed, float newMaxDist = 100.0f)
	{
		velocity = pVec *  pSpeed;
		maxDist = newMaxDist;
	}


    void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            // col.gameObject
            // set the player gameobjects parent to this
            // set a timer for it to be freed
            col.gameObject.transform.parent = gameObject.transform;
            DeparentInTime();
        }
    }

	void Kill()
	{
		gameObject.transform.DetachChildren();
		Destroy(gameObject);
	}

	void DeparentInTime()
	{
		StartCoroutine(DeparentAfter(1.5f));
	}

	IEnumerator DeparentAfter(float pTime)
	{
		yield return new WaitForSeconds(pTime);
		gameObject.transform.DetachChildren();
	}

}
