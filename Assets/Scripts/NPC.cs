using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour {

	public GameObject deadNPC;
	public Player player;

	public Vector2 velocity;
	public float baseSpeed = 0.0f;
	private float speed;

	public float alertRadius = 3f;
	public float attackRadius = 1f;
	public float suspicionIncreaseRate = 0.01f;
	public float chanceToChangeRooms = 0f;
	public float wanderChance = 0f;

	public Tilemap tilemap;

	public Tile currentTile;
	public List<Tile> currentPath;

	public int health = 100;

	public bool scared = false;
	public bool fighting = false;
	public bool fleeing = false;

	private int timeSinceLastSighting;
	public SuspicionBar suspicion_bar;

	private float attackCooldown = 1; // time between attacks in seconds
	private float currentCooldown = 0;

	Animator anim;


	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator> ();
		anim.SetBool ("NPC", true);
		anim.SetBool ("wolfForm", false);
		anim.SetBool ("up", false);
		anim.SetBool ("down", false);
		anim.SetBool ("left", false);
		anim.SetBool ("right", false);
		anim.SetBool ("Move", false);

		velocity = new Vector2 (0.0f, 0.0f);
		currentPath = new List<Tile>();
		speed = baseSpeed;
		suspicion_bar = this.GetComponent <SuspicionBar> ();

	}

	void Update(){

		// INcrease suspicion
		suspicion_bar.susp_level = Mathf.Min (1.0f, suspicion_bar.susp_level + suspicionIncreaseRate*Time.deltaTime);

		currentCooldown = Mathf.Min(currentCooldown+ Time.deltaTime, 10);

		AlertNPCS();

		WerewolfCheck();

		if (!scared){
			// If the currentPath is empty, there is a chance every frame to start to move to a random tile. (moveChance%)
			if (currentPath.Count == 0){
				Wander();
			}
		} else {

			if (fighting){
				GetNeigboursToFight();
				MoveToPlayer();
				AttackPlayer();

			} else {
				if (currentPath.Count == 0){
					currentPath = tilemap.GetRandomPath(currentTile);
				}

			}
		}
	}

	void UpdatePath(){

		foreach (Tile t in currentPath){
			if ((player.transform.position - t.transform.position).magnitude <= 2f){
				currentPath = tilemap.GetRandomPath(currentTile);
				break;
			}
		}
	}

	void WerewolfCheck(){

		if (player == null){
			return;
		}

		if (this.IsWerewolfVisible()){

			if (!scared){
				scared = true;
				FightOrFlight();
				if (fleeing){
					speed = 1.5f*baseSpeed;
				}
			} else if (timeSinceLastSighting > 0) {
				UpdatePath();
			}
			timeSinceLastSighting = 0;
			
		} else {
			
			timeSinceLastSighting++;
			if (timeSinceLastSighting > 500){
				scared = false;
				fighting = false;
				fleeing = false;
				speed = baseSpeed;
			}
			
		}

	}

	void FightOrFlight(){

		this.Stop();

		if (!player.transformed){
			scared = true;
			fighting = true;
		
		} else {

			float x = 1.5f;
			float chanceToFight = (x-1)/x;
			float outcome = Random.Range (0.0f, 1.0f);

			if (outcome < chanceToFight){
				fighting = true;
				GetNeigboursToFight();
			} else {
				fleeing = true;
			}
		}
	}

	void GetNeigboursToFight(){

		NPC[] npcs = GameObject.FindObjectsOfType(this.GetType()) as NPC[];

		for (int i = 0; i < npcs.Length; i++){
		
			NPC npc = npcs[i];

			if (!npc.fighting && (this.GetLocation() - npc.GetLocation()).magnitude <= 5){
				npc.fighting = true;
				npc.Stop ();
			}

		}

	}

	public void Stop(){
		velocity = new Vector2 (0.0f, 0.0f);
		this.rigidbody2D.Sleep ();
		currentPath = new List<Tile>();
	}

	void Wander(){

		float chanceToWander = Random.Range (0.0f, 100f);
		if (chanceToWander < wanderChance){
			currentPath.Add (currentTile.GetRandomNeighbour());
		} else {
			
			float chanceToMove = Random.Range(0.0f, 100.0f);
			if (chanceToMove <= chanceToChangeRooms){
				currentPath = tilemap.GetRandomPath(currentTile);
			}
		}

	}

	void MoveToPlayer(){

		if (player == null){
			return;
		}

		if (currentPath.Count == 0){
			currentPath = tilemap.GetPath (currentTile, tilemap.GetClosestTile(player.GetLocation()).GetRandomNeighbour());
		}

	}
	void AttackPlayer(){

		if (currentCooldown >= 0.2){
			anim.SetBool ("NPCAttack", false);
		}



		if (player == null || currentCooldown <= attackCooldown){
			return;
		}

		if ((player.GetLocation()-this.GetLocation ()).magnitude <= this.attackRadius){

			audio.Play ();
			currentCooldown = 0;
			player.TakeDamage(0.1f);
			anim.SetBool ("NPCAttack", true);

		}

	}

	// Update is called once per frame
	void FixedUpdate () {

		if (currentTile == null){
			currentTile = tilemap.GetTile ((int)this.transform.position.x,
			                               (int)this.transform.position.y);
		}

		this.velocity = new Vector2 (0.0f, 0.0f);



		float distanceToTravel = 0.0f;

		// Get the next point in the path.
		int nextIndex = this.currentPath.Count-1;
		if (nextIndex != -1){
			Vector2 currentPosition = new Vector2(this.transform.position.x,
		        	                              this.transform.position.y);
			Vector2 nextDestination = new Vector2(this.currentPath[nextIndex].transform.position.x,
		    	                                  this.currentPath[nextIndex].transform.position.y);
	
			// Direction and distance to the next point.
			Vector2 directionToDestination = nextDestination-currentPosition;
			distanceToTravel = directionToDestination.magnitude;

			// If the NPC is close enough, remove the point from the queue.
			if (directionToDestination.magnitude < 0.1f){
				this.currentTile = this.currentPath[nextIndex];
				this.currentPath.RemoveAt(nextIndex);
				this.velocity = new Vector2 (0.0f, 0.0f);

			} else {

				// Set the npc to move towards the destination and not overshoot it.
				this.velocity.x = speed*(directionToDestination.normalized.x);
				this.velocity.y = speed*(directionToDestination.normalized.y);

			}

			// Move the NPC.
			if (speed * Time.deltaTime > distanceToTravel) {
				this.rigidbody2D.velocity = (this.velocity.normalized * distanceToTravel /Time.deltaTime);
			} else {
				this.rigidbody2D.velocity = (this.velocity);
			}

			if(this.rigidbody2D.velocity.x >= 0.1 || this.rigidbody2D.velocity.x <= -0.1){
				anim.SetBool ("Move", true);
				if(this.rigidbody2D.velocity.x < 0){
					anim.SetBool ("left", true);
					anim.SetBool ("right", false);
					anim.SetBool ("up", false);
					anim.SetBool ("down", false);
				}
				else if(this.rigidbody2D.velocity.x > 0){
					anim.SetBool ("right", true);
					anim.SetBool ("left", false);
					anim.SetBool ("up", false);
					anim.SetBool ("down", false);
				}
			}
			else if(this.rigidbody2D.velocity.y >= 0.1 || this.rigidbody2D.velocity.y <= -0.1){
				anim.SetBool ("Move", true);
				if(this.rigidbody2D.velocity.y < 0){
					anim.SetBool ("up", false);
					anim.SetBool ("down", true);
					anim.SetBool ("right", false);
					anim.SetBool ("left", false);
				}
				else if(this.rigidbody2D.velocity.y > 0){
					anim.SetBool ("down", false);
					anim.SetBool ("up", true);
					anim.SetBool ("right", false);
					anim.SetBool ("left", false);
				}
			}
			else{
				anim.SetBool ("Move", false);
			}

		}
	}

	void AlertNPCS(){

		if (this.suspicion_bar.get_suspicion() >= 1f){

			NPC[] npcs = FindObjectsOfType(typeof(NPC)) as NPC[];
			foreach(NPC npc in npcs){

				if ((npc.transform.position - this.transform.position).magnitude < alertRadius){

					npc.suspicion_bar.set_suspicion(Mathf.Max (this.suspicion_bar.get_suspicion(), npc.suspicion_bar.get_suspicion()));
					this.suspicion_bar.set_suspicion(Mathf.Max (this.suspicion_bar.get_suspicion(), npc.suspicion_bar.get_suspicion()));

				}

			}
		}

	}

	bool IsWerewolfVisible(){

		if (player == null){
			return false;
		}
		
		RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y),
		                                     new Vector2(player.transform.position.x, player.transform.position.y)-
		                                     new Vector2(this.transform.position.x, this.transform.position.y));
		
		if ((new Vector2(player.transform.position.x, player.transform.position.y) - hit.point).magnitude <= 1){

			if (player.transformed || this.suspicion_bar.get_suspicion() >= 1f){
				return true;
			}
		}
		return false;
	}

	public void SetLocation(Vector3 loc){
		this.transform.position = loc;
	}

	public Vector2 GetLocation(){
		return new Vector2(this.transform.position.x, this.transform.position.y);
	}

	public void SetTilemap(Tilemap t){
		this.tilemap = t;
	}

	public void SetCurrentTile(Tile t){
		currentTile = t;
	}

	public void TakeDamage(int damage){
		this.health -= damage;

		if (this.health <= 0){
			this.Die();
		}

	}

	public void Die(){
		GameObject body = GameObject.Instantiate(deadNPC) as GameObject;
		body.transform.position = this.transform.position;

		Destroy(this.gameObject);
	}

}

