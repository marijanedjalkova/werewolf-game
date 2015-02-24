using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public GameObject tileObject;
	public Tile[,] tiles;

	public bool hallway = false;

	public int originX = 0;
	public int originY = 0;

	public int sizeX = 0;
	public int sizeY = 0;

	// Use this for initialization
	void Start () {

	}

	public void CreateRoom(int originX, int originY, int sizeX, int sizeY){

		this.originX = originX;
		this.originY = originY;

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
					tiles[x,y].SetLocation(originX+x, originY+y);
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
				tiles[x,y].SetLocation(originX+x, originY+y);
				tiles[x,y].SetPathable (true);
			}
		}

	}

	public void CreateHallway(int startx, int starty, int endx, int endy){
		hallway = true;
		this.sizeX = Mathf.Abs (startx - endx) + 2;
		this.sizeY = Mathf.Abs (starty - endy) + 2;

		this.originX = Mathf.Min (startx, endx);
		this.originY = Mathf.Min (starty, endy);

		tiles = new Tile[sizeX, sizeY];

		int x = startx-originX;
		int y = starty-originY;

		while (x != endx-originX){

			if (tiles[x,y] == null){
				GameObject currentTile = Instantiate(tileObject) as GameObject;
				currentTile.transform.parent = this.transform;
				tiles[x,y] = currentTile.GetComponent<Tile>();
				tiles[x,y].SetLocation(originX+x, originY+y);
				tiles[x,y].SetPathable (true);
			}

			if (tiles[x,y+1] == null){
				GameObject currentTile = Instantiate(tileObject) as GameObject;
				currentTile.transform.parent = this.transform;
				tiles[x,y+1] = currentTile.GetComponent<Tile>();
				tiles[x,y+1].SetLocation(originX+x, originY+y+1);
				tiles[x,y+1].SetPathable (true);
			}

			x += x < endx-originX ? 1 : -1;
		}

		if (starty > endy){

			if (tiles[x,y+1] == null){
				GameObject currentTile = Instantiate(tileObject) as GameObject;
				currentTile.transform.parent = this.transform;
				tiles[x,y+1] = currentTile.GetComponent<Tile>();
				tiles[x,y+1].SetLocation(originX+x, originY+y+1);
				tiles[x,y+1].SetPathable (true);
			}
			
			if (startx < endx){

				if (tiles[x+1,y+1] == null){
					GameObject currentTile = Instantiate(tileObject) as GameObject;
					currentTile.transform.parent = this.transform;
					tiles[x+1,y+1] = currentTile.GetComponent<Tile>();
					tiles[x+1,y+1].SetLocation(originX+x+1, originY+y+1);
					tiles[x+1,y+1].SetPathable (true);
				}

			} 
		}

		while (y != endy-originY){

			if (tiles[x,y] == null){
				GameObject currentTile = Instantiate(tileObject) as GameObject;
				currentTile.transform.parent = this.transform;
				tiles[x,y] = currentTile.GetComponent<Tile>();
				tiles[x,y].SetLocation(originX+x, originY+y);
				tiles[x,y].SetPathable (true);
			}

			if (tiles[x+1,y] == null){
				GameObject currentTile = Instantiate(tileObject) as GameObject;
				currentTile.transform.parent = this.transform;
				tiles[x+1,y] = currentTile.GetComponent<Tile>();
				tiles[x+1,y].SetLocation(originX+x+1, originY+y);
				tiles[x+1,y].SetPathable (true);
			}

			y += y < endy-originY ? 1 : -1;
		}
		
		
	}

	public void MoveAway(Room otherRoom){

		Vector2 vectorBetween = this.Centre () - otherRoom.Centre ();

		int moveX = System.Math.Sign (vectorBetween.x);
		int moveY = System.Math.Sign (vectorBetween.y);

		if (moveX == 0 && moveY == 0){
			moveX = Random.Range (-1, 1);
			moveY = Random.Range (-1, 1);
		}

		UpdateLocations (this.originX + moveX, this.originY + moveY);
	}

	public float DistanceTo(Room otherRoom){

		Room left;
		Room right;

		if (this.originX < otherRoom.originX){
			left = this;
		} else {
			left = otherRoom;
		}
		if (otherRoom.originX < this.originX){
			right = this;
		} else {
			right = otherRoom;
		}
		float xDifference;
		if (left.originX == right.originX){
			xDifference = 0.0f;
		} else {
			xDifference = right.originX - (left.originX + left.sizeX);
		}
		xDifference = Mathf.Max (0.0f, xDifference);

		Room up;
		Room down;

		if (this.originY < otherRoom.originY){
			up = this;
		} else {
			up = otherRoom;
		}

		if (otherRoom.originY < this.originY){
			down = this;
		} else {
			down = otherRoom;
		}

		float yDifference;
		if (up.originY == down.originY){
			yDifference = 0.0f;
		} else {
			yDifference = down.originY - (up.originY + up.sizeY);
		}
		yDifference = Mathf.Max (0.0f, yDifference);

		Vector2 diff = new Vector2(xDifference, yDifference);
		return diff.magnitude;
	}

	public Tile RandomTile(){

		Tile random = tiles[Random.Range (0, sizeX), Random.Range (0, sizeY)];

		while (!random.pathable){
			random = tiles[Random.Range (0, sizeX), Random.Range (0, sizeY)];
		}

		return random;

	}

	public Vector2 Centre(){
		return new Vector2(this.originX+this.sizeX/2, this.originY+this.sizeY/2);
	}

	public void UpdateLocations(int newBottomX, int newLeftY){

		originX = newBottomX;
		originY = newLeftY;

		for(int x = 0; x < sizeX;x++){
			for(int y = 0; y < sizeY; y++) {
				tiles[x, y].SetLocation(originX+x, originY+y);
			}
		}
	}

	public bool Overlaps(Room otherRoom){
		return
			(this.originX < otherRoom.originX + otherRoom.sizeX &&
			 this.originX + this.sizeX > otherRoom.originX &&
			 this.originY < otherRoom.originY + otherRoom.sizeY &&
			 this.sizeY + this.originY > otherRoom.originY);
	}

}
