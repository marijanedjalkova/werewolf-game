using UnityEngine;
using System.Collections.Generic;

public class NPCMovement : MonoBehaviour {

	public GameObject TileMap;
	
	public List<TileData> currentPath;

	public float speed = 1;

	public float velocityX = 0;
	public float velocityY = 0;

	public TileData currentLocation = null;

	// Use this for initialization
	void Start () {
	
		currentPath = new List<TileData>();

	}
	
	// Update is called once per frame
	void Update () {
	
		this.velocityX = 0;
		this.velocityY = 0;

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
				this.currentLocation = this.currentPath[nextIndex];
				this.currentPath.RemoveAt(nextIndex);
				this.velocityX = 0;
				this.velocityY = 0;

			} else {

				// Set the npc to move towards the destination and not overshoot it.
				this.velocityX = Mathf.Min(speed*Time.deltaTime*(directionToDestination.normalized.x),
				                           Time.deltaTime*directionToDestination.x);
				this.velocityY = Mathf.Min(speed*Time.deltaTime*(directionToDestination.normalized.y),
				                           Time.deltaTime*directionToDestination.y);
			}
		}

		// Move the NPC.
		this.transform.Translate(new Vector3(this.velocityX, this.velocityY, 0));

	}




}

