using UnityEngine;
using System.Collections.Generic;

public class TileData : MonoBehaviour {

	public int x;
	public int y;
	public bool pathable = true;

	public List<TileData> neighbours;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetPathable(bool p){
		pathable = p;
	}

	public void SetLocation(int x, int y){
		this.x = x;
		this.y = y;
		this.transform.localPosition = new Vector3(x, y, 0.0f);
	}

	public void AddNeighbour(TileData neighbour){
		neighbours.Add (neighbour);
	}
}
