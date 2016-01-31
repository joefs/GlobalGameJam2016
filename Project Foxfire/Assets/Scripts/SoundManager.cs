using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

	[SerializeField]
	string[] audioFileNames;

	Dictionary<string, AudioSource> ASDict;

	// Use this for initialization
	void Start () {
		ASDict = new Dictionary<string, AudioSource>();
		for(int i =0; i< audioFileNames.Length; i++)
		{
			 AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			 audioSource.clip = Resources.Load(audioFileNames[i]) as AudioClip;
			 ASDict.Add(audioFileNames[i],audioSource);
		}
		Play("Fire");
		Play("Music Competitive");
		ASDict["Music Competitive"].loop = true;
	}
	

	public void Play(string pString)
	{
		if(ASDict.ContainsKey(pString) && ASDict[pString] != null) ASDict[pString].Play();
	}
	
}
