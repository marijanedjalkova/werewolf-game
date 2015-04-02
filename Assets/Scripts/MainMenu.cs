using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public static int numRooms;
	public static int numNPC;

	string numRoomsString = "5";
	string numNPCString = "15";

	void OnGUI(){
		var passObject = GameObject.Find ("passingObject");
		var helpScript = passObject.GetComponent<menuHelper>();

		const int buttonHeight = 25;
		const int buttonWidth = 120;
		const int inputHeight = 20;
		const int inputWidth = 30;

		//Text input boxes for level creator
		numRoomsString = GUI.TextField(new Rect (
			Screen.width / 2 + (inputWidth*2/3)+50,
			Screen.height / 2 + (buttonHeight*3),
			inputWidth,inputHeight),
		    numRoomsString,25); 
		numNPCString = GUI.TextField (new Rect (
			Screen.width / 2 + (inputWidth*2/3)+50,
			Screen.height / 2 + (buttonHeight*2),
			inputWidth,inputHeight),
		    numNPCString, 25);
		//Text input box labels
		GUI.Label (new Rect (
			Screen.width / 2 - (inputWidth*3/2) ,
			Screen.height / 2 + (buttonHeight*3),
			300,inputHeight),
			"Number of Rooms:");
		GUI.Label (new Rect (
			Screen.width / 2 - (inputWidth*3/2) ,
			Screen.height / 2 + (buttonHeight*2),
			300,inputHeight),
		    "Number of NPCs:");

		//Finds coords to place button relative to screen size
		Rect startButtonRect = new Rect (
			Screen.width / 2 - (buttonWidth / 2),
			Screen.height / 2 - (buttonHeight / 2),
			buttonWidth, buttonHeight);

		Rect quitButtonRect = new Rect (
			Screen.width / 2 - (buttonWidth / 2),
			2*Screen.height / 3 - (buttonHeight / 2),
			buttonWidth, buttonHeight);

		Rect easyButtonRect = new Rect (Screen.width/3-buttonWidth/2,Screen.height / 2,buttonWidth,buttonHeight);
		Rect medButtonRect = new Rect (Screen.width/3-buttonWidth/2,Screen.height / 2 + buttonHeight,buttonWidth,buttonHeight);
		Rect hardButtonRect = new Rect (Screen.width/3-buttonWidth/2,Screen.height / 2 +buttonHeight *2,buttonWidth,buttonHeight);

		//Creates buttons
		if (GUI.Button (easyButtonRect, "Difficulty: Easy")) {
			numRoomsString = "5";
			numNPCString = "10";
		}
		if (GUI.Button (medButtonRect, "Difficulty: Medium")) {
			numRoomsString = "5";
			numNPCString = "15";
		}
		if (GUI.Button (hardButtonRect, "Difficulty: Hard")) {
			numRoomsString = "10";
			numNPCString = "25";
		}
		if (GUI.Button (startButtonRect, "Start new Game")) {
			helpScript.numRooms = int.Parse(numRoomsString);
			helpScript.numNPC = int.Parse(numNPCString);
			//Loads first level, left at prototype for testing.
			Application.LoadLevel("prototype_level");
		}
		if (GUI.Button (quitButtonRect, "Quit Game")) {
			//Quits the game
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
