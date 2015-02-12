using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour {

	public float speed = 3.0f;

	public int health = 100;

	public int suspicion = 0;

	public Tilemap tilemap;

	public Vector2 velocity;

	public float idleChance = 99.0f;

	public Tile currentTile = null;

	public List<Tile> currentPath;

	// Use this for initialization
	void Start () {

		velocity = new Vector2 (0.0f, 0.0f);
		currentPath = new List<Tile>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		float distanceToTravel = 0.0f;

		if (currentTile == null){
			currentTile = tilemap.GetTile ((int)this.transform.position.x,
			                               (int)this.transform.position.y);
		}

		this.velocity = new Vector2 (0.0f, 0.0f);
		if (currentPath == null){
			Debug.Log (currentTile.x);
			Debug.Log (currentTile.y);
		}

		// If the currentPath is empty, there is a chance every frame to start to move to a random tile. (moveChance%)
		// Currently hardcoded to move on the map I made, but could be generalized to allow the NPC
		// to walk around a room idly. -- Kyle
		if (currentPath.Count == 0){
			float chanceToMove = Random.Range(0.0f, 100.0f);
			if (chanceToMove >= idleChance){
					currentPath = tilemap.GetRandomPath(currentTile,
				                      					tilemap.GetTile (0,0),
				                      					tilemap.GetTile(tilemap.sizeX-1, tilemap.sizeY-1));
			}
		}

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
			if (directionToDestination.magnitude < 0.001f){
				this.currentTile = this.currentPath[nextIndex];
				this.currentPath.RemoveAt(nextIndex);
				this.velocity = new Vector2 (0.0f, 0.0f);

			} else {

				// Set the npc to move towards the destination and not overshoot it.
				this.velocity.x = speed*(directionToDestination.normalized.x);
				this.velocity.y = speed*(directionToDestination.normalized.y);

			}
		}

		// Move the NPC.
		if (speed * Time.deltaTime > distanceToTravel) {
			this.rigidbody2D.velocity = (this.velocity.normalized * distanceToTravel /Time.deltaTime);
		} else {
			this.rigidbody2D.velocity = (this.velocity);
		}

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

	// Method for testing if npcs are damaged.
	void OnMouseDown(){
		this.TakeDamage (100);
	}

	public void TakeDamage(int damage){
		this.health -= damage;

		if (this.health <= 0){
			Destroy(gameObject);

		}

	}


}

