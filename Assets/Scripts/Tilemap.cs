using UnityEngine;
using System.Collections.Generic;


public struct Id {
	int x;
	int y;

	public Id(int x, int y){
		this.x = x;
		this.y = y;
	}
	public Vector2 v(){
		return new Vector2(x,y);
	}
}


public class Tilemap : MonoBehaviour {

	public GameObject room;
	public GameObject tileObject;

	public Dictionary<Id, Tile> tiles;

	private List<Room> rooms;
	private List<Room> halls;

	private int originX = int.MaxValue;
	private int originY = int.MaxValue;
	
	private int cornerX = int.MinValue;
	private int cornerY = int.MinValue;

	// Use this for initialization
	void Start () {
	
	}

	public void GenerateTilemap(int numberOfRooms){

		rooms = new List<Room>();
		halls = new List<Room>();

		tiles = new Dictionary<Id, Tile>();

		this.BuildRooms (numberOfRooms);

		foreach (Room r in rooms){
			foreach (Tile t in r.tiles){
				tiles[new Id(t.x, t.y)] = t;
				t.hallway = false;
			}
		}
		
		this.ConnectRooms ();
		foreach (Room h in halls){
			foreach (Tile t in h.tiles){
				if (t != null){
					t.hallway = true;
					if (tiles.ContainsKey (new Id(t.x, t.y))){
						if (tiles[new Id(t.x, t.y)].pathable == false){
							Destroy (tiles[new Id(t.x, t.y)].gameObject);
							tiles[new Id(t.x, t.y)] = t;
						} else {
							Destroy (t.gameObject);
						}
					} else {
						tiles[new Id(t.x, t.y)] = t;
					}
				}
			}
		}

		foreach(Room r in rooms){
			originX = Mathf.Min (originX, r.originX);
			originY = Mathf.Min (originY, r.originY);
			cornerX = Mathf.Max (cornerX, r.originX + r.sizeX);
			cornerY = Mathf.Max (cornerY, r.originY + r.sizeY);
		}

		foreach(Room h in halls){
			originX = Mathf.Min (originX, h.originX);
			originY = Mathf.Min (originY, h.originY);
			cornerX = Mathf.Max (cornerX, h.originX + h.sizeX);
			cornerY = Mathf.Max (cornerY, h.originY + h.sizeY);
		}

		this.RaiseWalls();
		this.SetNeighbours();

	}

	private void BuildRooms(int numberOfRooms){

		if (numberOfRooms == 0){
			return;
		}
		// Create the starting room.
		GameObject temp = GameObject.Instantiate (room) as GameObject;
		temp.transform.parent = this.transform;
		rooms.Add (temp.GetComponent<Room>());
		rooms[0].CreateRoom(0, 0, 10, 10);
		
		// Randomize the remaining rooms
		for (int i = 1; i < numberOfRooms; i++){
			temp = GameObject.Instantiate (room) as GameObject;
			temp.transform.parent = this.transform;
			rooms.Add (temp.GetComponent<Room>());
			rooms[i].CreateRoom(Random.Range (-1, 1),
			                    Random.Range (-1, 1),
			                    Random.Range (6, 15),
			                    Random.Range (6, 15));
		}
		
		bool collisions = true;
		int numberOfIterations = 0;
		while (collisions && numberOfIterations < 100){
			collisions = false;
			numberOfIterations++;
			foreach(Room room1 in rooms){
				if (room1 != rooms[0]){
					foreach(Room room2 in rooms){
						if (room1 != room2){
							if (room1.Overlaps(room2)){
								collisions = true;
								room1.MoveAway (room2);
								room2.MoveAway (room1);
							}
						}
					}
				}
			}
		}
		
		List<Room> roomsToRemove = new List<Room>();
		if (collisions){
			foreach(Room room1 in rooms){
				if (room1 != rooms[0]){
					foreach(Room room2 in rooms){
						if (room1 != room2){
							if (room1.Overlaps(room2)){
								roomsToRemove.Add (room1);
							}
						}
					}
				}
			}
			foreach(Room r in roomsToRemove){
				rooms.Remove (r);
				Destroy(r.gameObject);
			}
		}
	}

