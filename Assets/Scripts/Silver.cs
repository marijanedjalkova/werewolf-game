using UnityEngine;
using System.Collections;

public class Silver : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var health = GameObject.Find ("Health");
		var player = GameObject.Find ("Player");
		float distance = Vector2.Distance (player.transform.position, this.transform.position);
		var healthScript = health.GetComponent<Health>();
		var playerScript = player.GetComponent<Player>();

		if (playerScript.transformed) {
			if (distance < 0.5) {
				healthScript.decreaseBy (0.01f);
			} else if (distance < 1) {
				healthScript.decreaseBy (0.005f);
			} else if (distance < 1.5) {
				healthScript.decreaseBy (0.002f);
			} else if (distance < 2) {
				healthScript.decreaseBy (0.001f);
			}
		}
	}

	public void SetLocation(Vector3 loc){
		this.transform.position = loc;
	}
	
}
