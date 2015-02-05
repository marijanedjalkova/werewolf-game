using UnityEngine;
using System.Collections.Generic;

public class NPCMovement : MonoBehaviour {

	public TilemapData tilemap;
	
	public List<TileData> currentPath;

	public float speed = 1;

	public float velocityX = 0;
	public float velocityY = 0;

	public float moveChance = 99;

	public TileData currentTile = null;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (currentTile == null){
			currentTile = tilemap.GetTile ((int)this.transform.position.x,
			                               (int)this.transform.position.y);
		}

		this.velocityX = 0;
		this.velocityY = 0;

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

			// If the NPC is close enough, remove the point from the queue.
			if (directionToDestination.magnitude < 0.1f){
				this.currentTile = this.currentPath[nextIndex];
				this.currentPath.RemoveAt(nextIndex);
				this.velocityX = 0;
				this.velocityY = 0;

			} else {

				// Set the npc to move towards the destination and not overshoot it.
				this.velocityX = speed*(directionToDestination.normalized.x);
				this.velocityY = speed*(directionToDestination.normalized.y);
			}
		}

		// Move the NPC.
		this.transform.Translate(new Vector3(this.velocityX, this.velocityY, 0)*Time.deltaTime);

	}




}

