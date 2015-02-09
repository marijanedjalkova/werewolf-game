using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float playerSpeed = 5.0f;
	
	//Animator anim;
	
	void Start(){
		//anim = GetComponent<Animator> ();
		//anim.SetBool ("up", false);
		///anim.SetBool ("down", false);
		//anim.SetBool ("left", false);
		//anim.SetBool ("right", false);
	}
	
	void Update(){
		bool move = false; 
		
		if(Input.GetKey(KeyCode.D)){
			move = true;
		//	anim.SetBool ("up", false);
		//	anim.SetBool ("down", false);
		//	anim.SetBool ("left", false);
		//	anim.SetBool("right", true);
			transform.position += new Vector3(playerSpeed * Time.deltaTime, 0.0f, 0.0f);
		}
		if(Input.GetKey(KeyCode.A)){
			move = true;
		//	anim.SetBool ("up", false);
		//	anim.SetBool ("down", false);
		//	anim.SetBool ("right", false);
		//	anim.SetBool ("left", true);
			transform.position -= new Vector3(playerSpeed * Time.deltaTime, 0.0f, 0.0f);
		}
		if(Input.GetKey(KeyCode.W)){
			move = true;
		//	anim.SetBool ("down", false);
		//	anim.SetBool ("left", false);
		//	anim.SetBool ("right", false);
		//	anim.SetBool ("up", true);
			transform.position += new Vector3( 0.0f, playerSpeed * Time.deltaTime, 0.0f);
		}
		if(Input.GetKey(KeyCode.S)){
			move = true;
		//	anim.SetBool ("up", false);
		//	anim.SetBool ("left", false);
		//	anim.SetBool ("right", false);
		//	anim.SetBool ("down", true);
			transform.position -= new Vector3( 0.0f, playerSpeed * Time.deltaTime, 0.0f);
		}
		//anim.SetBool("moving", move);
	}	
	
	
	void OnTriggerEnter2D(Collider2D coll){
		if(coll.name == "NPC1"){
			Destroy(GameObject.Find("NPC1"));
		}
		else if(coll.name == "NPC2"){
			Destroy(GameObject.Find("NPC2"));
		}
		else if(coll.name == "NPC3"){
			Destroy(GameObject.Find("NPC3"));
		}
		else if(coll.name == "NPC4"){
			Destroy(GameObject.Find("NPC4"));
		}
		else if(coll.name == "NPC5"){
			Destroy(GameObject.Find("NPC5"));
		}

	}
}