	private void ConnectRooms(){

		List<Room> unconnectedRooms = new List<Room>(rooms);
		List<Room> connectedRooms = new List<Room>();

		unconnectedRooms.Remove (rooms[0]);
		connectedRooms.Add (rooms[0]);

		while (unconnectedRooms.Count > 0){
			Room closestConnectedRoom = null;
			Room closestUnconnectedRoom = null;
			float shortestDistance = float.MaxValue;
			foreach (Room connectedRoom in connectedRooms){
				foreach (Room unconnectedRoom in unconnectedRooms){
					float distance = connectedRoom.DistanceTo (unconnectedRoom);
					if (distance < shortestDistance){
						shortestDistance = distance;
						closestConnectedRoom = connectedRoom;
						closestUnconnectedRoom = unconnectedRoom;
					}
				}
			}
			this.Connect (closestConnectedRoom, closestUnconnectedRoom);
			unconnectedRooms.Remove (closestUnconnectedRoom);
			connectedRooms.Add (closestUnconnectedRoom);
		}
	}

	private void Connect(Room r1, Room r2){

		GameObject hall = GameObject.Instantiate (room) as GameObject;
		hall.transform.parent = this.transform;
		halls.Add (hall.GetComponent<Room>());
		hall.GetComponent<Room>().CreateHallway ((int)r1.GetCentre().x,
		                                         (int)r1.GetCentre().y,
		                                         (int)r2.GetCentre().x,
		                                         (int)r2.GetCentre().y);
		
	}

	public void RaiseWalls(){

		for(int x = originX; x < cornerX; x++){
			for(int y = originY; y < cornerY; y++){

				if (tiles.ContainsKey (new Id(x,y)) && tiles[new Id(x,y)].pathable){
					if (!tiles.ContainsKey (new Id(x-1,y))){
						GameObject currentTile = Instantiate(tileObject) as GameObject;
						currentTile.transform.parent = this.transform;
						tiles[new Id(x-1,y)] = currentTile.GetComponent<Tile>();
						tiles[new Id(x-1,y)].SetLocation(x-1, y);
						tiles[new Id(x-1,y)].SetPathable (false);
					}
					if (!tiles.ContainsKey (new Id(x,y+1))){
						GameObject currentTile = Instantiate(tileObject) as GameObject;
						currentTile.transform.parent = this.transform;
						tiles[new Id(x,y+1)] = currentTile.GetComponent<Tile>();
						tiles[new Id(x,y+1)].SetLocation(x,y+1);
						tiles[new Id(x,y+1)].SetPathable (false);
					}
					if (!tiles.ContainsKey (new Id(x+1,y))){
						GameObject currentTile = Instantiate(tileObject) as GameObject;
						currentTile.transform.parent = this.transform;
						tiles[new Id(x+1,y)] = currentTile.GetComponent<Tile>();
						tiles[new Id(x+1,y)].SetLocation(x+1,y);
						tiles[new Id(x+1,y)].SetPathable (false);
					}
					if (!tiles.ContainsKey (new Id(x,y-1))){
						GameObject currentTile = Instantiate(tileObject) as GameObject;
						currentTile.transform.parent = this.transform;
						tiles[new Id(x,y-1)] = currentTile.GetComponent<Tile>();
						tiles[new Id(x,y-1)].SetLocation(x,y-1);
						tiles[new Id(x,y-1)].SetPathable (false);
					}
					if (!tiles.ContainsKey (new Id(x-1,y-1))){
						GameObject currentTile = Instantiate(tileObject) as GameObject;
						currentTile.transform.parent = this.transform;
						tiles[new Id(x-1,y-1)] = currentTile.GetComponent<Tile>();
						tiles[new Id(x-1,y-1)].SetLocation(x-1,y-1);
						tiles[new Id(x-1,y-1)].SetPathable (false);
					}
					if (!tiles.ContainsKey (new Id(x-1,y+1))){
						GameObject currentTile = Instantiate(tileObject) as GameObject;
						currentTile.transform.parent = this.transform;
						tiles[new Id(x-1,y+1)] = currentTile.GetComponent<Tile>();
						tiles[new Id(x-1,y+1)].SetLocation(x-1,y+1);
						tiles[new Id(x-1,y+1)].SetPathable (false);
					}
					if (!tiles.ContainsKey (new Id(x+1,y+1))){
						GameObject currentTile = Instantiate(tileObject) as GameObject;
						currentTile.transform.parent = this.transform;
						tiles[new Id(x+1,y+1)] = currentTile.GetComponent<Tile>();
						tiles[new Id(x+1,y+1)].SetLocation(x+1,y+1);
						tiles[new Id(x+1,y+1)].SetPathable (false);
					}
					if (!tiles.ContainsKey (new Id(x+1,y-1))){
						GameObject currentTile = Instantiate(tileObject) as GameObject;
						currentTile.transform.parent = this.transform;
						tiles[new Id(x+1,y-1)] = currentTile.GetComponent<Tile>();
						tiles[new Id(x+1,y-1)].SetLocation(x+1,y-1);
						tiles[new Id(x+1,y-1)].SetPathable (false);
					}
				}
			}
		}

	}

