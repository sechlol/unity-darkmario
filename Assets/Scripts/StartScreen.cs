using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (UserInput.JumpDown ()) {
			Application.LoadLevel("Level1");		
		}

		if (UserInput.Exit ()) {
			Debug.Log ("Quitting");
			Application.Quit();		
		}
	
	}
}
