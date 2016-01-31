using UnityEngine;
using System.Collections;

public class GameOverScreenController : MonoBehaviour {

	public static string gameOverMessage = "";

	[SerializeField]
	UnityEngine.UI.Text gameOverLabel;

	// Use this for initialization
	void Start () {
		if(gameOverLabel!=null) gameOverLabel.text = gameOverMessage;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
