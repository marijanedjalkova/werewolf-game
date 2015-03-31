using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AttackTrigger : MonoBehaviour {
	Player player;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
		this.transform.parent = player.transform;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(Input.GetKeyDown(KeyCode.Q)&&
		   ((player.anim.GetBool("up") && this.name == "North Trigger") ||
		 	(player.anim.GetBool("down") && this.name == "South Trigger") ||
		 	(player.anim.GetBool("left") && this.name == "West Trigger") ||
		 	(player.anim.GetBool("right") && this.name == "East Trigger")
		 	)){
			if (other.gameObject.name == "NPC(Clone)" && player.transformed){
				player.kill(other);
			}
		}
	}
	
	void OnTriggerStay2D(Collider2D other){
		if(Input.GetKeyDown(KeyCode.Q)&&
		   ((player.anim.GetBool("up") && this.name == "North Trigger") ||
		 	(player.anim.GetBool("down") && this.name == "South Trigger") ||
		 	(player.anim.GetBool("left") && this.name == "West Trigger") ||
		 	(player.anim.GetBool("right") && this.name == "East Trigger")
		 	)){

			if (other.gameObject.name == "NPC(Clone)" && player.transformed){
				player.kill(other);
			}
		}
	}
	
	
}
