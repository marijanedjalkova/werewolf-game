using UnityEngine;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour {

	public int mapSize = 0;
	public int numberOfNPC = 0;

	public GameObject playerObject;
	public GameObject tilemapObject;
	public GameObject tileObject;
	public GameObject npcObject;
	public GameObject silverObject;

	Tilemap tilemap;

	List<NPC> npcList;
	List<Silver> silverList;

	// Use this for initialization
	void Start () {
		//Pulls the 
		var passObject = GameObject.Find ("passingObject");
		var helpScript = passObject.GetComponent<menuHelper>();
		int map = helpScript.numRooms;
		int npc = helpScript.numNPC;
		mapSize = map;
		numberOfNPC = npc;
		
		GameObject tempTilemapObject = Instantiate(tilemapObject) as GameObject;
		tilemap = tempTilemapObject.GetComponent<Tilemap>();
		tilemap.GenerateTilemap (mapSize);
		playerObject.transform.position = tilemap.RandomStartPlayer();

		npcList = new List<NPC>();
		for(int i = 0; i < numberOfNPC; i++){

			GameObject tempNPCObject = Instantiate(npcObject) as GameObject;
			tempNPCObject.transform.parent = this.transform;
			tempNPCObject.layer = LayerMask.NameToLayer("NPC");

			npcList.Add(tempNPCObject.GetComponent<NPC>());
			npcList[i].SetTilemap(tilemap);
			npcList[i].player = playerObject.GetComponent<Player>();

			Tile currentTile = tilemap.RandomStartNPC();
			npcList[i].SetCurrentTile(currentTile);
			npcList[i].SetLocation (new Vector3(currentTile.x,currentTile.y,-1));

		}

		silverList = new List<Silver> ();
		for (int i = 0; i < numberOfNPC/2; i++) {
			GameObject tempSilverObject = Instantiate(silverObject) as GameObject;
			tempSilverObject.transform.parent = this.transform;
			tempSilverObject.layer = LayerMask.NameToLayer("Silver");

			silverList.Add(tempSilverObject.GetComponent<Silver>());
			Tile currentTile = tilemap.RandomStartNPC();
			silverList[i].SetLocation(new Vector3(currentTile.x,currentTile.y,-1));
		}

		GameObject.FindObjectOfType<Music>().GetComponent<Music>().StartMusic();

	}
	// Update is called once per frame
	void Update () {
	
	}
	
}
