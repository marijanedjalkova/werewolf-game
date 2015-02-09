using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	public int x;
	public int y;
	public bool pathable = true;

	public List<Tile> neighbours;

	// Use this for initialization
	void Start () {
	
		if (this.pathable == true){
			collider2D.isTrigger = true;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetSprite(Sprite s){
		this.GetComponent<SpriteRenderer>().sprite = s;
	}

	public void SetPathable(bool p){
		pathable = p;
	}

	public void SetLocation(int x, int y){
		this.x = x;
		this.y = y;
		this.transform.localPosition = new Vector3(x, y, 0.0f);
	}

	public void AddNeighbour(Tile neighbour){
		neighbours.Add (neighbour);
	}
}