	public void SetNeighbours(){

		foreach (KeyValuePair<Id, Tile> t in tiles){

			if(t.Value.pathable){

				int x = t.Value.x;
				int y = t.Value.y;

					
				// Left Neighbour
				if (tiles.ContainsKey (new Id(x-1, y)) && tiles[new Id(x-1,y)].pathable){
					tiles[new Id(x, y)].AddNeighbour (tiles[new Id(x-1, y)]);
				}
				
				// Up Neighbour
				if (tiles.ContainsKey (new Id(x, y+1)) && tiles[new Id(x,y+1)].pathable){
					tiles[new Id(x, y)].AddNeighbour (tiles[new Id(x, y+1)]);
				}

				// Right Neighbour
				if (tiles.ContainsKey (new Id(x+1, y)) && tiles[new Id(x+1,y)].pathable){
					tiles[new Id(x, y)].AddNeighbour (tiles[new Id(x+1, y)]);
				}

				// Down Neighbour
				if (tiles.ContainsKey (new Id(x, y-1)) && tiles[new Id(x,y-1)].pathable){
					tiles[new Id(x, y)].AddNeighbour (tiles[new Id(x, y-1)]);
				}

				if (!t.Value.hallway){
					// Down Left Neighbour
					if (tiles.ContainsKey (new Id(x-1, y-1)) && tiles[new Id(x-1,y-1)].pathable && !tiles[new Id(x-1,y-1)].hallway){
						tiles[new Id(x, y)].AddNeighbour (tiles[new Id(x-1, y-1)]);
					}
					// Down Right Neighbour
					if (tiles.ContainsKey (new Id(x+1, y-1)) && tiles[new Id(x+1,y-1)].pathable && !tiles[new Id(x+1,y-1)].hallway){
						tiles[new Id(x, y)].AddNeighbour (tiles[new Id(x+1, y-1)]);
					}
				
					// Up Left Neighbour
					if (tiles.ContainsKey (new Id(x-1, y+1)) && tiles[new Id(x-1,y+1)].pathable && !tiles[new Id(x-1,y+1)].hallway){
						tiles[new Id(x, y)].AddNeighbour (tiles[new Id(x-1, y+1)]);
					}

					// Up Right Neighbour
					if (tiles.ContainsKey (new Id(x+1, y+1)) && tiles[new Id(x+1,y+1)].pathable && !tiles[new Id(x+1,y+1)].hallway){
						tiles[new Id(x, y)].AddNeighbour (tiles[new Id(x+1, y+1)]);
					}
				}
			}
		}				
	}

