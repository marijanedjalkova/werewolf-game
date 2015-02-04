using UnityEngine;
using System.Collections.Generic;

public class TileData : MonoBehaviour {

	public float x;
	public float y;
	public bool pathable;

	public List<TileData> neighbours;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetLocation(float x, float y){
		this.x = x;
		this.y = y;
		this.transform.localPosition = new Vector3(x, y, 0.0f);
	}

	public void AddNeighbour(TileData neighbour){
		neighbours.Add (neighbour);
	}
}
