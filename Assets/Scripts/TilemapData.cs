using UnityEngine;
using System.Collections;

public class TilemapData : MonoBehaviour {

	public int x;
	public int y;

	public GameObject tileObject;

	private ArrayList tiles;

	// Use this for initialization
	void Start () {
	
		tiles = new ArrayList();

		for(int i=0; i < x; i++) {
			for(int j=0; j < y; j++) {
				GameObject tileTemp = Instantiate(tileObject) as GameObject;
				tileTemp.transform.parent = this.transform;
				tileTemp.GetComponent<TileData>().SetLocation(i, j);
				tiles.Add (tileTemp);

			}

		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