	public List<Tile> GetPath(Tile from, Tile to){
		// Based on the A* algorithm from the A* wikipedia page.
		// http://en.wikipedia.org/wiki/A*_search_algorithm

		// Tiles that have already been visied.
		List<Tile> closedTiles = new List<Tile>();

		// Tiles that are to be visited.
		List<Tile> openTiles = new List<Tile>();
		openTiles.Add (from);

		// Calculated cost for path distance to tiles.
		Dictionary<Tile, float> actualCost = new Dictionary<Tile, float>();
		actualCost.Add (from, 0.0f);

		// Estimated cost for path distance to tiles.
		Dictionary<Tile, float> estimateCost = new Dictionary<Tile, float>();
		estimateCost.Add (from, 0.0f);

		// The distance of the previous tile with the smallest path.
		Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();

		// While there are tiles to be visited
		while(openTiles.Count != 0){

			// Find the current tile as the tile in the list of open tiles with the smallest estimated distance.
			Tile currentTile = null;
			float minDistance = float.MaxValue;
			foreach(Tile tile in openTiles){
				if (estimateCost[tile] < minDistance){
					currentTile = tile;
					minDistance = estimateCost[tile];
				}
			}

			// If we have found our destination, reconstruct and return the path.
			if (currentTile == to){
				return ReconstructPath (cameFrom, to);
			}


			openTiles.Remove (currentTile);
			closedTiles.Add (currentTile);

			// Check the neighbours of the current tile.
			foreach(Tile neighbour in currentTile.neighbours){
				if (closedTiles.Contains (neighbour)){
					continue;
				}

				// Cost of the path including the distance between the current tile and its neighbour.
				float tenativeCost = actualCost[currentTile] + DistanceBetweenTiles(currentTile, neighbour);

				// Add the new path to the list if it doesn't exist or  the new path is better.
				if (!openTiles.Contains(neighbour) || tenativeCost < actualCost[neighbour]){
					cameFrom[neighbour] = currentTile;
					actualCost[neighbour] = tenativeCost;
					estimateCost[neighbour] = actualCost[neighbour] + DistanceBetweenTiles(currentTile, to);

					if (!openTiles.Contains(neighbour)){
						openTiles.Add (neighbour);
					}
				}
			}
		}

		return new List<Tile>();
	}

	public List<Tile> GetRandomPath(Tile from){

		List<Tile> path = new List<Tile>();

		// If there is no origin tile, there can be no path.
		if (!from){
			return path;
		}

		// Initialize the destination as the original tile.
		Tile destination = from;
		
		// Find a random pathable location in the specified range that is not the original tile.
		while (destination == from)
			destination = rooms[Random.Range (0, rooms.Count)].RandomTile();

		return GetPath (from, destination);
		
	}

	public Vector3 RandomStartPlayer(){
		return new Vector3(rooms[0].GetCentre ().x, rooms[0].GetCentre ().y, -1);
	}

	public Tile RandomStartNPC(){
		return rooms[Random.Range (0,rooms.Count)].RandomTile();
	}

	
	public List<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile current){
		List<Tile> path = new List<Tile>();

		path.Add (current);
		// Retrace the path back to the origin.
		while(cameFrom.ContainsKey (current)){
			current = cameFrom[current];
			path.Add (current);
		}
		return path;
	}
	
	public bool IsPathSafe(Tile tile1, Tile tile2){
		return true;
	}
	
	public float DistanceBetweenTiles(Tile tile1, Tile tile2){

		// If either tile is not pathable, the distance should be infinite.
		if (!tile1.pathable || ! tile2.pathable){
			return float.PositiveInfinity;
		}

		// Return the vector distance between the tiles.
		Vector2 tile1Location = new Vector2(tile1.x, tile1.y);
		Vector2 tile2Location = new Vector2(tile2.x, tile2.y);
		Vector2 distance2DVector = tile2Location - tile1Location;
		
		return distance2DVector.magnitude;
	}

	public Tile GetTile(int x, int y){
		return tiles[new Id(x, y)];
	}

}

