using UnityEngine;
using System.Collections;

public class SuspicionBar : MonoBehaviour
{

	public float barDisplay; //current progress
	public Vector2 pos;
	public Vector2 ViewportPoint;
	public Vector2 size = new Vector2(120,80);
	public Texture2D emptyTex;
	public Texture2D fullTex;
	public GUIStyle progress_empty, progress_full;
	public float susp_level=0.0f;
	public RectTransform rectTransform;

	private NPC parent_npc;
	
	public void increaseBy(float value){
		susp_level += value;
	}

	/*
	public SuspicionBar(NPC player){
		//pos = camera.WorldToScreenPoint(player.position).XY();
		parent_npc = player;
		this.transform.parent = parent_npc.transform;
	}
*/


	void OnGUI() {
		//draw the background:
		pos = this.transform.position;
		pos.x -= 0.25f;// get the game object position
		pos.y += 0.6f;
		ViewportPoint = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint
		
		GUI.BeginGroup (new Rect (ViewportPoint.x*Camera.main.pixelWidth, Camera.main.pixelHeight - ViewportPoint.y*Camera.main.pixelHeight, size.x, size.y));
		
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
			
			pos = this.transform.position;  // get the game object position
			ViewportPoint = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint
			
			barDisplay = susp_level;
		}
	
		// Update is called once per frame
		void Update ()
		{
			barDisplay = susp_level;
		}

	public float get_suspicion(){
		return susp_level;
	}

	public void set_suspicion(float susp){
		susp_level = susp;
	}
}

