using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	public float playerSpeed = 5.0f;

	public Vector2 velocity;

	public Hunger hunger_bar;

	Animator anim;

	public bool isTransformed = false;
	private bool transformation_On_CD = false;
	private int transform_Cooldown = 0;
	private int TRANSFORM_CD_TIME = 100;

	void Start(){

		anim = GetComponent<Animator> ();
		anim.SetBool ("wolfForm", false);
		//anim = GetComponent<Animator> ();
		//anim.SetBool ("up", false);
		///anim.SetBool ("down", false);
		//anim.SetBool ("left", false);
		//anim.SetBool ("right", false);
	}
	
	void FixedUpdate(){

		//Transformation Cooldown Code
		if(transformation_On_CD){
			transform_Cooldown++;
			if(transform_Cooldown == TRANSFORM_CD_TIME){
				transform_Cooldown = 0;
				transformation_On_CD = false;
			}
		}
		//Transformation Code		
		if (Input.GetKey (KeyCode.Space) && !transformation_On_CD) {
			if(anim.GetBool("wolfForm") == false){
				anim.SetBool ("wolfForm", true);
				isTransformed = true;
				transformation_On_CD = true;
			}
			else{
				anim.SetBool ("wolfForm", false);
				isTransformed = false;
				transformation_On_CD = true;
			}
		}

		//bool move = false; 
		velocity = new Vector2(0.0f, 0.0f);
		if(Input.GetKey(KeyCode.D)){
			//move = true;
		//	anim.SetBool ("up", false);
		//	anim.SetBool ("down", false);
		//	anim.SetBool ("left", false);
		//	anim.SetBool("right", true);
			velocity += new Vector2(playerSpeed, 0.0f);
		}

		if(Input.GetKey(KeyCode.A)){
			//move = true;
		//	anim.SetBool ("up", false);
		//	anim.SetBool ("down", false);
		//	anim.SetBool ("right", false);
		//	anim.SetBool ("left", true);
			velocity -= new Vector2(playerSpeed, 0.0f);
		}
		if(Input.GetKey(KeyCode.W)){
			//move = true;
		//	anim.SetBool ("down", false);
		//	anim.SetBool ("left", false);
		//	anim.SetBool ("right", false);
		//	anim.SetBool ("up", true);
			velocity += new Vector2( 0.0f, playerSpeed);
		}
		if(Input.GetKey(KeyCode.S)){
			//move = true;
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
	
	
	void OnCollisionEnter2D(Collision2D coll){

		if (coll.gameObject.name == "NPC(Clone)" && isTransformed){
			NPC npc = coll.gameObject.GetComponent<NPC>();
			npc.TakeDamage (100);
			hunger_bar.increaseBy(0.05f);
		}


	}
}
