﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
	
	public float barDisplay; //current progress
	public Vector2 pos = new Vector2(20,500);
	public Vector2 size = new Vector2(120,40);
	public Texture2D deadTex;
	public Texture2D healthyTex;
	public GUIStyle progress_empty, progress_full;
	public float current_health=100.0f;

	//Increases the current health by value 
	public void increaseBy(float value){
		current_health += value;
	}

	//Decreases the current health by value
	public void decreaseBy(float value){
		current_health -= value;
	}
	
	void OnGUI() {
		//draw the background:
		GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
		GUI.Box (new Rect (0, 0, size.x, size.y), deadTex);
		
		//draw the filled-in part:
		GUI.BeginGroup (new Rect (0, 0, size.x * barDisplay, size.y));
		GUI.Box (new Rect (0, 0, size.x, size.y), healthyTex);
		GUI.EndGroup ();
		GUI.EndGroup ();
	}
	
	// Use this for initialization
	void Start ()
	{
		barDisplay = current_health;
	}
	
	// Update is called once per frame
	void Update ()
	{
		barDisplay = current_health;
		if (current_health <= 0) {
			Application.LoadLevel("won_level"); //Todo add losing level
		}
		
	}
}
