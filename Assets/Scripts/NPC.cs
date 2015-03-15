using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour {

	public GameObject deadNPC;
	public Player player;

	public Vector2 velocity;
	public float baseSpeed = 0.0f;
	private float speed;

	public float attackRadius = 2f;
	public float chanceToChangeRooms = 0f;
	public float wanderChance = 0f;

	public Tilemap tilemap;

	public Tile currentTile;
	public List<Tile> currentPath;

	public int health = 100;

	private bool scared = false;
	private bool fighting = false;
	private bool fleeing = false;

	private int timeSinceLastSighting;
	public SuspicionBar suspicion_bar;

	private float attackCooldown = 1; // time between attacks in seconds
	private float currentCooldown = 0;

	// Use this for initialization
	void Start () {

		velocity = new Vector2 (0.0f, 0.0f);
		currentPath = new List<Tile>();
		speed = baseSpeed;
		suspicion_bar = new SuspicionBar ();

	}

	void Update(){

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
		Debug.Log ("Oh shit!");
		if (!player.transformed){
			scared = true;
			fighting = true;
			Debug.Log ("It's on Now.");
		
		} else {

			float x = 1.5f;
			float chanceToFight = (x-1)/x;
			float outcome = Random.Range (0.0f, 1.0f);

			if (outcome < chanceToFight){
				fighting = true;
			} else {
				fleeing = true;
			}
		}
	}

	void Stop(){
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

		if (player == null || currentCooldown <= attackCooldown){
			return;
		}

		if ((player.GetLocation()-this.GetLocation ()).magnitude <= this.attackRadius){

			Debug.Log ("Take that!");
			currentCooldown = 0;
			player.TakeDamage(10);

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
		}
	}

	void AlertNPCS(){

		if (this.suspicion_bar.get_suspicion() >= 0.9f){

			NPC[] npcs = FindObjectsOfType(typeof(NPC)) as NPC[];
			foreach(NPC npc in npcs){

				if ((npc.transform.position - this.transform.position).magnitude < 6){

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

			if (player.transformed || this.suspicion_bar.get_suspicion() >= 0.9f){
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

