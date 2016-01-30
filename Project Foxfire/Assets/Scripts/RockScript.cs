using UnityEngine;
using System.Collections;

public class RockScript : MonoBehaviour {

	int health = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ConvertToTree()
	{
		health = 1;
	}

	public void ConvertToRock()
	{
		health = 3;
	}

	void TakeDamage()
	{
		health--;
		if(health<=0) Destroy(gameObject);
	}


    void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.tag == "Fireball")
        {
    		Debug.Log("ARF" + Time.frameCount);
            TakeDamage();
        }
    }

}
