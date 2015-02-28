using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	public int x;
	public int y;
	public bool pathable = true;
	public bool hallway;

	public Sprite[] pathableSprites;
	public Sprite[] unpathableSprites;

	public List<Tile> neighbours;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetPathable(bool p){
		pathable = p;

		SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
		collider2D.isTrigger = p;
		if (pathable){
			renderer.sprite = pathableSprites[Random.Range (0, pathableSprites.Length)];
		} else {
			renderer.sprite = unpathableSprites[Random.Range (0, unpathableSprites.Length)];
		}

	}

	public void SetLocation(int x, int y){
		this.x = x;
		this.y = y;
		this.transform.localPosition = new Vector3(x, y, 0.0f);
	}
	public Vector2 GetLocation(){
		return new Vector2(this.x, this.y);
	}

	public void AddNeighbour(Tile neighbour){
		neighbours.Add (neighbour);
	}
}
