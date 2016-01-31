using UnityEngine;
using System.Collections;
using System;

public class FloodScript : MonoBehaviour {

	float m_initialScale;
	GameObject m_owner;

	// Use this for initialization
	void Start () {
		m_initialScale = transform.localScale.x;
		InitiateExpand();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator Expand(Func<float, float> pExpansionFunction, float finalRadius, float expansionTime)
	{
		float elapsedTime = 0.0f;
		float curRad = 0.0f;
		float percentThrough;
		while(elapsedTime<expansionTime)
		{
			percentThrough = elapsedTime/expansionTime;
			curRad = Mathf.Lerp(m_initialScale,finalRadius,pExpansionFunction(percentThrough));
			transform.localScale = new Vector3(curRad, 0.1f, curRad);
			elapsedTime += Time.deltaTime; 

			if(GetComponent<Renderer>().material.color != Color.clear && elapsedTime<expansionTime) GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.clear, pExpansionFunction(percentThrough) );
			// this seemingly roundabout method targets the inidividuated copy of the shader

			yield return new WaitForEndOfFrame();
		}
		transform.localScale = new Vector3(finalRadius, 0.1f, finalRadius);
		GetComponent<Renderer>().material.color = Color.clear;
		Destroy(gameObject);
	}

	public void InitiateExpand()
	{
		StartCoroutine(Expand( (float f) => {return f;},100,3.0f));
	}

	public void RegisterOwner(GameObject pGO)
	{
		m_owner = pGO;
	}

	public bool IsOwnedBy(GameObject pGO)
	{
		if(pGO == m_owner) return true;
		else return false;
	}
}

