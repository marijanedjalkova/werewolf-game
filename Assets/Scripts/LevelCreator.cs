using UnityEngine;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour {

	public int mapSize = 0;
	public int numberOfNPC = 0;

	public GameObject tilemapObject;
	public GameObject tileObject;
	public GameObject npcObject;

	public string[] npcNames = {
				"NPC1",
				"NPC2",
				"NPC3",
				"NPC4",
				"NPC5",
				"NPC6",
				"NPC7",
				"NPC8",
				"NPC9",
				"NPC10",
				"NPC11",
				"NPC12",
				"NPC13",
				"NPC14",
				"NPC15",
				"NPC16",
				"NPC17",
				"NPC18",
				"NPC19",
				"NPC20"
	};

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
			for(int i = 0; i < 5; i++){
				GameObject tempNPCObject = Instantiate(npcObject) as GameObject;
				tempNPCObject.name = npcNames[i];
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
