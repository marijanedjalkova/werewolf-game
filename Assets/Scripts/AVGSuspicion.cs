using UnityEngine;
using System.Collections;

public class AVGSuspicion : MonoBehaviour {

	public float barDisplay; //current progress
	public Vector2 pos = new Vector2(60,550);
	public Vector2 size = new Vector2(120,40);
	public Texture2D emptyTex;
	public Texture2D fullTex;
	public GUIStyle suspicion_empty, suspicion_full;
	public float avg_suspicion_level=0.0f;
	
	public void increaseBy(float value){
		avg_suspicion_level += value;
	}
	
	void OnGUI() {
		//draw the background:
		GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
		GUI.Box (new Rect (0, 0, size.x, size.y), emptyTex);
		
		//draw the filled-in part:
		GUI.BeginGroup (new Rect (0, 0, size.x * barDisplay, size.y));
		GUI.Box (new Rect (0, 0, size.x, size.y), fullTex);
		GUI.EndGroup ();
		GUI.EndGroup ();
	}


	// Use this for initialization
	void Start () {
		barDisplay = avg_suspicion_level;
	}
	
	// Update is called once per frame
	void Update () {
		float avg_susp = 0.0f;
		NPC [] npc_list = FindObjectsOfType(typeof(NPC)) as NPC[];
		foreach (NPC obj in npc_list) {
			avg_susp += obj.getSuspicionRate();
		}
		avg_susp /= npc_list.Length;
		avg_suspicion_level = avg_susp;
		barDisplay = avg_suspicion_level;
	}
}
