using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour {

	public GameObject deadNPC;
	public Player player;

	public Vector2 velocity;
	public float baseSpeed = 0.0f;
	private float speed;

	public float idleChance = 99.0f;

	public Tilemap tilemap;

	public Tile currentTile;
	public List<Tile> currentPath;

	public int health = 100;
	public int suspicion = 0;

	private bool scared = false;
	private bool fighting = false;
	private bool fleeing = false;

	private int timeSinceLastSighting;


	// Use this for initialization
	void Start () {

		velocity = new Vector2 (0.0f, 0.0f);
		currentPath = new List<Tile>();
		speed = baseSpeed;

	}

	void Update(){

		if (this.IsWerewolfVisible()){

			if (!scared){
				scared = true;
				this.FightOrFlight();
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

		if (!scared){
			// If the currentPath is empty, there is a chance every frame to start to move to a random tile. (moveChance%)
			if (currentPath.Count == 0){
				
				float chanceToMove = Random.Range(0.0f, 100.0f);
				if (chanceToMove >= idleChance){
					currentPath = tilemap.GetRandomPath(currentTile);
				}
			}
		} else {

			if (fighting){


			} else {
				if (currentPath.Count == 0){
					currentPath = tilemap.GetRandomPathFleeing(currentTile);
				}

			}

		}
	}

	void UpdatePath(){

		foreach (Tile t in currentPath){
			if ((player.transform.position - t.transform.position).magnitude <= 2f){
				Debug.Log (this.transform.position);
				currentPath = tilemap.GetRandomPathFleeing(currentTile);
				break;
			}
		}
	}

	void FightOrFlight(){

		this.Stop ();

		int x = 1;
		float chanceToFight = (x-1)/x;

		float outcome = Random.Range (0.0f, 1.0f);

		if (outcome < chanceToFight){
			fighting = true;
		} else {
			fleeing = true;
		}
	}

	void Stop(){
		velocity = new Vector2 (0.0f, 0.0f);
		this.rigidbody2D.velocity = (this.velocity);
		currentPath = new List<Tile>();
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

	bool IsWerewolfVisible(){

		if (!player.transformed)
			return false;

		RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y),
		                                     new Vector2(player.transform.position.x, player.transform.position.y)-
		                                     new Vector2(this.transform.position.x, this.transform.position.y));
		
		if ((new Vector2(player.transform.position.x, player.transform.position.y) - hit.point).magnitude <= 1){
			return true;
		}
		return false;
	}

	public void SetLocation(Vector3 loc){
		this.transform.position = loc;
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

