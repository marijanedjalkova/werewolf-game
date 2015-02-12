using UnityEngine;
using System.Collections.Generic;

public class Tilemap : MonoBehaviour {

	public int sizeX = 0;
	public int sizeY = 0;

	public GameObject tileObject;

	public Tile[,] tiles;

	public Sprite pathable;
	public Sprite unpathable;
	
	// Use this for initialization
	void Start () {
	
	}

	public void SetTile(GameObject tile){
		this.tileObject = tile;
	}

	public void GenerateTilemap(int sizeX, int sizeY){

		this.sizeX = sizeX;
		this.sizeY = sizeY;

		tiles = new Tile[sizeX, sizeY];

		for(int x = 0; x < sizeX;x++){
			for(int y = 0; y < sizeY; y++) {
				if (x == 0 || x == sizeX-1 ||
				    y == 0 || y == sizeY-1){
					GameObject currentTile = Instantiate(tileObject) as GameObject;
					currentTile.transform.parent = this.transform;
					tiles[x,y] = currentTile.GetComponent<Tile>();
					tiles[x,y].SetLocation(x, y);
					tiles[x,y].SetSprite (unpathable);
					tiles[x,y].SetPathable (false);
				}
			}
					
		}

		// Create the tile objects
		for(int x = 1; x < sizeX-1; x++) {
			for(int y = 1; y < sizeY-1; y++) {
				GameObject currentTile = Instantiate(tileObject) as GameObject;
				currentTile.transform.parent = this.transform;
				tiles[x,y] = currentTile.GetComponent<Tile>();
				tiles[x,y].SetLocation(x, y);
				tiles[x,y].SetSprite (pathable);
				tiles[x,y].SetPathable (true);
			}
		}

		for(int x = 0; x < sizeX; x++) {
			for(int y = 0; y < sizeY; y++) {

				// Down Left Neighbour
				if (x-1 >= 0 && y-1 >= 0){
					tiles[x,y].AddNeighbour (tiles[x-1,y-1]);
				}

				// Left Neighbour
				if (x-1 >= 0){
					tiles[x,y].AddNeighbour (tiles[x-1,y]);
				}

				// Up Left Neighbour
				if (x-1 >= 0 && y+1 < sizeY){
					tiles[x,y].AddNeighbour (tiles[x-1,y+1]);
				}
				
				// Up Neighbour
				if (y+1 < sizeY){
					tiles[x,y].AddNeighbour (tiles[x,y+1]);
				}

				// Up Right Neighbour
				if (y+1 < sizeY && x+1 < sizeX){
					tiles[x,y].AddNeighbour (tiles[x+1,y+1]);
				}
				
				// Right Neighbour
				if (x+1 < sizeX){
					tiles[x,y].AddNeighbour (tiles[x+1,y]);
				}

				// Down Right Neighbour
				if (x+1 < sizeX && y-1 >= 0){
					tiles[x,y].AddNeighbour (tiles[x+1,y-1]);
				}
				
				// Down Neighbour
				if (y-1 >= 0){
					tiles[x,y].AddNeighbour (tiles[x,y-1]);
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

	public List<Tile> GetRandomPath(Tile from, Tile minTile, Tile maxTile){

		List<Tile> path = new List<Tile>();
		
		if (minTile == maxTile || from.pathable == false){
			return path;
		}
		
		// If there is no origin tile, there can be no path.
		if (!from){
			return path;
		}
		
		// Initialize the destination as the original tile.
		Tile destination = from;
		
		// Find a random pathable location in the specified range that is not the original tile.
		while(destination == from || destination.pathable == false){
			destination = RandomTileInRegion(minTile, maxTile);
		}
		
		return GetPath (from, destination);
		
	}
	
	public Tile RandomTile(){
		return RandomTileInRegion(tiles[0,0],tiles[sizeX-1, sizeY-1]);
	}

	public Tile RandomTileInRegion(Tile minTile, Tile maxTile){
		int x = (int)Random.Range (minTile.x, maxTile.x+1);
		int y = (int)Random.Range (minTile.y, maxTile.y+1);
		if (tiles[x,y].pathable == true)
			return tiles[x,y];		
		else
			return RandomTileInRegion(minTile, maxTile);
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
		return tiles[x, y];
	}

}

