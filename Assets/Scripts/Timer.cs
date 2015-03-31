using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	float timeRemaining = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeRemaining -= Time.deltaTime;
	}

	void OnGUI(){
		if (timeRemaining > 0) {
			GUIStyle myLabelStyle = new GUIStyle(GUI.skin.label);
			myLabelStyle.fontSize = 24;
			GUI.Label(new Rect (10, 80, 200, 100), "Time left: "+ (int)timeRemaining, myLabelStyle);
		} else {
			Application.LoadLevel("lost_level");
		}
	}
}
