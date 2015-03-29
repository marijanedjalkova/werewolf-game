using UnityEngine;
using System.Collections;

public class BasicMenu : MonoBehaviour {

	void OnGUI(){
		const int buttonHeight = 50;
		const int buttonWidth = 140;
		//Finds coords to place button relative to screen size
		Rect startButtonRect = new Rect (
			Screen.width / 2 - (buttonWidth / 2),
			2 * Screen.height / 3 - (buttonHeight / 2),
			buttonWidth, buttonHeight);

		Rect quitButtonRect = new Rect (
			Screen.width / 2 - (buttonWidth / 2),
			2 * Screen.height / 3 - (buttonHeight / 2)+ buttonHeight * 2,
			buttonWidth, buttonHeight);

		//Creates button
		if (GUI.Button (startButtonRect, "Back to Main Menu")) {
			//Loads first level, left at prototype for testing.
			Application.LoadLevel ("MainMenu");
		}
		if (GUI.Button (quitButtonRect, "Quit Game")) {
			//Quits th game
			Application.Quit (); //Wrong method, not sure what right one is 
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
