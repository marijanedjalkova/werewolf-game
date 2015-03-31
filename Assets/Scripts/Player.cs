using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	public float playerSpeed = 5.0f;

	public Vector2 velocity;

	public Hunger hunger_bar;

	public Health health_bar;

	public float health = 100f;

	public Animator anim;

	public bool transformed = false;
	public AudioClip transformSound;
	public AudioClip attackSound;

	private bool transformation_On_CD = false;
	private const float TRANSFORM_CD_TIME = 10f;
	private float transform_Cooldown = 0f;

	void Start(){
		anim = GetComponent<Animator> ();
		anim.SetBool ("wolfForm", false);
		anim.SetBool ("up", false);
		anim.SetBool ("down", false);
		anim.SetBool ("left", false);
		anim.SetBool ("right", false);
		anim.SetBool ("Move", false);
	}
	
	void FixedUpdate(){

		//Transformation Cooldown Code
		if(transformation_On_CD){
			transform_Cooldown += Time.deltaTime;
			if(transform_Cooldown >= TRANSFORM_CD_TIME){
				transform_Cooldown = 0;
				transformation_On_CD = false;
			}
		}
		//Transformation Code		
		if (Input.GetKey (KeyCode.Space) && !transformation_On_CD) {
			if(anim.GetBool("wolfForm") == false){
				audio.clip = transformSound;
				audio.Play ();
				anim.SetBool ("wolfForm", true);
				transformed = true;
				transformation_On_CD = true;
				TransformNPCCheck();
			}
			else{
				anim.SetBool ("wolfForm", false);
				transformed = false;
				transformation_On_CD = true;
				TransformNPCCheck();
			}
		}

			bool move = false; 
			velocity = new Vector2(0.0f, 0.0f);
		if(Input.GetKey(KeyCode.D) || Input.GetKey (KeyCode.RightArrow)){
			move = true;
			anim.SetBool ("up", false);
			anim.SetBool ("down", false);
			anim.SetBool ("left", false);
			anim.SetBool("right", true);
			velocity += new Vector2(playerSpeed, 0.0f);
		}

		if(Input.GetKey(KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)){
			move = true;
			anim.SetBool ("up", false);
			anim.SetBool ("down", false);
			anim.SetBool ("right", false);
			anim.SetBool ("left", true);
			velocity -= new Vector2(playerSpeed, 0.0f);
		}
		if(Input.GetKey(KeyCode.W) || Input.GetKey (KeyCode.UpArrow)){
			move = true;
			anim.SetBool ("down", false);
			anim.SetBool ("left", false);
			anim.SetBool ("right", false);
			anim.SetBool ("up", true);
			velocity += new Vector2( 0.0f, playerSpeed);
		}
		if(Input.GetKey(KeyCode.S) || Input.GetKey (KeyCode.DownArrow)){
			move = true;
			anim.SetBool ("up", false);
			anim.SetBool ("left", false);
			anim.SetBool ("right", false);
			anim.SetBool ("down", true);
			velocity -= new Vector2(0.0f, playerSpeed);
		}

		anim.SetBool("Move", move);

		if (velocity.magnitude > 0){
			rigidbody2D.velocity = this.velocity;
		} else {
			rigidbody2D.Sleep ();
		}


		//Finds distance to silver
		var silver = GameObject.Find ("Silver");
		if (silver != null){
			SilverDamage (silver);
		}
	}	

	void Update(){

		if (Input.GetKeyDown(KeyCode.Q) && transformed){

			audio.clip = attackSound;
			audio.Play ();

		}

	}

	public Vector2 GetLocation(){
		return new Vector2(this.transform.position.x, this.transform.position.y);
	}

	void TransformNPCCheck(){

		NPC[] npcs = FindObjectsOfType(typeof(NPC)) as NPC[];
		foreach(NPC npc in npcs){

			RaycastHit2D hit = Physics2D.Raycast(new Vector2(npc.transform.position.x, npc.transform.position.y),
			                                     new Vector2(this.transform.position.x, this.transform.position.y)-
			                                     new Vector2(npc.transform.position.x, npc.transform.position.y));
			
			if ((new Vector2(this.transform.position.x, this.transform.position.y) - hit.point).magnitude <= 1){
				npc.suspicion_bar.set_suspicion(1.0f);

			}

		}
	}

	public void kill(Collider2D npc){

		GameObject passObject = GameObject.Find ("passingObject");
		menuHelper helpScript = passObject.GetComponent<menuHelper> ();

		int numNPC = helpScript.numNPC;
		NPC to_kill = npc.gameObject.GetComponent<NPC> ();
		to_kill.TakeDamage (100);
		hunger_bar.increaseBy (1f / numNPC);

	}


	public void TakeDamage(float damageAmount){
		health_bar.decreaseBy (damageAmount);
		this.health -= damageAmount;
		if (this.health <= 0f){
			health = 0f;
		}
	}

	//Determines if the player is close enough to specified silver to deal damage, and carries it out
	void SilverDamage(GameObject silver){
		float distance = Vector3.Distance (this.transform.position, silver.transform.position);
		if (distance < 1) {
			health_bar.decreaseBy (1.0f);
		} else if (distance < 2) {
			health_bar.decreaseBy (0.5f);
		} else if (distance < 3) {
			health_bar.decreaseBy (0.25f);
		} else if (distance < 5) {
			health_bar.decreaseBy (0.1f);
		}
	}

	void Die(){

		//GameObject body = GameObject.Instantiate(deadNPC) as GameObject;
		//body.transform.position = this.transform.position;

		this.GetComponentInChildren<Camera>().transform.parent = null;
		Destroy(this.gameObject);
	}
}
