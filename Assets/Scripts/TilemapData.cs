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
			}
		}

		for(int x = 0; x < sizeX; x++) {
			for(int y = 0; y < sizeY; y++) {

				// Left Neighbour
				if (x-1 >= 0){
					tiles[x,y].AddNeighbour (tiles[x-1,y]);
				}

				// Left Neighbour
				if (x+1 < sizeX){
					tiles[x,y].AddNeighbour (tiles[x+1,y]);
				}

				// Below Neighbour
				if (y-1 >= 0){
					tiles[x,y].AddNeighbour (tiles[x,y-1]);
				}

				// Above Neighbour
				if (y+1 < sizeY){
					tiles[x,y].AddNeighbour (tiles[x,y+1]);
				}
			}
		}
	}


	List<TileData> GetPath(TileData from, TileData to){
		List<TileData> path = new List<TileData>();
		return path;
	}

}
