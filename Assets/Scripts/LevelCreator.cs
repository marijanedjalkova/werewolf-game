using UnityEngine;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour {

	public int mapSize = 0;
	public int numberOfNPC = 0;

	public GameObject playerObject;
	public GameObject tilemapObject;
	public GameObject tileObject;
	public GameObject npcObject;

	Tilemap tilemap;

	List<NPC> npcList;

	// Use this for initialization
	void Start () {
		
		GameObject tempTilemapObject = Instantiate(tilemapObject) as GameObject;
		tilemap = tempTilemapObject.GetComponent<Tilemap>();
		tilemap.GenerateTilemap (mapSize);
		playerObject.transform.position = new Vector3(tilemap.RandomStart().x, tilemap.RandomStart().y, -1);

		npcList = new List<NPC>();
		for(int i = 0; i < numberOfNPC; i++){

			GameObject tempNPCObject = Instantiate(npcObject) as GameObject;
			tempNPCObject.transform.parent = this.transform;
			tempNPCObject.layer = LayerMask.NameToLayer("NPC");

			npcList.Add(tempNPCObject.GetComponent<NPC>());
			npcList[i].SetTilemap(tilemap);
			npcList[i].player = playerObject.GetComponent<Player>();

			Tile currentTile = tilemap.RandomStart();
			npcList[i].SetCurrentTile(currentTile);
			npcList[i].SetLocation (new Vector3(currentTile.x,currentTile.y,-1));

		}

	}
	// Update is called once per frame
	void Update () {
	
	}
	
}
