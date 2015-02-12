using UnityEngine;
using System.Collections;

public class Hunger : MonoBehaviour
{

	public float barDisplay; //current progress
	public Vector2 pos = new Vector2(20,550);
	public Vector2 size = new Vector2(120,40);
	public Texture2D emptyTex;
	public Texture2D fullTex;
	public GUIStyle progress_empty, progress_full;
	public float hunger_level=0.0f;

	public void increaseBy(float value){
		hunger_level += value;
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
		void Start ()
		{
			barDisplay = hunger_level;
		}
	
		// Update is called once per frame
		void Update ()
		{
		barDisplay = hunger_level;
		if (hunger_level == 1) {
			Application.LoadLevel("won_level");
				}

	}
}

