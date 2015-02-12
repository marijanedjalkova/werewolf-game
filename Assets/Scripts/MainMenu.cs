using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void OnGUI(){
		const int buttonHeight = 50;
		const int buttonWidth = 120;
		//Finds coords to place button relative to screen size
		Rect startButtonRect = new Rect (
			Screen.width / 2 - (buttonWidth / 2),
			Screen.height / 2 - (buttonHeight / 2),
			buttonWidth, buttonHeight);

		Rect quitButtonRect = new Rect (
			Screen.width / 2 - (buttonWidth / 2),
			2*Screen.height / 3 - (buttonHeight / 2),
			buttonWidth, buttonHeight);

		//Creates button
		if (GUI.Button (startButtonRect, "Start new Game")) {
			//Loads first level, left at prototype for testing.
			Application.LoadLevel("prototype_level");
		}
		if (GUI.Button (quitButtonRect, "Quit Game")) {
			//Quits the game
			Application.Quit (); 
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
