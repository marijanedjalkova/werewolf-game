using UnityEngine;
using System.Collections.Generic;

public class NPCMovement : MonoBehaviour {

	public TilemapData tilemap;
	
	public List<TileData> currentPath;

	public float speed = 1;

	public Vector3 velocity;

	public float moveChance = 99;

	public TileData currentTile = null;
	
	// Use this for initialization
	void Start () {
	
		velocity = new Vector3 (0, 0, 0);

	}
	
	// Update is called once per frame
	void Update () {
	
		float distanceToTravel = 0.0f;

		if (currentTile == null){
			currentTile = tilemap.GetTile ((int)this.transform.position.x,
			                               (int)this.transform.position.y);
		}

		this.velocity = new Vector3 (0, 0, 0);

		// If the currentPath is empty, there is a chance every frame to start to move to a random tile. (moveChance%)
		// Currently hardcoded to move on the map I made, but could be generalized to allow the NPC
		// to walk around a room idly. -- Kyle
		if (currentPath.Count == 0){
			float chanceToMove = Random.Range(0, 100);
			if (chanceToMove >= moveChance){
					currentPath = tilemap.GetRandomPath(currentTile,
				                      					tilemap.GetTile (0,0),
				                      					tilemap.GetTile(tilemap.sizeX-1, tilemap.sizeY-1));
			}
		}

		// Get the next point in the path.
		int nextIndex = this.currentPath.Count-1;
		if (nextIndex != -1){
			Vector3 currentPosition = new Vector3(this.transform.position.x,
			                                      this.transform.position.y,
			                                      0);
			Vector3 nextDestination = new Vector3(this.currentPath[nextIndex].transform.position.x,
			                                      this.currentPath[nextIndex].transform.position.y,
			                                      0);

			// Direction and distance to the next point.
			Vector3 directionToDestination = nextDestination-currentPosition;
			distanceToTravel = directionToDestination.magnitude;

			// If the NPC is close enough, remove the point from the queue.
			if (directionToDestination.magnitude < 0.1f){
				this.currentTile = this.currentPath[nextIndex];
				this.currentPath.RemoveAt(nextIndex);
				this.velocity = new Vector3 (0, 0, 0);

			} else {

				// Set the npc to move towards the destination and not overshoot it.
				this.velocity.x = speed*(directionToDestination.normalized.x);
				this.velocity.y = speed*(directionToDestination.normalized.y);

			}
		}

		// Move the NPC.
		if (speed * Time.deltaTime > distanceToTravel) {
			this.transform.Translate (this.velocity.normalized * distanceToTravel);
		} else {
			this.transform.Translate (this.velocity * Time.deltaTime);
		}

	}




}

