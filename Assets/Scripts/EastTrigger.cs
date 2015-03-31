using UnityEngine;
using System.Collections;

public class EastTrigger : MonoBehaviour {
	Player player;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
		float x = player.GetLocation ().x - 1.0f;
		float y = player.GetLocation ().y;
		transform.position = new Vector2 (x, y);
	}
	
	// Update is called once per frame
	void Update () {
		float x = player.GetLocation ().x - 1.0f;
		float y = player.GetLocation ().y;
		transform.position = new Vector2 (x, y);
	}

	void OnTriggerEnter2D(Collider2D other){
		if(Input.GetKey(KeyCode.Q)&& player.anim.GetBool("right")){
			Debug.Log("KILLING EAST");
			if (other.gameObject.name == "NPC(Clone)" && player.transformed){
				player.kill(other);
			}
		}

	}
}
