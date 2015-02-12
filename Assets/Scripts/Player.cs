using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	public float playerSpeed = 5.0f;

	public Vector2 velocity;

	public Hunger hunger_bar;

	//Animator anim;
	void Start(){


		//anim = GetComponent<Animator> ();
		//anim.SetBool ("up", false);
		///anim.SetBool ("down", false);
		//anim.SetBool ("left", false);
		//anim.SetBool ("right", false);
	}
	
	void FixedUpdate(){
		bool move = false; 

		velocity = new Vector2(0.0f, 0.0f);

		if(Input.GetKey(KeyCode.D)){
			move = true;
		//	anim.SetBool ("up", false);
		//	anim.SetBool ("down", false);
		//	anim.SetBool ("left", false);
		//	anim.SetBool("right", true);
			velocity += new Vector2(playerSpeed, 0.0f);
		}

		if(Input.GetKey(KeyCode.A)){
			move = true;
		//	anim.SetBool ("up", false);
		//	anim.SetBool ("down", false);
		//	anim.SetBool ("right", false);
		//	anim.SetBool ("left", true);
			velocity -= new Vector2(playerSpeed, 0.0f);
		}
		if(Input.GetKey(KeyCode.W)){
			move = true;
		//	anim.SetBool ("down", false);
		//	anim.SetBool ("left", false);
		//	anim.SetBool ("right", false);
		//	anim.SetBool ("up", true);
			velocity += new Vector2( 0.0f, playerSpeed);
		}
		if(Input.GetKey(KeyCode.S)){
			move = true;
		//	anim.SetBool ("up", false);
		//	anim.SetBool ("left", false);
		//	anim.SetBool ("right", false);
		//	anim.SetBool ("down", true);
			velocity -= new Vector2(0.0f, playerSpeed);
		}

		//anim.SetBool("moving", move);
		if (velocity.magnitude > 0){
			rigidbody2D.velocity = this.velocity;
		} else {
			rigidbody2D.Sleep ();
		}
	}	
	
	
	void OnTriggerEnter2D(Collider2D coll){

		if (coll.name == "NPC(Clone)"){
			NPC npc = coll.GetComponent<NPC>();
			npc.TakeDamage (100);
			hunger_bar.increaseBy(0.05f);
		}


	}
}
