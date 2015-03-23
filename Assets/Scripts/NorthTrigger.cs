﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NorthTrigger : MonoBehaviour {
	Player player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
		player.ntr = this;
		float x = player.GetLocation ().x;
		float y = player.GetLocation ().y + 1.0f;
		transform.position = new Vector2 (x, y);
	}
	
	// Update is called once per frame
	void Update () {
		float x = player.GetLocation ().x;
		float y = player.GetLocation ().y + 1.0f;
		transform.position = new Vector2 (x, y);
	}

	void OnTriggerEnter2D(Collider2D other){
		if(Input.GetKey(KeyCode.Q)&& player.anim.GetBool("up")){
			if (other.gameObject.name == "NPC(Clone)" && player.transformed){
				NPC npc = other.gameObject.GetComponent<NPC>();
				npc.TakeDamage (100);
				player.hunger_bar.increaseBy(0.05f);
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if(Input.GetKey(KeyCode.Q)&& player.anim.GetBool("up")){
			Debug.Log("KILLING NORTH");
			if (other.gameObject.name == "NPC(Clone)" && player.transformed){
				player.kill(other);
			}
		}
	}


}