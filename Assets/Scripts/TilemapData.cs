using UnityEngine;
using System.Collections.Generic;

public class TilemapData : MonoBehaviour {

	public int sizeX = 0;
	public int sizeY = 0;

	public GameObject tileObject;

	public TileData[,] tiles;

	// Use this for initialization
	void Start () {
	
		tiles = new TileData[sizeX, sizeY];

		// Create the tile objects
		for(int x = 0; x < sizeX; x++) {
			for(int y = 0; y < sizeY; y++) {
				GameObject currentTile = Instantiate(tileObject) as GameObject;
				currentTile.transform.parent = this.transform;
				tiles[x,y] = currentTile.GetComponent<TileData>();
				tiles[x,y].SetLocation(x, y);
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

	public List<TileData> GetRandomPath(TileData from, TileData minTile, TileData maxTile){

		List<TileData> path = new List<TileData>();

		// If there is no origin tile, there can be no path.
		if (!from){
				return path;
		}

		// Initialize the destination as the original tile.
		int x = from.x;
		int y = from.y;

		// Find a random pathable location in the specified range that is not the original tile.
		while(x == from.x || y == from.y || tiles[x,y].pathable == false){
			x = (int)Random.Range (minTile.x, maxTile.x);
			y = (int)Random.Range (minTile.y, maxTile.y);
		}

		return GetPath (from, tiles[x,y]);

	}

	public List<TileData> GetPath(TileData from, TileData to){
		// Based on the A* algorithm from the A* wikipedia page.
		// http://en.wikipedia.org/wiki/A*_search_algorithm

		// Tiles that have already been visied.
		List<TileData> closedTiles = new List<TileData>();

		// Tiles that are to be visited.
		List<TileData> openTiles = new List<TileData>();
		openTiles.Add (from);

		// Calculated cost for path distance to tiles.
		Dictionary<TileData, float> actualCost = new Dictionary<TileData, float>();
		actualCost.Add (from, 0.0f);

		// Estimated cost for path distance to tiles.
		Dictionary<TileData, float> estimateCost = new Dictionary<TileData, float>();
		estimateCost.Add (from, 0.0f);

		// The distance of the previous tile with the smallest path.
		Dictionary<TileData, TileData> cameFrom = new Dictionary<TileData, TileData>();

		// While there are tiles to be visited
		while(openTiles.Count != 0){

			// Find the current tile as the tile in the list of open tiles with the smallest estimated distance.
			TileData currentTile = null;
			float minDistance = float.MaxValue;
			foreach(TileData tile in openTiles){
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
			foreach(TileData neighbour in currentTile.neighbours){
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
		return null;
	}

	public List<TileData> ReconstructPath(Dictionary<TileData, TileData> cameFrom, TileData current){
		List<TileData> path = new List<TileData>();
		path.Add (current);

		// Retrace the path back to the origin.
		while(cameFrom.ContainsKey (current)){
			current = cameFrom[current];
			path.Add (current);
		}
		return path;
	}

	public float DistanceBetweenTiles(TileData tile1, TileData tile2){

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

	public TileData GetTile(int x, int y){
		return tiles[x, y];
	}

}
