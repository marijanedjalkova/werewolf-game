using UnityEngine;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour {

	public int mapSize = 0;
	public int numberOfNPC = 0;

	public GameObject tilemapObject;
	public GameObject tileObject;
	public GameObject npcObject;

	Tilemap tilemap;

	List<NPC> npcList;

	// Use this for initialization
	void Start () {

		if (mapSize > 3){

			GameObject tempTilemapObject = Instantiate(tilemapObject) as GameObject;
			tempTilemapObject.transform.parent = this.transform;
			tilemap = tempTilemapObject.GetComponent<Tilemap>();
			tilemap.SetTile (tileObject);
			tilemap.GenerateTilemap (mapSize, mapSize);

			npcList = new List<NPC>();
			for(int i = 0; i < numberOfNPC; i++){
				GameObject tempNPCObject = Instantiate(npcObject) as GameObject;
				tempNPCObject.transform.parent = this.transform;
				npcList.Add(tempNPCObject.GetComponent<NPC>());
				npcList[i].SetTilemap(tilemap);
				Tile currentTile = tilemap.RandomTile();
				npcList[i].SetCurrentTile(currentTile);
				npcList[i].SetLocation (new Vector3(currentTile.x,currentTile.y,-1));
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
