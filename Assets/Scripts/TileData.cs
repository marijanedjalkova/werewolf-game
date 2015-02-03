using UnityEngine;
using System.Collections;

public class TileData : MonoBehaviour {

	public float x;
	public float y;

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
}
