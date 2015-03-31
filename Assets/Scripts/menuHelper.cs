using UnityEngine;
using System.Collections;

public class menuHelper : MonoBehaviour {

	public int numRooms = 10;
	public int numNPC = 25;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Make this game object and all its transform children
	// survive when loading a new scene.
	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}
}
